using E_Commers.Domain.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_Commers.Domain.Entity
{
    public class Role:BaseAuditableEntity
    {
        public string Name  { get; set; }
        public Guid[] PremissionIDs { get; set; }
        [JsonIgnore]
        public ICollection<UserRole>? UserRoles { get; set; }
        [JsonIgnore]
        public ICollection<RolePermission>? RolePermissions { get; set; }
    }
}
