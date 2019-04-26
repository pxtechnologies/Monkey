using System;
using System.Runtime.Serialization;

namespace Monkey.Sql.Scripts
{
    [Serializable]
    public class MigrationException : Exception
    {
        public MigrationException()
        {
        }

        protected MigrationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public MigrationException(string message) : base(message)
        {
        }

        public MigrationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}