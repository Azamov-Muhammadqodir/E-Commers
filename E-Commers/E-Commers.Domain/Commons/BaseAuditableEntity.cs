using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_Commers.Domain.Commons
{
    public class BaseAuditableEntity:BaseEntity
    {
        [JsonIgnore]
        public DateTime CreatedDate { get; set; }
        [JsonIgnore]
        public DateTime ModifyDate { get; set; }
        [JsonIgnore]
        public string? CreatedBy { get; set; }
        [JsonIgnore]
        public string? ModifyBy { get; set; }
    }
}
