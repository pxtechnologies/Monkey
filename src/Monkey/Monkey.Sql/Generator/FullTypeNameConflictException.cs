using System;

namespace Monkey.Sql.Generator
{
    public class FullTypeNameConflictException : Exception
    {
        public FullTypeNameConflictException(string conflictingName) : base($"Class full type name conflict: {conflictingName}")
        {
            ConflictingName = conflictingName;
        }

        public string ConflictingName { get; private set; }
    }
}