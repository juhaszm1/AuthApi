using AuthApi.Models;
using AuthApi.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Controllers
{
    [Route("roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly AuthuserContext _context;

        public RolesController(AuthuserContext context)
        {
            _context = context;
        }

        [HttpPost("newRole")]
        public async Task<ActionResult> NewRole([FromBody] NewRoleDto newRoleDto)
        {
            try
            {
                var role = new Role
                {
                    Id = Guid.NewGuid(),
                    Name = newRoleDto.Name,
                    RegTime = DateTime.Now

                };
                if (role is null)
                {
                    return StatusCode(404, new { message = "nincs szerepkor" });
                }
                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync();

                return StatusCode(200, new { message = "Sikeres szerepkör létrehozás" });

            }

            catch (Exception exception)
            {
                return StatusCode(400, new { message = exception.Message });
            }


        }


        [HttpGet("allRoles")]
        public async Task<ActionResult> AllRoles()
        {
            try
            {
                var roles = await _context.Roles.ToListAsync();

                if (roles is null)
                {
                    return StatusCode(400, new { message = "Hiba" });
                }

                return StatusCode(200, new { message = "Lekérdezve", result = roles });
            }
            catch (Exception exception)
            {
                return StatusCode(400, new { message = exception.Message });
            }

        }
        [HttpGet("byid")]
        public async Task<ActionResult> Rolesbyid()
        {
            try
            {
                var id = await _context.Roles.ToListAsync();

                if (id is null)
                {
                    return StatusCode(400, new { message = "Hiba" });
                }

                return StatusCode(200, new { message = "Lekérdezve", result = id });
            }
            catch (Exception exception)
            {
                return StatusCode(400, new { message = exception.Message });
            }
        }
    }
}