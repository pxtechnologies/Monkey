using System;
using System.Runtime.Serialization;
using Microsoft.CodeAnalysis;

namespace Monkey.Compilation
{

    public class CompilationException : Exception
    {
        public Diagnostic[] Errors { get; set; }
        public CompilationException()
        {
        }

        protected CompilationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public CompilationException(string message) : base(message)
        {
        }

        public CompilationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}