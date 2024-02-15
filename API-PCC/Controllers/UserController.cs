using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_PCC.Data;
using API_PCC.Models;
using Microsoft.AspNetCore.Authorization;
using API_PCC.Manager;
using PeterO.Numbers;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace API_PCC.Controllers
{
    [Authorize("ApiKey")]
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly PCC_DEVContext _context;

        public UserController(PCC_DEVContext context)
        {
            _context = context;
        }

        public class LoginModel
        {
            public string? email { get; set; }
            public string? password { get; set; }
        }

        // POST: user/login
        [HttpPost]
        public async Task<ActionResult<IEnumerable<UserTbl>>> login(LoginModel loginModel)
        {
            if (_context.UserTbls == null)
            {
                return NotFound();
            }

            var userCredentials = _context.UserTbls.Where(user => user.Email == loginModel.email && user.Password == Cryptography.Encrypt(loginModel.password)).First();

            if (userCredentials == null)
            {
                return Unauthorized("User Credentials Invalid !!");
            }

            if (userCredentials.Status == 0)
            {
                return Unauthorized("Account under approval !!");
            }

            return Ok("Login Successful !!");
        }

        // POST: user/register
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserTbl>> register(UserTbl userTbl)
        {
            if (_context.UserTbls == null)
            {
                return Problem("Entity set 'PCC_DEVContext.UserTbls'  is null.");
            }

            var isEmailExists = _context.UserTbls.Any(user => user.Email == userTbl.Email);

            if (isEmailExists)
            {
                return Conflict("Email already exists!");
            }

            userTbl.Password = Cryptography.Encrypt(userTbl.Password);

            _context.UserTbls.Add(userTbl);
            await _context.SaveChangesAsync();

            return CreatedAtAction("register", new { id = userTbl.Id }, userTbl);
        }

        // GET: user/rememberPassword
        [HttpGet("{email}")]
        public async Task<IActionResult> rememberPassword(String email)
        {
            return NoContent();
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> forgotPassword(String email)
        {
            if (_context.UserTbls == null)
            {
                return Problem("Entity set 'PCC_DEVContext.UserTbls'  is null.");
            }

            var isEmailExists = _context.UserTbls.Any(user => user.Email == email); 

            if (!isEmailExists)
            {
                return BadRequest("User does not exists!!"); 
            }

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> sendResetPasswordMail()
        {
            return NoContent();
        }

        private bool UserTblExists(int id)
        {
            return (_context.UserTbls?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
