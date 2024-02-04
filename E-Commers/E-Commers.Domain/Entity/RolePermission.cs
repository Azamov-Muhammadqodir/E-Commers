using E_Commers.Domain.Commons;

namespace E_Commers.Domain.Entity
{
    public class RolePermission:BaseAuditableEntity
    {
        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; }
        public Guid PermissionId { get; set; }
        public virtual Permission Permission { get; set; }
    }
}