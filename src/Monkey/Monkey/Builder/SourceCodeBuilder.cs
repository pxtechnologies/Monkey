using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Monkey.Builder
{
    public sealed class SourceCodeBuilder
    {
        private StringBuilder _sb;
        private int _indent = 0;
        private string _linePrefix = "";
        private string _tab = "   ";
        public SourceCodeBuilder IndentUp()
        {
            _indent += 1;
            _linePrefix = string.Concat(Enumerable.Repeat(_tab, _indent));
            return this;
        }

        public SourceCodeBuilder IndentDown()
        {
            _indent -= 1;
            _linePrefix = string.Concat(Enumerable.Repeat(_tab, _indent));
            return this;
        }
        public SourceCodeBuilder CloseBlock()
        {
            return this.IndentDown().AppendLine("}");
        }
        public SourceCodeBuilder OpenBlock()
        {
            return this.AppendLine("{").IndentUp();

        }
        public SourceCodeBuilder()
        {
            _sb = new StringBuilder();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            ((ISerializable)_sb).GetObjectData(info, context);
        }

        public SourceCodeBuilder Append(bool value)
        {
            _sb.Append(value); return this;
            return this;
        }

        public SourceCodeBuilder Append(byte value)
        {
            _sb.Append(value); return this;
        }

        public SourceCodeBuilder Append(char value)
        {
            _sb.Append(value); return this;
        }


        public SourceCodeBuilder Append(char value, int repeatCount)
        {
            _sb.Append(value, repeatCount); return this;
        }

        public SourceCodeBuilder Append(char[] value)
        {
            _sb.Append(value); return this;
        }

        public SourceCodeBuilder Append(char[] value, int startIndex, int charCount)
        {
            _sb.Append(value, startIndex, charCount); return this;
        }

        public SourceCodeBuilder Append(decimal value)
        {
            _sb.Append(value); return this;
        }

        public SourceCodeBuilder Append(double value)
        {
            _sb.Append(value); return this;
        }

        public SourceCodeBuilder Append(short value)
        {
            _sb.Append(value); return this;
        }

        public SourceCodeBuilder Append(int value)
        {
            _sb.Append(value); return this;
        }

        public SourceCodeBuilder Append(long value)
        {
            _sb.Append(value); return this;
        }

        public SourceCodeBuilder Append(object value)
        {
            _sb.Append(value); return this;
        }

        public SourceCodeBuilder Append(ReadOnlySpan<char> value)
        {
            _sb.Append(value); return this;
        }

        public SourceCodeBuilder Append(sbyte value)
        {
            _sb.Append(value); return this;
        }

        public SourceCodeBuilder Append(float value)
        {
            _sb.Append(value); return this;
        }

        public SourceCodeBuilder Append(string value)
        {
            _sb.Append(value); return this;
        }

        public SourceCodeBuilder Append(string value, int startIndex, int count)
        {
            _sb.Append(value, startIndex, count); return this;
        }

        public SourceCodeBuilder Append(SourceCodeBuilder value)
        {
            _sb.Append(value); return this;
        }

        public SourceCodeBuilder Append(SourceCodeBuilder value, int startIndex, int count)
        {
            _sb.Append(value._sb, startIndex, count); return this;
        }

        public SourceCodeBuilder Append(ushort value)
        {
            _sb.Append(value); return this;
        }

        public SourceCodeBuilder Append(uint value)
        {
            _sb.Append(value); return this;
        }

        public SourceCodeBuilder Append(ulong value)
        {
            _sb.Append(value); return this;
        }

        public SourceCodeBuilder AppendFormat(IFormatProvider provider, string format, object arg0)
        {
            _sb.AppendFormat(provider, format, arg0); return this;
        }

        public SourceCodeBuilder AppendFormat(IFormatProvider provider, string format, object arg0, object arg1)
        {
            _sb.AppendFormat(provider, format, arg0, arg1); return this;
        }

        public SourceCodeBuilder AppendFormat(IFormatProvider provider, string format, object arg0, object arg1, object arg2)
        {
            _sb.AppendFormat(provider, format, arg0, arg1, arg2); return this;
        }

        public SourceCodeBuilder AppendFormat(IFormatProvider provider, string format, params object[] args)
        {
            _sb.AppendFormat(provider, format, args); return this;
        }

        public SourceCodeBuilder AppendFormat(string format, object arg0)
        {
            _sb.AppendFormat(format, arg0); return this;
        }

        public SourceCodeBuilder AppendFormat(string format, object arg0, object arg1)
        {
            _sb.AppendFormat(format, arg0, arg1); return this;
        }

        public SourceCodeBuilder AppendFormat(string format, object arg0, object arg1, object arg2)
        {
            _sb.AppendFormat(format, arg0, arg1, arg2); return this;
        }

        public SourceCodeBuilder AppendFormat(string format, params object[] args)
        {
            _sb.AppendFormat(format, args); return this;
        }

        public SourceCodeBuilder AppendJoin(char separator, params object[] values)
        {
            _sb.AppendJoin(separator, values); return this;
        }

        public SourceCodeBuilder AppendJoin(char separator, params string[] values)
        {
            _sb.AppendJoin(separator, values); return this;
        }

        public SourceCodeBuilder AppendJoin(string separator, params object[] values)
        {
            _sb.AppendJoin(separator, values); return this;
        }

        public SourceCodeBuilder AppendJoin(string separator, params string[] values)
        {
            _sb.AppendJoin(separator, values); return this;
        }

        public SourceCodeBuilder AppendJoin<T>(char separator, IEnumerable<T> values)
        {
            _sb.AppendJoin(separator, values); return this;
        }

        public SourceCodeBuilder AppendJoin<T>(string separator, IEnumerable<T> values)
        {
            _sb.AppendJoin(separator, values); return this;
        }

        public SourceCodeBuilder AppendLine()
        {
            _sb.AppendLine(); return this;
        }

        public SourceCodeBuilder AppendLine(string value)
        {
            _sb.Append($"{_linePrefix}{value}");
            _sb.AppendLine();
            return this;
        }

        public SourceCodeBuilder Clear()
        {
            _sb.Clear();
            return this;
        }

        public void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count)
        {
            _sb.CopyTo(sourceIndex, destination, destinationIndex, count);
        }

        public void CopyTo(int sourceIndex, Span<char> destination, int count)
        {
            _sb.CopyTo(sourceIndex, destination, count);
        }

        public int EnsureCapacity(int capacity)
        {
            return _sb.EnsureCapacity(capacity);
        }

        public bool Equals(ReadOnlySpan<char> span)
        {
            return _sb.Equals(span);
        }

        public bool Equals(SourceCodeBuilder sb)
        {
            return _sb.Equals(sb);
        }

        public SourceCodeBuilder Insert(int index, bool value)
        {
            _sb.Insert(index, value); return this;
        }

        public SourceCodeBuilder Insert(int index, byte value)
        {
            _sb.Insert(index, value); return this;
        }

        public SourceCodeBuilder Insert(int index, char value)
        {
            _sb.Insert(index, value); return this;
        }

        public SourceCodeBuilder Insert(int index, char[] value)
        {
            _sb.Insert(index, value); return this;
        }

        public SourceCodeBuilder Insert(int index, char[] value, int startIndex, int charCount)
        {
            _sb.Insert(index, value, startIndex, charCount); return this;
        }

        public SourceCodeBuilder Insert(int index, decimal value)
        {
            _sb.Insert(index, value); return this;
        }

        public SourceCodeBuilder Insert(int index, double value)
        {
            _sb.Insert(index, value); return this;
        }

        public SourceCodeBuilder Insert(int index, short value)
        {
            _sb.Insert(index, value); return this;
        }

        public SourceCodeBuilder Insert(int index, int value)
        {
            _sb.Insert(index, value); return this;
        }

        public SourceCodeBuilder Insert(int index, long value)
        {
            _sb.Insert(index, value); return this;
        }

        public SourceCodeBuilder Insert(int index, object value)
        {
            _sb.Insert(index, value); return this;
        }

        public SourceCodeBuilder Insert(int index, ReadOnlySpan<char> value)
        {
            _sb.Insert(index, value); return this;
        }

        public SourceCodeBuilder Insert(int index, sbyte value)
        {
            _sb.Insert(index, value); return this;
        }

        public SourceCodeBuilder Insert(int index, float value)
        {
            _sb.Insert(index, value); return this;
        }

        public SourceCodeBuilder Insert(int index, string value)
        {
            _sb.Insert(index, value); return this;
        }

        public SourceCodeBuilder Insert(int index, string value, int count)
        {
            _sb.Insert(index, value, count); return this;
        }

        public SourceCodeBuilder Insert(int index, ushort value)
        {
            _sb.Insert(index, value); return this;
        }

        public SourceCodeBuilder Insert(int index, uint value)
        {
            _sb.Insert(index, value); return this;
        }

        public SourceCodeBuilder Insert(int index, ulong value)
        {
            _sb.Insert(index, value); return this;
        }

        public SourceCodeBuilder Remove(int startIndex, int length)
        {
            _sb.Remove(startIndex, length); return this;
        }

        public SourceCodeBuilder Replace(char oldChar, char newChar)
        {
            _sb.Replace(oldChar, newChar); return this;
        }

        public SourceCodeBuilder Replace(char oldChar, char newChar, int startIndex, int count)
        {
            _sb.Replace(oldChar, newChar, startIndex, count); return this;
        }

        public SourceCodeBuilder Replace(string oldValue, string newValue)
        {
            _sb.Replace(oldValue, newValue); return this;
        }

        public SourceCodeBuilder Replace(string oldValue, string newValue, int startIndex, int count)
        {
            _sb.Replace(oldValue, newValue, startIndex, count); return this;
        }

        public override string ToString()
        {
            return _sb.ToString();
        }

        public string ToString(int startIndex, int length)
        {
            return _sb.ToString(startIndex, length);
        }

        public int Capacity
        {
            get => _sb.Capacity;
            set => _sb.Capacity = value;
        }

        public char this[int index]
        {
            get => _sb[index];
            set => _sb[index] = value;
        }

        public int Length
        {
            get => _sb.Length;
            set => _sb.Length = value;
        }

        public int MaxCapacity => _sb.MaxCapacity;

        public SourceCodeBuilder AppendLines(IEnumerable<string> lines)
        {
            foreach (var i in lines)
                AppendLine(i);
            return this;
        }
    }
}