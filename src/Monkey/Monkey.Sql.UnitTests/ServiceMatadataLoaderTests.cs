using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using Monkey.Generator;
using Monkey.Logging;
using Monkey.Patterns.UnitTests;
using Monkey.PubSub;
using Monkey.Sql.Generator;
using Monkey.Sql.Model;
using Monkey.Sql.Services;
using NSubstitute;
using Xunit;

namespace Monkey.Sql.UnitTests
{
    public class ServiceMatadataLoaderTests
    {
        ServiceMatadataLoaderFixture Fixture = new ServiceMatadataLoaderFixture();

        class ServiceMatadataLoaderFixture : TestFor<ServiceMatadataLoader>
        {
            private string _nodeName;
            public IServiceMetadataProvider MetadataProvider { get; set; }
            public ISqlCqrsGenerator SqlCqrsGenerator { get; set; }
            public IDynamicTypePool DynamicTypePool { get; set; }
            public ILogger Logger { get; set; }
            public IEventHub EventHub { get; set; }
            public IRepository Repository { get; set; }
            public INodeNameProvider NodeNameProvider { get; set; }

            public string NodeName
            {
                get { return _nodeName; }
                
            }

            public ServiceMatadataLoaderFixture()
            {
                SqlCqrsGenerator
                    .Generate(Arg.Any<long>())
                    .Returns(new SourceUnitCollection());
            }
            protected override ServiceMatadataLoader CreateSut()
            {
                return new ServiceMatadataLoader(MetadataProvider, 
                    SqlCqrsGenerator, 
                    DynamicTypePool, 
                    Logger,
                    EventHub, 
                    Repository, 
                    NodeNameProvider);
            }
            public ServiceMatadataLoaderFixture WithNodeName(string name = "node1")
            {
                NodeNameProvider.Name().Returns(name);
                _nodeName = name;
                return this;
            }
            public ServiceMatadataLoaderFixture With<T>(params T[] w) where T:class
            {
                Repository.Query<T>().Returns(new TestAsyncEnumerable<T>(w.AsEnumerable()));
                return this;
            }
            
        }

        [Fact]
        public async Task FirstEmptyRun()
        {
            Fixture.WithNodeName()
                .With<Workspace>();

            await Fixture.Sut.Load();

            await Fixture.SqlCqrsGenerator.DidNotReceive().Generate(Arg.Any<long>());
            Fixture.MetadataProvider.DidNotReceive().GetServices();
        }
        
        [Fact]
        public async Task FirstRunCleansWorkspaces()
        {
            var loaded = new Workspace() { Status = WorkspaceStatus.Loaded, NodeName = "node1" };
            var running = new Workspace() { Status = WorkspaceStatus.Running, NodeName = "node1" };

            Fixture.WithNodeName()
                .With(loaded, running);

            
            await Fixture.Sut.Load();

            loaded.Status.Should().Be(WorkspaceStatus.Compiled);
            running.Status.Should().Be(WorkspaceStatus.Compiled);
            await Fixture.Repository.Received(1).CommitChanges();
        }

        [Fact]
        public async Task FirstRunChecksModel_WhenEmptyWorkspace()
        {
            var empty = new Workspace() { Status = WorkspaceStatus.Created };
            Fixture.WithNodeName()
                .With(empty);

            await Fixture.Sut.Load();

            await Fixture.SqlCqrsGenerator.Received(1).Generate(Arg.Any<long>());
        }
        [Fact]
        public async Task FirstRunChecksModel_WhenRunOnPrv()
        {
            var empty = new Workspace() { Status = WorkspaceStatus.Created, NodeName = "node1"};
            Fixture.WithNodeName()
                .With(empty);

            await Fixture.Sut.Load();

            await Fixture.SqlCqrsGenerator.Received(1).Generate(Arg.Any<long>());
        }
        [Fact]
        public async Task FirstRunChecksModel_WhenRunOnNode2()
        {
            var empty = new Workspace() { Status = WorkspaceStatus.Created, NodeName = "node2" };
            Fixture.WithNodeName()
                .With(empty);

            await Fixture.Sut.Load();

            await Fixture.SqlCqrsGenerator.Received(1).Generate(Arg.Any<long>());
        }
        AssemblyPurpose purpose = AssemblyPurpose.Handlers |
                                  AssemblyPurpose.Queries |
                                  AssemblyPurpose.Commands |
                                  AssemblyPurpose.Results;
        [Fact]
        public async Task FirstRunShouldLoadPrvAssemblies()
        {
            var w = new Workspace() { Status = WorkspaceStatus.Compiled, NodeName = "node1", Id=123 };
            var loc = this.GetType().Assembly.Location;
            var c = new Model.Compilation() {
                Assembly = File.ReadAllBytes(loc),
                Hash = Guid.NewGuid(),
                Workspace = w,
                WorkspaceId = 123,
                Purpose = purpose
            };
            Fixture.WithNodeName()
                .With(w)
                .With(c);

            await Fixture.Sut.Load();

            Fixture.DynamicTypePool.Received(1)
                .AddOrReplace(Arg.Is<DynamicAssembly>(y => y.SrcHash == c.Hash));
            await Fixture.Repository.Received().CommitChanges();
            w.Status.Should().Be(WorkspaceStatus.Running);
            Fixture.MetadataProvider.Discover(Arg.Is<Assembly[]>(y => y[0].Location == loc));

        }
    }
}