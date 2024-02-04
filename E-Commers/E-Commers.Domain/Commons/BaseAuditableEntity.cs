using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commers.Domain.Commons
{
    public class BaseAuditableEntity:BaseEntity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifyBy { get; set; }
    }
}
