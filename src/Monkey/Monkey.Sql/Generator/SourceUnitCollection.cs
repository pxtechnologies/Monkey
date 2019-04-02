    using System;
using System.Collections;
using System.Collections.Generic;
using Monkey.Generator;

namespace Monkey.Sql.Generator
{
    public class SourceUnitCollection : IEnumerable<SourceUnit>
    {
        Dictionary<Guid, SourceUnit> _srcUnits = new Dictionary<Guid, SourceUnit>();
        Dictionary<string, SourceUnit> _nameUnits = new Dictionary<string, SourceUnit>();

        public bool Append(SourceUnit unit)
        {
            if (_srcUnits.TryGetValue(unit.CodeHash, out SourceUnit u))
            {
                if (u.FullName == unit.FullName)
                    return false;
                else throw new NotSupportedException("MD5 generation conflict.");
            }
            else
            {
                if (_nameUnits.TryGetValue(unit.FullName, out SourceUnit u2))
                    throw new FullTypeNameConflictException(u2.FullName);
                else
                {
                    _srcUnits.Add(unit.CodeHash, unit);
                    _nameUnits.Add(unit.FullName, unit);
                    return true;
                }
            }
        }

        public IEnumerator<SourceUnit> GetEnumerator()
        {
            foreach (var sourceUnit in _srcUnits)
            {
                yield return sourceUnit.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}