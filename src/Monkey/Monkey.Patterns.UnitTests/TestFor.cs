using System;
using System.Linq;
using System.Reflection;
using NSubstitute;

namespace Monkey.Patterns.UnitTests
{
    public abstract class TestFor<T>
    {
        
        private T _sut;
        protected abstract T CreateSut();
        public T Sut
        {
            get
            {
                if (_sut == null)
                {
                    _sut = CreateSut();
                }

                return _sut;
            }
        }

        protected TestFor()
        {
            Init();
        }
        private void Init()
        {
            var fields = this.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetField |
                                                  BindingFlags.GetField);
            foreach (var f in fields.Where(x => x.FieldType.IsInterface))
            {
                var obj = f.GetValue(this);
                if (obj == null)
                    f.SetValue(this, Substitute.For(new Type[] {f.FieldType}, new object[0]));
            }
        }
    }
}