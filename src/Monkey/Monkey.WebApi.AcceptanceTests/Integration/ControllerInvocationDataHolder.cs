using System;
using System.Collections.Generic;

namespace Monkey.WebApi.AcceptanceTests.Integration
{
    public class ControllerInvocationDataHolder
    {
        private object _actualResult;
        private Guid _id;
        public Dictionary<string, Type> Types { get; private set; }
        public Type CommandHandlerInterface { get; set; }

        public object ActualResult
        {
            get { return _actualResult; }
            set { _actualResult = value; }
        }

        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public ControllerInvocationDataHolder()
        {
            Types = new Dictionary<string, Type>();
        }
    }
}