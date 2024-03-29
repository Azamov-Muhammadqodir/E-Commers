﻿using E_Commers.Application.Abstraction;
using E_Commers.Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace E_Commers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IApplicationDbContext _roleService;

        public RoleController(IApplicationDbContext roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] Role role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            role.RolePermissions = new List<RolePermission>();
            foreach (Guid permission in role.PremissionIDs)
            {
                Permission? _permission = _roleService.Permissions.Find(permission);
                if (_permission != null)
                {

                    role.RolePermissions.Add(new RolePermission()
                    {
                        Id = Guid.NewGuid(),
                        Permission = _permission,
                        Role = role,
                    });
                }
                else return BadRequest(permission + " is not found in DB!");
            }
            if (await _roleService.Roles.AddAsync(role) != null)
            {
                await _roleService.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();

        }
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            IQueryable<GetRoleModel> r = _roleService.Roles.Include(x => x.RolePermissions).ThenInclude(x => x.Permission)
                    .Select(x => new GetRoleModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Permissions = x.RolePermissions.Select(t => t.Permission).ToArray()
                    });


            return Ok(r);
        }

        private class GetRoleModel
        {
            [JsonPropertyName("role_id")]
            public Guid Id { get; set; }

            public string Name { get; set; }

            public Permission[] Permissions { get; set; }
        }
    }
}
