using E_Commers.Application.Abstraction;
using E_Commers.Application.Extensions;
using E_Commers.Application.Interfaces;
using E_Commers.Domain.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commers.Application.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly IApplicationDbContext _dbContext;

        public UserRepository(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> CreateAsync(User entity)
        {
            var roles = new List<UserRole>();

            foreach (var item in entity._roles)
                {
                roles.Add(new UserRole()
                {
                    Role = _dbContext.Roles.Find(item)

                });
            }

            entity.UserRoles = roles;
            entity.Password = entity.Password.ComputeHash();
            await _dbContext.Users.AddAsync(entity);
            int result = await _dbContext.SaveChangesAsync();
            return entity;
        }
        

        public Task<bool> DeleteAsync(Guid Id)
        {
            User? user = _dbContext.Users.FirstOrDefault(x => x.Id == Id);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<IQueryable<User>> GetAllAsync(Expression<Func<User, bool>>? expression = null)
        {
            return expression == null ? Task.FromResult(_dbContext.Users.AsQueryable()) :
                                    Task.FromResult(_dbContext.Users.Where(expression));
        }

        public Task<User> GetAsync(Expression<Func<User, bool>> expression)
        {
            User? user = _dbContext.Users.Where(expression)?
                                    .Include(x => x.UserRoles)
                                    .ThenInclude(x => x.Role)
                                    .ThenInclude(x => x.RolePermissions)
                                    .ThenInclude(x => x.Permission)
                                    .Select(x => x).FirstOrDefault();

            return Task.FromResult(user);
        }

        public async Task<bool> UpdateAsync(User entity)
        {
            _dbContext.Users.Update(entity);
            int result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }
    }
}
