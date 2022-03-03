using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Wrappers
{
    public class Response<T>
    {
        public Response()
        {
        }

        public Response(string message = null)
        {
            Message = message;
        }

        public string Message { get; set; }
        public List<string> Errors { get; set; }
    }
}
