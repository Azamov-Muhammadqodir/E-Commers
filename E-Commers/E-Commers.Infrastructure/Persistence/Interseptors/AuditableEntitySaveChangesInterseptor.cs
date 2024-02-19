using E_Commers.Application.Interfaces;
using E_Commers.Domain.Commons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;

namespace E_Commers.Infrastructure.Persistence.Interseptors
{
    public class AuditableEntitySaveChangesInterseptor : SaveChangesInterceptor
    {
        private readonly ICurrentUser _currentUser;
        public AuditableEntitySaveChangesInterseptor(ICurrentUser currentUser)
        {
             _currentUser = currentUser;
        }
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }
        public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);
            return base.SavedChangesAsync(eventData, result, cancellationToken);
        }

        public void UpdateEntities(DbContext? dbContext)
        {
            if (dbContext == null) return;
            foreach(var item in dbContext.ChangeTracker.Entries<BaseAuditableEntity>())
            {
                if(item.State == EntityState.Added)
                {
                    item.Entity.CreatedBy = _currentUser.Name??"Admin";
                    item.Entity.CreatedDate = DateTime.Now;
                }
                else if(item.State == EntityState.Modified)
                {
                    item.Entity.ModifyBy = _currentUser.Name??"Admin";
                    item.Entity.ModifyDate = DateTime.Now;
                }
            }
        }
    }
    
}
