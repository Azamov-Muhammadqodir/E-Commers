using E_Commers.Domain.Commons;

namespace E_Commers.Domain.Entity
{
    public class UserRole:BaseAuditableEntity
    {
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}