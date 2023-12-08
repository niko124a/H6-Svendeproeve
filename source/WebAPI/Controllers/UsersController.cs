using Common.Entities;
using Common.Helpers;
using DatabaseAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebAPI.Models.User;

namespace WebAPI.Controllers
{
    [Route("api/users")]
    [Authorize]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly PasswordHelper _passwordHelper;
        private readonly IConfiguration _configuration;

        public UsersController(IUserRepository userRepository, IRoleRepository roleRepository, PasswordHelper passwordHelper, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _passwordHelper = passwordHelper;
            _configuration = configuration;
        }

        [Authorize(Roles = "Admin,Bogholder")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var dbResult = await _userRepository.GetAllUsersAsync();
            if (dbResult.Count() == 0)
                return NoContent();

            return Ok(dbResult);
        }

        [Authorize(Roles = "Admin,Bogholder")]
        [HttpGet("role/{roleName}")]
        public async Task<IActionResult> GetUsersByRoleName(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return BadRequest("Invalid role name.");

            var dbResult = await _userRepository.GetUsersByRoleNameAsync(roleName);
            if (dbResult == null)
                return NoContent();

            return Ok(dbResult);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            if (id < 1)
                return BadRequest("Id have to be 1 or higher.");

            var dbResult = await _userRepository.GetUserByIdAsync(id);
            if (dbResult == null)
                return NotFound();

            return Ok(dbResult);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
        {
            
            if (await _userRepository.UserExistsAsync(userDto.Username))
            {
                return BadRequest($"A user with the username {userDto.Username} already exists.");
            }

            var role = await _roleRepository.GetRoleByNameAsync(Common.Enums.Entitity.UserRole.Kunde.ToString());
            if (role == null)
                return StatusCode(500, $"Unable to create user due to missing {Common.Enums.Entitity.UserRole.Kunde} role");

            string hash = _passwordHelper.HashPasword(userDto.Password, out var salt);

            var user = new User()
            {
                Username = userDto.Username,
                PasswordHash = hash,
                Salt = salt,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                PhoneNumber = userDto.PhoneNumber,
                Role = role
            };
            _userRepository.CreateUser(user);
            await _userRepository.SaveChangesAsync();

            return CreatedAtAction("CreateUser", user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("admin")]
        public async Task<IActionResult> CreateUser([FromBody] AdminCreateUserDto userDto)
        {

            if (await _userRepository.UserExistsAsync(userDto.Username))
            {
                return BadRequest($"A user with the username {userDto.Username} already exists.");
            }

            var role = await _roleRepository.GetRoleByNameAsync(userDto.RoleName);
            if (role == null)
                return StatusCode(500, $"Unable to create user due to missing {userDto.RoleName} role");

            string hash = _passwordHelper.HashPasword(userDto.Password, out var salt);

            var user = new User()
            {
                Username = userDto.Username,
                PasswordHash = hash,
                Salt = salt,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                PhoneNumber = userDto.PhoneNumber,
                Role = role
            };
            _userRepository.CreateUser(user);
            await _userRepository.SaveChangesAsync();

            return CreatedAtAction("CreateUser", user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto userDto)
        {
            if (id < 1)
                return BadRequest("Id have to be 1 or higher.");

            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Email = userDto.Email;
            user.PhoneNumber = userDto.PhoneNumber;

            await _userRepository.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (id < 1)
                return BadRequest("Id have to be 1 or higher.");

            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _userRepository.DeleteUser(user);
            await _userRepository.SaveChangesAsync();

            return Ok();
        }
    }
}
