using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Wrappers
{
    public class BaseQuery<T> : IRequest<T>
    {
        public dynamic RequestPayLod { get; set; }
        public BaseQuery(dynamic requestPayLod)
        {
            RequestPayLod = requestPayLod;
        }

        public BaseQuery()
        {

        }
    }

    // IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>

    /*
     
    public class BaseCommand<T, TResponse> : IRequest<TResponse> where T : class, new()
    {
        public T CommandPayload { get; set; }

        public BaseCommand(T commandPayload)
        {
            CommandPayload = commandPayload;
        }
    }
     
     */
}
