using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Wrappers
{
    /// <summary>
	/// Base Command for MediatR
	/// </summary>
	/// <typeparam name="T">Paylod for the Command</typeparam>
	/// <typeparam name="TResponse">Command Response</typeparam>
    public class BaseCommand<T, TResponse> : IRequest<TResponse> where T : class, new()
    {
        public T CommandPayload { get; set; }

        public BaseCommand(T commandPayload)
        {
            CommandPayload = commandPayload;
        }
    }

    //public class BaseCommand<T, TResponse> : IRequest<TResponse> where T : class, new()
    //{
    //    public T CommandPayload { get; set; }

    //    public BaseCommand(T commandPayload)
    //    {
    //        CommandPayload = commandPayload;
    //    }
    //}
}
