using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Wrappers
{
    //[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<string>? Errors { get; set; }
    }
}
