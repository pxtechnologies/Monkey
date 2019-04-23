using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Monkey.Builder;
using Monkey.Generator;
using Monkey.Logging;
using Monkey.Sql.Builder;
using Monkey.Sql.Model;


namespace Monkey.Sql.Generator
{
    internal class SqlCqrsGenerator : ISqlCqrsGenerator
    {
        private readonly IRepository _repo;

        public SqlCqrsGenerator(IRepository repo)
        {
            _repo = repo;
        }

        public async Task<SourceUnitCollection> Generate(long version)
        {
            SourceUnitCollection collection = new SourceUnitCollection();

            foreach (var proc in await _repo.Query<ProcedureDescriptor>().ToArrayAsync())
            {
                foreach (var u in await GenerateCqrsHandlerForProcedure(proc))
                    collection.Append(u);
            }

            return collection;
        }

        private ILogger _logger;
        private async Task<SourceUnit[]> GenerateCqrsHandlerForProcedure(ProcedureDescriptor proc)
        {
            SourceUnit[] result = new SourceUnit[3];

            var procBinding = await _repo.Query<ProcedureBinding>()
                .Include(x=>x.Result)
                .Include(x=>x.Result.Properties)
                .Include(x=>x.Request)
                .Include(x => x.Request.Properties)
                .FirstOrDefaultAsync(x => x.ProcedureId == proc.Id);

            if (procBinding == null)
            {
                _logger.Warn("Procedure {ProcedureName} has no bindings.", proc.Name);
                return new SourceUnit[0];
            }

            if (procBinding.Mode == Mode.Command)
            {
                result[1] = await GenerateCommand(proc, procBinding.Mode, procBinding.Request);
                result[0] = await GenerateCommandHandler(proc, procBinding);

            }
            else
            {
                result[1] = await GenerateQuery(proc, procBinding.Mode, procBinding.Request);
                result[0] = await GenerateQueryHandler(proc, procBinding);
            }
            result[2] = await GenerateResult(proc, procBinding.Result);

            return result;
        }

        private async Task<SourceUnit> GenerateQueryHandler(ProcedureDescriptor proc, ProcedureBinding procBinding)
        {
            var paramBindings = await _repo.Query<ProcedureParameterBinding>()
                .Where(x => x.Parameter.ProcedureId == proc.Id)
                .Include(x => x.Parameter)
                .Include(x => x.Property)
                .ToArrayAsync();

            var resultBindings = await _repo.Query<ProcedureResultColumnBinding>()
                .Where(x => x.ResultColumn.ProcedureId == proc.Id)
                .Include(x => x.ResultColumn)
                .Include(x => x.Property)
                .ToArrayAsync();

            var handlerTypeName = $"{procBinding.Name}{procBinding.Mode.ToString()}Handler";
            var handlerBuilder = new SqlQueryHandlerBuilder()
                .InNamespace(proc.Schema)
                .WithName(handlerTypeName)
                .WithProcedureName(proc.Name, proc.ConnectionName)
                .WithQueryTypeName(procBinding.Request.FullName())
                .WithResultTypeName(procBinding.Result.FullName())
                .AddDefaultUsings();

            foreach (var p in paramBindings)
            {
                handlerBuilder.BindParameter(p.Property.Name, p.Parameter.Name);
            }

            foreach (var rCol in resultBindings)
            {
                handlerBuilder.BindColumnResult(rCol.Property.Name,
                    rCol.Property.PropertyType.FullName(),
                    rCol.ResultColumn.Name);
            }

            return new SourceUnit(proc.Schema,
                handlerTypeName,
                handlerBuilder.GenerateCode());
        }

        private async Task<SourceUnit> GenerateResult(ProcedureDescriptor proc, Result obj)
        {
            DataClassBuilder builder = new DataClassBuilder()
                .InNamespace(obj.Namespace)
                .WithName(obj.Name);

            foreach (var p in obj.Properties)
                builder.WithProperty(p.PropertyType.SrcName(), p.Name);

            return new SourceUnit(obj.Namespace, obj.Name, builder.GenerateCode());
        }
        private async Task<SourceUnit> GenerateQuery(ProcedureDescriptor proc, Mode mode, ObjectType obj)
        {
            if (mode != Mode.Command && obj is Command)
                throw new InvalidOperationException("Query mode cannot be used with command object.");
            if (mode != Mode.Query && obj is Query)
                throw new InvalidOperationException("Command mode cannot be used with query object.");

            DataClassBuilder builder = new DataClassBuilder()
                .InNamespace(obj.Namespace)
                .WithName(obj.Name);

            foreach (var p in obj.Properties)
                builder.WithProperty(p.PropertyType.SrcName(), p.Name);

            return new SourceUnit(obj.Namespace, obj.Name, builder.GenerateCode());
        }

        private async Task<SourceUnit> GenerateCommand(ProcedureDescriptor proc, Mode mode, ObjectType obj)
        {
            if(mode != Mode.Command && obj is Command)
                throw new InvalidOperationException("Query mode cannot be used with command object.");
            if(mode != Mode.Query && obj is Query)
                throw new InvalidOperationException("Command mode cannot be used with query object.");

            DataClassBuilder builder = new DataClassBuilder()
                .InNamespace(obj.Namespace )
                .WithName(obj.Name);

            foreach (var p in obj.Properties)
                builder.WithProperty(p.PropertyType.SrcName(), p.Name);

            return new SourceUnit(obj.Namespace, obj.Name, builder.GenerateCode());
        }

        private async Task<SourceUnit> GenerateCommandHandler(ProcedureDescriptor proc, ProcedureBinding procBinding)
        {
            var paramBindings = await _repo.Query<ProcedureParameterBinding>()
                .Where(x => x.Parameter.ProcedureId == proc.Id)
                .Include(x => x.Parameter)
                .Include(x => x.Property)
                .ToArrayAsync();

            var resultBindings = await _repo.Query<ProcedureResultColumnBinding>()
                .Where(x => x.ResultColumn.ProcedureId == proc.Id)
                .Include(x => x.ResultColumn)
                .Include(x => x.Property)
                .ToArrayAsync();

            var handlerTypeName = $"{procBinding.Name}{procBinding.Mode.ToString()}Handler";
            var handlerBuilder = new SqlCommandHandlerBuilder()
                .InNamespace(proc.Schema)
                .WithName(handlerTypeName)
                .WithProcedureName(proc.Name, proc.ConnectionName)
                .WithCommandTypeName(procBinding.Request.FullName())
                .WithResultTypeName(procBinding.Result.FullName())
                .AddDefaultUsings();

            foreach (var p in paramBindings)
            {
                handlerBuilder.BindParameter(p.Property.Name, p.Parameter.Name);
            }

            foreach (var rCol in resultBindings)
            {
                handlerBuilder.BindColumnResult(rCol.Property.Name,
                    rCol.Property.PropertyType.FullName(),
                    rCol.ResultColumn.Name);
            }

            return new SourceUnit(proc.Schema,
                handlerTypeName,
                handlerBuilder.GenerateCode());
        }
    }
}
