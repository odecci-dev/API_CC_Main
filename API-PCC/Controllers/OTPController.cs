using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API_PCC.Data;
using API_PCC.Models;
using static API_PCC.Controllers.UserController;
using API_PCC.Utils;
using Microsoft.EntityFrameworkCore;

namespace API_PCC.Controllers
{
    [Authorize("ApiKey")]
    [Route("[controller]/[action]")]
    [ApiController]

    public class OTPController : ControllerBase
    {
        MailSender _mailSender;
        private readonly PCC_DEVContext _context;

        public OTPController(PCC_DEVContext context, IConfiguration configuration)
        {
            _context = context;
            _mailSender = new MailSender(configuration);
        }

        [HttpPost]
        public async Task<IActionResult> SendOTP(TblRegistrationOtpmodel data)
        {
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
            }

            catch (Exception ex)
            {
                string exception = ex.GetBaseException().ToString();
                return Problem(exception);
            }
            return Ok("OTP sent successfully!!");
        }

        [HttpPost]
        public async Task<IActionResult> VerifyOTP(TblRegistrationOtpmodel data)
        {
            try
            {
                var registOtpModels = _context.TblRegistrationOtpmodels.Where(otpModel => otpModel.Otp == data.Otp && otpModel.Email == data.Email && (otpModel.Status == 9 || otpModel.Status == 10));

                if (registOtpModels != null)
                {
                    data.Status = 1;
                    _context.Entry(data).State = EntityState.Modified;

                    var userModel = _context.TblUsersModels.Where(user => user.Email == data.Email && user.Otp == data.Otp).First();
                    _context.Entry(userModel).State = EntityState.Modified;
                    userModel.Status = 1;

                    await _context.SaveChangesAsync();
                    return Ok("OTP verification successful!");
                }
                else
                {
                    var userModel = _context.TblUsersModels.Where(user => user.Email == data.Email && user.Otp == data.Otp).First();
                    _context.Entry(userModel).State = EntityState.Modified;
                    userModel.Status = 10;
                    return BadRequest("OTP verification unsuccessful!");
                }
            }

            catch (Exception ex)
            {
                String exception = ex.GetBaseException().ToString();
                return BadRequest(exception);
            }
        }


        [HttpPost]
        public async Task<IActionResult> ResendOTP(TblRegistrationOtpmodel data)
        {
            try
            {
                var registrationOtpModel = _context.TblRegistrationOtpmodels.Where(otpModel => otpModel.Email == data.Email && otpModel.Status == 10).First();
                if (registrationOtpModel != null)
                {
                    registrationOtpModel.Otp = data.Otp;
                    _context.Entry(registrationOtpModel).State = EntityState.Modified;

                    await _context.SaveChangesAsync();

                    ; _mailSender.sendOtpMail(data);
                    return Ok("Resending OTP successful!");
                }
                else
                {
                    return BadRequest("Record not found on database!");
                }
            }
            catch (Exception ex)
            {
                String exception = ex.GetBaseException().ToString();
                return BadRequest(exception);
            }
        }
    }
}
