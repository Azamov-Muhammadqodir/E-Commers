using E_Commers.Domain.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_Commers.Domain.Entity
{
    public class Permission:BaseAuditableEntity
    {
        public string Name { get; set; }
        [JsonIgnore]
        public ICollection<RolePermission>? RolePermissions { get; set; }
    }
}
