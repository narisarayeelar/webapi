using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.data;
using webapi.dto;
using webapi.model;
using webapi.Repository;

namespace webapi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _repo;
        public UserController(UserRepository repo)
        {
            _repo = repo;
        }

        [Authorize] 
        [HttpGet]
        public async Task<List<User>> GetAll()
        {
            var users = await _repo.GetAll(); //await _dbContext.User.ToListAsync();
            //var users = await _dbContext.User.ToListAsync();
            return users;
        }

        // [HttpPost]
        // public async Task<IActionResult> AddUser(User user)
        // {
        //     var encryptedPassword = EncryptedPassword(user.HashPassword);
        //     user.HashPassword = encryptedPassword.Item2;
        //     user.SaltKey = encryptedPassword.Item1;
        //     user.CreatedDate = DateTime.Now;
        //     user.CreatedBy = 0;

        //     await _dbContext.User.AddAsync(user);
        //     await _dbContext.SaveChangesAsync();
        //     return Ok();
        // }

        // private Tuple<string, string> EncryptedPassword(string plainPassword)
        // {
        //     string password = plainPassword;

        //     // generate a 128-bit salt using a secure PRNG
        //     byte[] salt = new byte[128 / 8];
        //     using (var rng = RandomNumberGenerator.Create())
        //     {
        //         rng.GetBytes(salt);
        //     }
        //     string saltStr = Convert.ToBase64String(salt);

        //     // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
        //     string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
        //         password: password,
        //         salt: salt,
        //         prf: KeyDerivationPrf.HMACSHA1,
        //         iterationCount: 10000,
        //         numBytesRequested: 256 / 8));

        //     return Tuple.Create(saltStr, hashed);
        // }
    }
}