using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Monkey.Generator;

namespace Monkey.Sql.Model
{
    public class Compilation
    {
        public long Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset CompiledAt { get; set; }
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
