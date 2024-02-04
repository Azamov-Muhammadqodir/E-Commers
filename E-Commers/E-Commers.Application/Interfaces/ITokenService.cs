using E_Commers.Domain.Entity.Token;
using E_Commers.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Commers.Application.Interfaces
{
    public interface ITokenService
    {
        public Task<Tokens> CreateTokensAsync(User user);
        public Task<Tokens> CreateTokensFromRefresh(ClaimsPrincipal principal, RefreshToken savedRefreshToken);
        public ClaimsPrincipal GetClaimsFromExpiredToken(string token);

        public Task<bool> AddRefreshToken(RefreshToken tokens);
        public bool Update(RefreshToken tokens);
        public IQueryable<RefreshToken> Get(Expression<Func<RefreshToken, bool>> predicate);
        public bool Delete(RefreshToken token);
    }
}
