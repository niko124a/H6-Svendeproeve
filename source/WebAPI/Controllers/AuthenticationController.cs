using Common.Helpers;
using DatabaseAccess.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Models.Authentication;

namespace WebAPI.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHelper _passwordHelper;
        private readonly IConfiguration _configuration;
        public AuthenticationController(IUserRepository userRepository, PasswordHelper passwordHelper, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _passwordHelper = passwordHelper;
            _configuration = configuration;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]AuthenticationRequestBody authenticationRequestBody)
        {
            // validate usernam and password
            var user = await _userRepository.GetUserByUsernameAsync(authenticationRequestBody.Username);
            if (user == null)
                return Unauthorized();

            if (!_passwordHelper.VerifyPassword(authenticationRequestBody.Password, user.PasswordHash, user.Salt))
                return Unauthorized();

            // create token
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"]));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", user.Id.ToString()));
            claimsForToken.Add(new Claim(ClaimTypes.Name, user.Username));
            claimsForToken.Add(new Claim(ClaimTypes.Role, user.Role.Name));

            JwtSecurityToken jwtSecurityToken;

            if (user.Username == "Website")
            {
                jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow, // start of token life
                DateTime.UtcNow.AddYears(1), // end of token life
                signingCredentials);
            }
            else
            {
                jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow, // start of token life
                DateTime.UtcNow.AddHours(1), // end of token life
                signingCredentials);
            }
            

            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return Ok(token);
        }
    }
}
