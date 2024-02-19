using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_Commers.Domain.Commons
{
    public class BaseEntity
    {
        [JsonIgnore]
        public Guid Id { get; set; }
    }
}
