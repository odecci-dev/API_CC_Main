using API_PCC.Data;
using API_PCC.Manager;
using API_PCC.Models;
using API_PCC.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace API_PCC.Controllers
{
    [Authorize("ApiKey")]
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        MailSender _mailSender;
        private readonly PCC_DEVContext _context;

        public UserController(PCC_DEVContext context, IConfiguration configuration)
       {
            _context = context;
            _mailSender = new MailSender(configuration);
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
                return Problem("Entity set 'PCC_DEVContext.TblUsersModels'  is null!");
            }

            var userCredentials = _context.TblUsersModels.Where(user => !user.DeleteFlag && user.Email == loginModel.email && user.Password == Cryptography.Encrypt(loginModel.password)).FirstOrDefault();

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
                return Problem("Entity set 'PCC_DEVContext.TblUsersModels' is null!");
            }

            var userInfo = _context.TblUsersModels.Where(user => !user.DeleteFlag && user.Email == email).FirstOrDefault();

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
                return Problem("Entity set 'PCC_DEVContext.TblUsersModels' is null!");
            }

            var userList = _context.TblUsersModels.Where(users => !users.DeleteFlag).ToList();

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
            var isEmailExists = _context.TblUsersModels.Any(user => !user.DeleteFlag && user.Email == userTbl.Email);

            if (isEmailExists)
            {
                return Conflict("Email already exists!");
            }

            try
            {
                userTbl.Password = Cryptography.Encrypt(userTbl.Password);

                _context.TblUsersModels.Add(userTbl);
                await _context.SaveChangesAsync();

                return CreatedAtAction("register", new { id = userTbl.Id }, userTbl);
            }
            catch (Exception ex)
            {
                String exception = ex.GetBaseException().ToString();
                return BadRequest(exception);
            }
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
                return Problem("Entity set 'PCC_DEVContext.TblUsersModels'  is null!");
            }

            var isEmailExists = _context.TblUsersModels.Any(user => !user.DeleteFlag && user.Email == email); 

            if (!isEmailExists)
            {
                return BadRequest("User does not exists!!"); 
            }

            try
            {
                _mailSender.sendForgotPasswordMail(email);
                return Ok("Password Reset Email sent successfully!");
            }
            catch (Exception ex)
            {
                String exception = ex.GetBaseException().ToString();
                return BadRequest(exception);
            }
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> resendForgotPassword(String email)
        {
            if (_context.TblUsersModels == null)
            {
                return Problem("Entity set 'PCC_DEVContext.TblUsersModels' is null!");
            }

            var isEmailExists = _context.TblUsersModels.Any(user => !user.DeleteFlag && user.Email == email);

            if (!isEmailExists)
            {
                return BadRequest("User does not exists!!");
            }

            try
            {
                _mailSender.sendForgotPasswordMail(email);
                return Ok("Password Reset Email resent successfully!");
            }
            catch (Exception ex)
            {
                String exception = ex.GetBaseException().ToString();
                return BadRequest(exception);
            }
        }

        private bool UserTblExists(int id)
        {
            return (_context.TblUsersModels?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
