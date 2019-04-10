using System;
using System.Collections.Generic;

namespace Monkey.WebApi.AcceptanceTests.Integration
{
    public class MockRegister 
    {
        private Dictionary<Type, object> _mocks;
        


        public MockRegister()
        {
            _mocks = new Dictionary<Type, object>();
        }

        public T GetMock<T>()
        {
            return (T) GetMock(typeof(T));
        }
        public object GetMock(Type key)
        {
            if (!_mocks.TryGetValue(key, out object mock))
            {
                mock = NSubstitute.Substitute.For(new Type[] {key}, null);
                _mocks.Add(key, mock);
            }

            return mock;
        }
    }
}