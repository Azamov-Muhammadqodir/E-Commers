using E_Commers.Application.Abstraction;
using E_Commers.Domain.Entity;
using E_Commers.Domain.Entity.Token;
using E_Commers.Infrastructure.Persistence.Interseptors;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace E_Commers.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly AuditableEntitySaveChangesInterseptor _auditableEntitySaveChangesInterceptor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, 
            AuditableEntitySaveChangesInterseptor auditableEntitySaveChangesInterseptor)
            :base(options)
        {
            _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterseptor;
        }
        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles {get; set;}

        public DbSet<UserRole> UserRoles {get; set;}

        public DbSet<Permission> Permissions {get; set;}

        public DbSet<RolePermission> RolePermissions {get; set;}

        public DbSet<RefreshToken> RefreshToken {get; set;}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
        }
    }
}
