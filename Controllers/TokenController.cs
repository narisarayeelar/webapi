using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using webapi.data;
using webapi.dto;
using webapi.model;

namespace webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _dbContext;

        public TokenController(IConfiguration configuration, ApplicationDbContext dbContext)
        {
            _config = configuration;
            _dbContext = dbContext;
        }

        [HttpPost]
        public IActionResult CreateToken(LoginCredential login)
        {
            IActionResult response = Unauthorized();

            if (login != null)
            {
                var user = AuthenticateUser(login);
                if (user != null)
                {
                    string token = BuildToken(user);
                    response = Ok(new { Token = token });
                }
            }

            return response;
        }

        private string BuildToken(User user)
        {
            var tokenKey = _config["Jwt:Key"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>{
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.NameIdentifier,user.FirstName),
                new Claim(ClaimTypes.Version,"1.0")

            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Issuer"], claims, DateTime.UtcNow, DateTime.UtcNow.AddDays(1), cred);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private User AuthenticateUser(LoginCredential login)
        {
            var user = _dbContext.User.Where(x => x.UserName == login.UserName).FirstOrDefault();
            var hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: login.Password,
                salt: System.Convert.FromBase64String(user.SaltKey),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            if (hashedPassword.Equals(user.HashPassword))
            {
                return user;
            }

            return null;
        }
    }
}