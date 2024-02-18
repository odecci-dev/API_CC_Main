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
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Data;
using MimeKit;
using MailKit.Net.Smtp;
using static API_PCC.Controllers.UserController;
using System.Web.Http.Results;
using API_PCC.Utils;

namespace API_PCC.Controllers
{
    [Authorize("ApiKey")]
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        MailSender _mailSender;
        private readonly PCC_DEVContext _context;
        private String status = "";

        private readonly IConfiguration _configuration;

        public UserController(PCC_DEVContext context, IConfiguration configuration)
       {
            _context = context;
            _mailSender = new MailSender(configuration);
        }

        public class Registerstats
        {
            public string Status { get; set; }

        }
        public class OTP
        {
            public string otp { get; set; }

        }

        public class JWTokenModel
        {
            public string? Email { get; set; }

        }

        public class StatusResult
        {
            public string Status { get; set; }

        }

        public class LoginModel
        {
            public string? email { get; set; }
            public string? password { get; set; }
        }

        // POST: user/login
        [HttpPost]
        public async Task<ActionResult<IEnumerable<TblUsersModel>>> login(LoginModel loginModel)
        {
            if (_context.TblUsersModels == null)
            {
                return NotFound();
            }

            var userCredentials = _context.TblUsersModels.Where(user => user.Email == loginModel.email && user.Password == Cryptography.Encrypt(loginModel.password)).First();

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

        //POST: user/info
        [HttpPost]
        public async Task<ActionResult<IEnumerable<TblUsersModel>>> info(String email)
        {
            if (_context.TblUsersModels == null)
            {
                return NotFound();
            }

            var userInfo = _context.TblUsersModels.Where(user => user.Email == email).First();

            if (userInfo == null)
            {
                return Conflict("User not Found !!");
            }

            return Ok(userInfo);
        }

        //POST: user/listAll
        [HttpPost]
        public async Task<ActionResult<IEnumerable<TblUsersModel>>> listAll()
        {
            if (_context.TblUsersModels == null)
            {
                return NotFound();
            }

            var userList = _context.TblUsersModels.ToList();

            if (userList == null)
            {
                return Conflict("No records!!");
            }

            return Ok(userList);
        }


        // POST: user/register
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TblUsersModel>> register(TblUsersModel userTbl)
        {
            if (_context.TblUsersModels == null)
            {
                return Problem("Entity set 'PCC_DEVContext.TblUsersModels'  is null.");
            }

            var isEmailExists = _context.TblUsersModels.Any(user => user.Email == userTbl.Email);

            if (isEmailExists)
            {
                return Conflict("Email already exists!");
            }

            userTbl.Password = Cryptography.Encrypt(userTbl.Password);

            _context.TblUsersModels.Add(userTbl);
            await _context.SaveChangesAsync();

            return CreatedAtAction("register", new { id = userTbl.Id }, userTbl);
        }

        [HttpPost]
        public async Task<IActionResult> SendOTP(TblRegistrationOtpmodel data)
        {
            var result = new OTP();
            try
            {
                var model = new TblRegistrationOtpmodel()
                {
                    Email = data.Email,
                    Otp = data.Otp,
                    Status = 10,

                };
                _context.TblRegistrationOtpmodels.Add(model);
                _context.SaveChanges();

                _mailSender.sendOtpMail(data);
                result.otp = "Success";
            }

            catch (Exception ex)
            {
                string status = ex.GetBaseException().ToString();
                return Problem(status);
            }
            return Ok(status);
        }

        [HttpPost]
        public async Task<IActionResult> VerifyOTP(TblRegistrationOtpmodel data)
        {
            var result = new Registerstats();
            try
            {
                string query = "";

                var registOtpModels = _context.TblRegistrationOtpmodels.Where(otpModel => otpModel.Otp == data.Otp && otpModel.Email == data.Email && (otpModel.Status == 9 || otpModel.Status == 10)); 

                if (registOtpModels != null)
                {
                    data.Status = 1;
                    _context.Entry(data).State = EntityState.Modified;

                    var userModel = _context.TblUsersModels.Where(user => user.Email == data.Email && user.Otp == data.Otp).First();
                    _context.Entry(userModel).State = EntityState.Modified;
                    userModel.Status = 1;

                    await _context.SaveChangesAsync();
                    result.Status = "OTP Matched!";
                    return Ok(result);
                } else
                {
                    var userModel = _context.TblUsersModels.Where(user => user.Email == data.Email && user.Otp == data.Otp).First();
                    _context.Entry(userModel).State = EntityState.Modified;
                    userModel.Status = 10;
                    result.Status = "OTP UnMatched!";
                    return BadRequest(result);
                }
            }

            catch (Exception ex)
            {
                result.Status = "OTP UnMatched!";
                return BadRequest(result);
            }
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> ResendOTP(TblRegistrationOtpmodel data)
        {
            var result = new OTP();
            try
            {
                var registrationOtpModel = _context.TblRegistrationOtpmodels.Where(otpModel => otpModel.Email == data.Email && otpModel.Status == 10).First();
                if (registrationOtpModel != null)
                {
                    registrationOtpModel.Otp = data.Otp;
                    _context.Entry(registrationOtpModel).State = EntityState.Modified;

                    await _context.SaveChangesAsync();

;                   _mailSender.sendOtpMail(data);
                    result.otp = "Success";
                } else
                {
                    result.otp = "Error";
                    return BadRequest(result);
                }
            }

            catch (Exception ex)
            {
                result.otp = "Error";
                return BadRequest(result);
            }
            return Ok(result);
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
            if (_context.TblUsersModels == null)
            {
                return Problem("Entity set 'PCC_DEVContext.TblUsersModels'  is null.");
            }

            var isEmailExists = _context.TblUsersModels.Any(user => user.Email == email); 

            if (!isEmailExists)
            {
                return BadRequest("User does not exists!!"); 
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> sendResetPasswordMail(JWTokenModel data)
        {
            string status = "";
            var result = new StatusResult();
            try
            {
                _mailSender.sendForgotPasswordMail(data);
                result.Status = "Success!";
                return Ok(result);
            }
            catch (Exception ex)
            {
                result.Status = "Error";
                return BadRequest(result);
            }
        }

        private bool UserTblExists(int id)
        {
            return (_context.TblUsersModels?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
