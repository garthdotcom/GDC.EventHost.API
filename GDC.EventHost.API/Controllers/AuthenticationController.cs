using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GDC.EventHost.API.Controllers
{
    /// <summary>
    /// Defunct: Authentication is now handled by an external identity provider
    /// </summary>
    [ApiController]
    [Route("api/authentication")]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration ??
                throw new ArgumentNullException(nameof(configuration));
        }

        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate(
            AuthenticationRequestBody authenticationRequestBody)
        {
            // Step 1: validate the username and password

            var user = ValidateUserCredentials(
                authenticationRequestBody.Username, authenticationRequestBody.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            // Step 2: create a token

            var keyFromConfig = _configuration["Authentication:SecretForKey"] ??
                throw new ArgumentNullException(_configuration["Authentication:SecretForKey"]);

            var securityKey = new SymmetricSecurityKey(
                Convert.FromBase64String(keyFromConfig));

            var signingCredentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256 );

            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", user.UserId.ToString()));
            claimsForToken.Add(new Claim("given_name", user.FirstName.ToString()));
            claimsForToken.Add(new Claim("family_name", user.LastName.ToString()));
            claimsForToken.Add(new Claim("city", user.City.ToString()));
            claimsForToken.Add(new Claim("admin", user.Administrator.ToString()));

            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredentials);

            var response = new
            {
                AccessToken = new JwtSecurityTokenHandler()
                    .WriteToken(jwtSecurityToken)
            };

            return Ok(response);
        }

        private EventHostUser? ValidateUserCredentials(string? username, string? password)
        {
            //TODO: query the database and build a User if authorized.

            var defaultUsername = _configuration["DefaultUsername"];
            var defaultPassword = _configuration["DefaultPassword"];

            if (username == null || password == null || username != defaultUsername || password != defaultPassword)
            {
                return null;
            }

            return new EventHostUser(Guid.NewGuid(), username ?? "", "Gareth", "Hewson", "London", true);
        }

        public class AuthenticationRequestBody
        {
            public string? Username { get; set; }
            public string? Password { get; set; }
            public bool? Administrator { get; set; }
        }

        private class EventHostUser
        {
            public Guid UserId { get; set; }
            public string Username { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string City { get; set; }
            public bool Administrator { get; set; }

            public EventHostUser(Guid userId, string userName, string firstName, string lastName, string city, bool administrator)
            {
                UserId = userId;
                Username = userName;
                FirstName = firstName;
                LastName = lastName;
                City = city;
                Administrator = administrator;
            }
        }
    }
}
