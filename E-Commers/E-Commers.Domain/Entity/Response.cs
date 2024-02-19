using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_Commers.Domain.Entity
{
    public class Response<T>
    {
        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; } = 200;
        public string Message { get; set; } = "";
        public bool IsSuccess { get; set; } = true;
        public object Result { get; set; }

    }
}
