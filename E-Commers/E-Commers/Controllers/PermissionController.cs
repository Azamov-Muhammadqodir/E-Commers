using E_Commers.Application.Abstraction;
using E_Commers.Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commers.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PermissionController : ControllerBase
{
    private readonly IApplicationDbContext _permissionService;
    public PermissionController(IApplicationDbContext permissionService)
    {
        _permissionService = permissionService;
    }

    [HttpPost("permission/add")]
    [AllowAnonymous]
    public async Task<IActionResult> Create([FromBody] string[] permissions)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        var _permissions = new List<Permission>();
        foreach (string item in permissions)
        {
            _permissions.Add(new()
            {
                Name = item
            });
        }
        bool result = _permissionService.Permissions.AddRangeAsync(_permissions).IsCompletedSuccessfully;
        if (result)
        {
            await _permissionService.SaveChangesAsync();
            return Ok(new Response<Permission>());
        }
        return BadRequest();
    }

    [HttpGet("permission/getbyid")]
    [AllowAnonymous]

    public async Task<IActionResult> GetById(Guid id)
    {
        var permission = _permissionService.Permissions.FirstOrDefault(x => x.Id == id);
        if (permission != null)
        {
            return Ok(new Response<Permission>()
            {
                Result = permission
            });
        }

        return NotFound(new Response<Permission>()
        {
            StatusCode = 404,
            Message = "Permission Not Found",
            IsSuccess = true,
            Result = null
        });
    }
    [HttpGet("permission/getall")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        IQueryable<Permission> permissions = _permissionService.Permissions;
        if (permissions.Count() > 0)
        {
            return Ok(new Response<Permission>()
            {
                Result = permissions
            });
        }

        return NotFound(new Response<Permission>()
        {
            StatusCode = 404,
            Message = "Permission Not Found",
            IsSuccess = true,
            Result = null
        });
    }

}

