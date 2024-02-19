using E_Commers.Application.Interfaces;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Security.Claims;

namespace E_Commers.Common.Services;

public class CurrentUserService : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;

    }
    public string Name => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
}
