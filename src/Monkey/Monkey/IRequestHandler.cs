using System;
using System.Threading.Tasks;

namespace Monkey
{
    public interface IRequestHandler<in TRequest, TResponse>
    {
        Task<TResponse> Execute(TRequest request);
    }

    
}