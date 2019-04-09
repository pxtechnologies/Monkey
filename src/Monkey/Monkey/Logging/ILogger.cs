using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.Operations;

namespace Monkey.Logging
{
    public interface ILogger
    {
        void Info(string text, params object[] args);
        void Warn(string text, params object[] args);
        void Error(string text, params object[] args);
        void Debug(string text, params object[] args);
    }

    public class ConsoleLogger : ILogger
    {
        private void Write(string text, object[] args)
        {
            int i = 0;
            int index = text.IndexOf('{');
            int index2 = -1;
            while (index >= 0)
            {
                index2 = text.IndexOf('}', index);
                if (index2 > index)
                {
                    text = text.Remove(index, index2 - index);
                    text = text.Insert(index, $"{{{i++}}}");
                    index = text.IndexOf('{', index2);
                }
                else break;
            }
            Console.WriteLine(text, args);
        }
        public void Info(string text, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Write(text, args);
        }

        public void Warn(string text, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Write(text, args);
        }

        public void Error(string text, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Write(text, args);
        }

        public void Debug(string text, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Write(text, args);
        }
    }
}
