using System;
using Monkey.Cqrs;

namespace Monkey.WebApi.AcceptanceTests.Integration
{
    public static class MockRegisterExtensions
    {
        public static ICommandHandler<TCommand, TResult> GetHandler<TCommand, TResult>(this MockRegister register)
        {
            return register.GetMock< ICommandHandler <TCommand,TResult> >();   
        }
        public static object GetHandler(this MockRegister register, Type commandType, Type resultType)
        {
            return register.GetMock(typeof(ICommandHandler<,>).MakeGenericType(commandType, resultType));
        }

        
    }
}