using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Monkey.Generator;
using NSubstitute.Core;

namespace Monkey.Sql.Model
{
    public enum WorkspaceStatus
    {
        Created = 0,
        Error = -1,
        Compiled = 1,
        Loaded = 2,
        Running = 3
    }

    public class Workspace
    {
        public Workspace()
        {
            Compilations = new List<Compilation>();
            VersionSignature = Guid.NewGuid();
            Created = DateTimeOffset.Now;
            HeartBeat = Created;
            Status = WorkspaceStatus.Created;
        }
        public bool IsDisabled { get; set; }
        public long Id { get; set; }
        public Guid VersionSignature { get; set; }
        public WorkspaceStatus Status { get; set; }
        public DateTimeOffset HeartBeat { get; set; }
        public DateTimeOffset Created { get; set; }
        public string NodeName { get; set; }
        public string Error { get; set; }
        public virtual ICollection<Compilation> Compilations { get; set; }
    }
    public class Compilation
    {
        public long Id { get; set; }
        public int Version { get; set; }
        public virtual long? WorkspaceId { get; set; }
        public virtual Workspace Workspace { get; set; }

        public DateTimeOffset CompiledAt { get; set; }
        public DateTimeOffset LoadedAt { get; set; }
        public TimeSpan CompilationDuration { get; set; }
        public Guid Hash { get; set; }
        public string Source { get; set; }
        public AssemblyPurpose Purpose { get; set; }
        public byte[] Assembly { get; set; }
        public string Classes { get; set; }

        [MaxLength(255)]
        public string ServerName { get; set; }
        public string Errors { get; set; }
    }
}
