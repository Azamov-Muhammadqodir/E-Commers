using E_Commers.Domain.Entity;
using E_Commers.Domain.Entity.Token;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commers.Application.Abstraction
{
    public interface IApplicationDbContext
    {
        public DbSet<User> Users { get; }
        public DbSet<Role> Roles { get;}
        public DbSet<UserRole> UserRoles { get;}
        public DbSet<Permission> Permissions { get;}
        public DbSet<RolePermission> RolePermissions { get;}
        public DbSet<RefreshToken> RefreshToken { get;}
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
