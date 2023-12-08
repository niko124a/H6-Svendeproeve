using Common.Entities;
using DatabaseAccess.Interfaces;
using DatabaseAccess.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.Role;

namespace WebAPI.Controllers
{
    [Route("api/roles")]
    [Authorize]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;

        public RolesController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var dbResult = await _roleRepository.GetAllRolesAsync();
            if (dbResult.Count() == 0)
                return NoContent();

            return Ok(dbResult);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            if (id < 1)
                return BadRequest("Id have to be 1 or higher.");

            var role = await _roleRepository.GetRoleByIdAsync(id);
            if (role == null)
                return NotFound();

            return Ok(role);
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetRoleByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Invalid name.");

            var role = await _roleRepository.GetRoleByNameAsync(name);
            if (role == null)
                return NotFound();

            return Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] RoleDto roleDto)
        {
            if (await _roleRepository.RoleExistsAsync(roleDto.Name))
                return BadRequest($"There is already a role with the name {roleDto.Name}.");

            Role role = new Role();
            role.Name = roleDto.Name;
            
            _roleRepository.CreateRole(role);
            await _roleRepository.SaveChangesAsync();

            return CreatedAtAction("CreateRole", role);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] RoleDto roleDto)
        {
            if (id < 1)
                return BadRequest("Id have to be 1 or higher.");

            Role? role = await _roleRepository.GetRoleByIdAsync(id);
            if (role == null)
                return NotFound();

            role.Name = roleDto.Name;

            await _roleRepository.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            if (id < 1)
                return BadRequest("Id have to be 1 or higher.");

            Role? role = await _roleRepository.GetRoleByIdAsync(id);
            if (role == null)
                return NotFound();

            _roleRepository.DeleteRole(role);
            await _roleRepository.SaveChangesAsync();

            return Ok();
        }
    }
}
