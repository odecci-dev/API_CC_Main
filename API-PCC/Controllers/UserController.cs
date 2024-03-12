using API_PCC.Data;
using API_PCC.Manager;
using API_PCC.Models;
using API_PCC.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Text;
using static API_PCC.Manager.DBMethods;

namespace API_PCC.Controllers
{
    [Authorize("ApiKey")]
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        string sql = "";
        string Stats = "";
        string Mess = "";
        string JWT = "";
        DbManager db = new DbManager();
        MailSender _mailSender;
        private readonly PCC_DEVContext _context;
        DBMethods dbmet = new DBMethods();
        private readonly EmailSettings _emailsettings;
        public UserController(PCC_DEVContext context, IOptions <EmailSettings> emailsettings)
        {
            _context = context;
            _emailsettings = emailsettings.Value;
            TblMailSenderCredential tblMailSenderCredential = _context.TblMailSenderCredentials.First();
            _emailsettings.username = tblMailSenderCredential.Email;
            _emailsettings.password = tblMailSenderCredential.Password;

        }
        public class EmailSettings
        {
            public Title Title { get; set; }
            public string Host { get; set; }
            public string username { get; set; }
            public string password { get; set; }
            public string sender { get; set; }

        }

        public class Title
        {
            public string OTP { get; set; }
            public string ForgotPassword { get; set; }
        }
        public class LoginModel
        {
            public string? email { get; set; }
            public string? password { get; set; }
        }
        public class loginCredentials
        {
            public string? username { get; set; }
            public string? password { get; set; }
            public string? ipaddress { get; set; }
            public string? location { get; set; }
            public string? rememberToken { get; set; }
        }
        public class StatusReturns
        {
            public string? Status { get; set; }
            public string? Message { get; set; }
            public string? JwtToken { get; set; }
        }

        public class ForgotPasswordModel
        {
            public string Email { get; set; }
            public string ForgotPasswordLink { get; set; }
        }
        public class ResetPasswordModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        [HttpGet]
        public async Task<IActionResult> UserForApprovalList()
        {
            var list = _context.TblUsersModels.Where(a => a.Status == 3).ToList();
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveRegistration(TblRegistrationOtpmodel data)
        {
            try
            {
                if (_context.TblRegistrationOtpmodels == null)
                {
                    return Problem("Entity set 'PCC_DEVContext.OTP' is null!");
                }
                var registOtpModels = _context.TblRegistrationOtpmodels.Where(otpModel => otpModel.Email == data.Email && otpModel.Status == 3).FirstOrDefault();

                if (registOtpModels != null)
                {
                    if (registOtpModels.Otp == data.Otp)
                    {
                        registOtpModels.Status = 5;
                        _context.Entry(registOtpModels).State = EntityState.Modified;

                        var userModel = _context.TblUsersModels.Where(user => user.Email == data.Email).FirstOrDefault();
                        _context.Entry(userModel).State = EntityState.Modified;
                        userModel.Status = 5;

                        await _context.SaveChangesAsync();

                        MailSender email = new MailSender(_emailsettings);
                        email.sendApprovalMail(data.Email);
                        return Ok("OTP verification successful!");
                    }
                    else
                    {
                        var userModel = _context.TblUsersModels.Where(user => user.Email == data.Email).FirstOrDefault();
                        _context.Entry(userModel).State = EntityState.Modified;
                        userModel.Status = 4;
                        await _context.SaveChangesAsync();

                        return Problem("Incorrect OTP. Please try again!");
                    }

                }
                else
                {
                    return BadRequest("Record not found on database!");
                }
            }

            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        // POST: user/login

        [HttpPost]
        public async Task<ActionResult<IEnumerable<TblUsersModel>>> login(loginCredentials data)
        {
            var loginstats = dbmet.GetUserLogIn(data.username, data.password, data.ipaddress, data.location);
            if (!data.rememberToken.IsNullOrEmpty())
            {
                var userModel = await _context.TblUsersModels.Where(userModel => userModel.Username == data.username).FirstAsync();

                userModel.RememberToken = data.rememberToken;
                _context.Entry(userModel).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            var item = new StatusReturns();
            item.Status = loginstats.Status;
            item.Message = loginstats.Message;
            item.JwtToken = loginstats.JwtToken;
            return Ok(item);
        }

        //POST: user/info
        [HttpPost]
        public async Task<ActionResult<IEnumerable<TblUsersModel>>> info(String email)
        {
            if (_context.TblUsersModels == null)
            {
                return Problem("Entity set 'PCC_DEVContext.TblUsersModels' is null!");
            }

            var userInfo = _context.TblUsersModels.Where(user => user.DeleteFlag != false && user.Email == email).FirstOrDefault();

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

            var userList = _context.TblUsersModels.Where(users => users.DeleteFlag != false).ToList();

            if (userList == null)
            {
                return Conflict("No records!!");
            }

            return Ok(userList);
        }

        [HttpPost]
        public async Task<ActionResult<TblUsersModel>> register(RegistrationModel userTbl)
        {
            string filepath = "";
            var user_list = _context.TblUsersModels.AsEnumerable().Where(a => a.Username.Equals(userTbl.Username, StringComparison.Ordinal)).ToList();
            if (user_list.Count == 0)
            {
                var email_count = _context.TblUsersModels.Where(a => a.Email == userTbl.Email).ToList();
                if (email_count.Count != 0)
                {
                    Stats = "Error";
                    Mess = "Email Already Used!";
                    JWT = "";
                }
                else
                {
                    StringBuilder str_build = new StringBuilder();
                    Random random = new Random();
                    int length = 8;
                    char letter;

                    for (int i = 0; i < length; i++)
                    {
                        double flt = random.NextDouble();
                        int shift = Convert.ToInt32(Math.Floor(25 * flt));
                        letter = Convert.ToChar(shift + 2);
                        str_build.Append(letter);
                    }

                    var token = Cryptography.Encrypt(str_build.ToString());
                    string strtokenresult = token;
                    string[] charsToRemove = new string[] { "/", ",", ".", ";", "'", "=", "+" };
                    foreach (var c in charsToRemove)
                    {
                        strtokenresult = strtokenresult.Replace(c, string.Empty);
                    }
                    if (userTbl.FilePath == null)
                    {
                        filepath = "";
                    }
                    else
                    {
                        filepath = userTbl.FilePath.Replace(" ", "%20");
                    }
                    string fullname = userTbl.Fname + ", " + userTbl.Mname + ", " + userTbl.Lname;
                    string user_insert = $@"INSERT INTO [dbo].[tbl_UsersModel]
                                           ([Username]
                                           ,[Password]
                                           ,[Fullname]
                                           ,[Fname]
                                           ,[Lname]
                                           ,[Mname]
                                           ,[Email]
                                           ,[Gender]
                                           ,[EmployeeID]
                                           ,[JWToken]
                                           ,[FilePath]
                                           ,[Active]
                                           ,[Cno]
                                           ,[Address]
                                           ,[Status]
                                           ,[Date_Created]
                                           ,[CenterId]
                                           ,[AgreementStatus])
                                     VALUES
                                           ('" + userTbl.Username + "'" +
                                            ",'" + Cryptography.Encrypt(userTbl.Password) + "'," +
                                           "'" + fullname + "'," +
                                           "'" + userTbl.Fname + "'," +
                                           "'" + userTbl.Lname + "'," +
                                           "'" + userTbl.Mname + "'," +
                                           "'" + userTbl.Email + "'," +
                                           "'" + userTbl.Gender + "'," +
                                           "'" + userTbl.EmployeeId + "'," +
                                           "'" + string.Concat(strtokenresult.TakeLast(15)) + "'," +
                                           "'" + filepath + "'," +
                                           "'1'," +
                                           "'" + userTbl.Cno + "'," +
                                           "'" + userTbl.Address + "'," +
                                           "'6'," +
                                           "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                           "'" + userTbl.CenterId + "'," +
                                           "'" + userTbl.AgreementStatus + "')";
                    db.DB_WithParam(user_insert);


                    const string chars = "0123456789";
                    Random random_OTP = new Random();
                    string otp_res = "";
                    char[] randomArray = new char[8];
                    for (int i = 0; i < 6; i++)
                    {
                        otp_res += chars[random.Next(chars.Length)];
                    }
                    TblRegistrationOtpmodel items = new TblRegistrationOtpmodel();
                    items.Email = userTbl.Email;
                    items.Otp = otp_res.ToString();

                    MailSender email =  new MailSender(_emailsettings);
                    email.sendOtpMail(items);

                    string OTPInsert = $@"insert into tbl_RegistrationOTPModel (email,OTP,status) values ('" + userTbl.Email + "','" + otp_res + "','4')";
                    db.DB_WithParam(OTPInsert);

                    Stats = "Ok";
                    Mess = "User is for Verification, OTP Already Send!";
                    JWT = string.Concat(strtokenresult.TakeLast(15));
                }
            }
            else
            {
                Stats = "Error";
                Mess = "Username Already Exist!";
                JWT = "";
            }
            StatusReturns result = new StatusReturns
            {
                Status = Stats,
                Message = Mess,
                JwtToken = JWT
            };

            return Ok(result);
        }


        // POST: user/rememberPassword
        [HttpPost]
        public async Task<IActionResult> rememberPassword(String token)
        {
            if (_context.TblUsersModels == null)
            {
                return Problem("Entity set 'PCC_DEVContext.TblUsersModels'  is null!");
            }

            var userModel = await _context.TblUsersModels.Where(userModel => userModel.RememberToken == token).FirstAsync();
            if (userModel == null) 
            {
                return NotFound("Token: "+ token + " not found");
            }
            return Ok(userModel?.Jwtoken);
        }

        [HttpPost]
        public async Task<IActionResult> resetPassword(ResetPasswordModel resetPasswordModel)
        {
            if (_context.TblUsersModels == null)
            {
                return Problem("Entity set 'PCC_DEVContext.TblUsersModels'  is null!");
            }

            var userModel = await _context.TblUsersModels.Where(user => user.Email == resetPasswordModel.Email).FirstAsync();
            if (userModel == null)
            {
                return Conflict("User not Found !!");
            }

            userModel.Password = Cryptography.Encrypt(resetPasswordModel.Password);
            _context.Entry(userModel).State = EntityState.Modified;
            
            await _context.SaveChangesAsync();
            
            return Ok("Password Reset successful for user: " + resetPasswordModel.Email);
        }

        [HttpPost]
        public async Task<IActionResult> forgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            if (_context.TblUsersModels == null)
            {
                return Problem("Entity set 'PCC_DEVContext.TblUsersModels'  is null!");
            }

            var isEmailExists = _context.TblUsersModels.Any(user => !user.DeleteFlag != false && user.Email == forgotPasswordModel.Email);

            if (!isEmailExists)
            {
                return BadRequest("Email does not exists!!");
            }

            try
            {
                DateTime dateCreated = DateTime.Now;
                DateTime expirydate = dateCreated.AddDays(1);
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(forgotPasswordModel.Email);
                string emailBase64 = System.Convert.ToBase64String(plainTextBytes);

                var tokenModel = new TblTokenModel
                {
                    Token = emailBase64,
                    ExpiryDate = expirydate,
                    Status = "5",
                    DateCreated = dateCreated

                };

                _context.TblTokenModels.Add(tokenModel);
                _context.SaveChanges();

                MailSender _mailSender = new MailSender(_emailsettings);
                _mailSender.sendForgotPasswordMail(forgotPasswordModel.Email, forgotPasswordModel.ForgotPasswordLink + "?token="+emailBase64);
                return Ok("Password Reset Email sent successfully!");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> resendForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            if (_context.TblUsersModels == null)
            {
                return Problem("Entity set 'PCC_DEVContext.TblUsersModels' is null!");
            }

            var isEmailExists = _context.TblUsersModels.Any(user => user.Email == forgotPasswordModel.Email);
            //var isEmailExists = _context.TblUsersModels.Any(user => !user.DeleteFlag && user.Email == email);

            if (!isEmailExists)
            {
                return BadRequest("Email does not exists!!");
            }

            try
            {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(forgotPasswordModel.Email);
                string emailBase64 = System.Convert.ToBase64String(plainTextBytes);

                MailSender _mailSender = new MailSender(_emailsettings);
                _mailSender.sendForgotPasswordMail(forgotPasswordModel.Email, forgotPasswordModel.ForgotPasswordLink + "?token=" + emailBase64);
                return Ok("Password Reset Email resent successfully!");
            }
            catch (Exception ex)
            {
                
                return Problem(ex.Message);
            }
        }

        [HttpGet("{token}")]
        public async Task<IActionResult> CheckResetPasswordLinkExpiry(String token)
        {
            if (_context.TblUsersModels == null)
            {
                return Problem("Entity set 'PCC_DEVContext.TblUsersModels' is null!");
            }

            var tokenModel = await _context.TblTokenModels.Where(tokenModel => tokenModel.Token == token).OrderByDescending(tokenModel => tokenModel.Id).FirstOrDefaultAsync();

            if(tokenModel == null)
            {
                return Conflict("Token does not exists !!");
            }

            return Ok(tokenModel.ExpiryDate.ToString("yyyy-MM-dd hh:mm:ss tt"));
        }

        private bool UserTblExists(int id)
        {
            return (_context.TblUsersModels?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
