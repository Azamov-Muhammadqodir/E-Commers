using E_Commers.Application.Abstraction;
using E_Commers.Application.Extensions;
using E_Commers.Application.Interfaces;
using E_Commers.Domain.Entity;
using E_Commers.Domain.Entity.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

namespace E_Commers.Application.Services;
public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly IApplicationDbContext _dbcontext;
    private readonly int _refreshTokenLifeTime;
    private readonly int _accessTokenLifeTime;

    public TokenService(IConfiguration configuration, IApplicationDbContext dbcontext)
    {
        _configuration = configuration;
        _dbcontext = dbcontext;
        _refreshTokenLifeTime = int.Parse(configuration["JWT:RefreshTokenLifetime"]);
        _accessTokenLifeTime = int.Parse(configuration["JWT:AccessTokenLifetime"]);
    }

    public async Task<bool> AddRefreshToken(RefreshToken tokens)
    {
        await _dbcontext.RefreshToken.AddAsync(tokens);
        await _dbcontext.SaveChangesAsync();
        return true;
    }

    public string GenerateToken()
    {
        return (DateTime.Now.ToString() + _configuration["JWT:Key"]).ComputeHash();
    }
    public Tokens CreateToken(IEnumerable<Claim> claims)
    {
        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            expires: DateTime.UtcNow.AddMinutes(_accessTokenLifeTime),
            claims: claims,
            signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["JWT:Key"]

                    )), SecurityAlgorithms.HmacSha256Signature)
            );
        Tokens tokens = new()
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = GenerateToken()
        };

        return tokens;
    }

    public async Task<Tokens> CreateTokensAsync(User user)
    {
        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, user.Name),
        };

        foreach (var role in user.UserRoles)
        {
            foreach (var permission in role.Role.RolePermissions)
            {
                claims.Add(new Claim(ClaimTypes.Name, permission.Permission?.Name));
            }
        }

        claims = claims.Distinct().ToList();    
        Tokens tokens = CreateToken(claims);
        var SavedRefreshToken = Get(x=>x.Username ==  user.Username).FirstOrDefault();

        if( SavedRefreshToken == null )
        {
            var refreshToken = new RefreshToken()
            {
                ExpireDate = DateTime.UtcNow.AddMinutes(_refreshTokenLifeTime),
                Username = user.Username,
                RefreshTokenValue = tokens.RefreshToken,
            };
            await AddRefreshToken(refreshToken);
        }
        else
        {
            SavedRefreshToken.RefreshTokenValue = tokens.RefreshToken;
            SavedRefreshToken.ExpireDate = DateTime.UtcNow.AddMinutes(_refreshTokenLifeTime);
            Update(SavedRefreshToken);
        }

        return tokens;

    }

    public Task<Tokens> CreateTokensFromRefresh(ClaimsPrincipal principal, RefreshToken savedRefreshToken)
    {
        Tokens tokens = CreateToken(principal.Claims);
        savedRefreshToken.RefreshTokenValue = tokens.RefreshToken;
        savedRefreshToken.ExpireDate = DateTime.UtcNow.AddMinutes(_refreshTokenLifeTime);
        Update(savedRefreshToken);
        return Task.FromResult(tokens);
    }

    public bool Delete(RefreshToken token)
    {
        _dbcontext.RefreshToken.Remove(token);
        _dbcontext.SaveChangesAsync();
        return true;
    }

    public IQueryable<RefreshToken> Get(Expression<Func<RefreshToken, bool>> predicate)
    {
        return _dbcontext.RefreshToken.Where(predicate);
    }

    public ClaimsPrincipal GetClaimsFromExpiredToken(string token)
    {
        byte[] Key = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);

        var tokenParams = new TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = false,
            ValidAudience = _configuration["JWT:Audience"],
            ValidIssuer = _configuration["JWT:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Key),
            ClockSkew = TimeSpan.Zero
        };

        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        ClaimsPrincipal claimsPrincipal = handler.ValidateToken(token, tokenParams, out SecurityToken securityToken);
        JwtSecurityToken? jwtSecurityToken = securityToken as JwtSecurityToken;

        if (jwtSecurityToken == null)
        {
            throw new SecurityTokenException("Invalid token");
        }

        return claimsPrincipal;
    }

    public bool Update(RefreshToken tokens)
    {
        _dbcontext.RefreshToken.Update(tokens);
        _dbcontext.SaveChangesAsync();
        return true;
    }
}
