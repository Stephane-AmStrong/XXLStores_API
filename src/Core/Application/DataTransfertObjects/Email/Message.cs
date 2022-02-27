using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace Application.DataTransfertObjects.Email
{
    public record Message
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string From { get; set; }
        public IFormFileCollection Attachments { get; set; }
    }
}
