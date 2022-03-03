using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Wrappers
{
    public class Response_Old<T>
    {
        public Response_Old()
        {
        }

        public Response_Old(T data, string message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }

        public Response_Old(string message)
        {
            Succeeded = false;
            Message = message;
        }

        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public T Data { get; set; }
    }
}
