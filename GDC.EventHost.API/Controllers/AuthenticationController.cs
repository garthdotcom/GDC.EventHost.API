using GDC.EventHost.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GDC.EventHost.API.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthenticationController(
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration ??
                throw new ArgumentNullException(nameof(configuration));
            _userManager = userManager ??
                throw new ArgumentNullException(nameof(userManager));
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterRequestBody request)
        {
            var user = new ApplicationUser
            {
                UserName = request.Username,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                City = request.City ?? string.Empty
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            if (request.IsAdmin)
            {
                await _userManager.AddToRoleAsync(user, "admin");
            }

            return Ok();
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticationRequestBody request)
        {
            var user = await _userManager.FindByNameAsync(request.Username ?? string.Empty);

            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password ?? string.Empty))
            {
                return Unauthorized();
            }

            var keyFromConfig = _configuration["Authentication:SecretForKey"] ??
                throw new InvalidOperationException("Authentication:SecretForKey is not configured.");

            var securityKey = new SymmetricSecurityKey(Convert.FromBase64String(keyFromConfig));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                new Claim("city", user.City)
            };

            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var token = new JwtSecurityToken(
                issuer: _configuration["Authentication:Issuer"],
                audience: _configuration["Authentication:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: signingCredentials);

            return Ok(new { AccessToken = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        public class RegisterRequestBody
        {
            public string? Username { get; set; }
            public string? Email { get; set; }
            public string? Password { get; set; }
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string? City { get; set; }
            public bool IsAdmin { get; set; }
        }

        public class AuthenticationRequestBody
        {
            public string? Username { get; set; }
            public string? Password { get; set; }
        }
    }
}
