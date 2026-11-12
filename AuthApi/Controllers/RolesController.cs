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
        public async Task<ActionResult> Rolesbyid(Guid id)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);

                if (user is null)
                {
                    return StatusCode(404, new { message = "Nincs találat" });
                }
                else if (user is not null)
                {
                    return StatusCode(200, new { message = "Lekérdezve", result = user });
                }

                return StatusCode(400, new { message = "Hiba" });
            }
            catch (Exception exception)
            {
                return StatusCode(400, new { message = exception.Message });
            }

        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateRole([FromQuery] Guid id, [FromBody] UpdateRoleDto updateRoleDto)
        {
            try
            {
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);

                if (role is null)
                {
                    return StatusCode(404, new { message = "Nincs találat" });
                }
                else
                {
                    role.Name = updateRoleDto.Name;

                    _context.Roles.Update(role);
                    await _context.SaveChangesAsync();

                    return StatusCode(200, new { message = "Frissítve", result = role });
                }
            }
            catch (Exception exception)
            {
                return StatusCode(400, new { message = exception.Message });
            }
        }

        [HttpDelete("deleteRole")]
        public async Task<IActionResult> DeleteRole([FromQuery] Guid id)
        {
            try
            {
                var role = await _context.Roles.FindAsync(id);

                if (role is null)
                {
                    return StatusCode(404, new { message = "Nincs találat" });
                }
                else
                {
                    _context.Roles.Remove(role);
                    await _context.SaveChangesAsync();

                    return StatusCode(200, new { message = "Törölve", result = role });
                }
            }
            catch (Exception exception)
            {
                return StatusCode(400, new { message = exception.Message });
            }
        }
    }
}