using API_PCC.Data;
using API_PCC.Models;
using API_PCC.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            if (_context.TblRegistrationOtpmodels == null)
            {
                return Problem("Entity set 'PCC_DEVContext.OTP' is null!");
            }

            try
            {
                var registrationOtpModel = _context.TblRegistrationOtpmodels.Where(otpModel => otpModel.Otp == data.Otp && otpModel.Email == data.Email && otpModel.Status == 10).FirstOrDefault();
                if (registrationOtpModel != null)
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
                    return Ok("OTP sent successfully!!");
                }
                else
                {
                    return BadRequest("Record not found on database!");
                }
            }
            catch (Exception ex)
            {
                string exception = ex.GetBaseException().ToString();
                return Problem(exception);
            }
        }

        [HttpPost]
        public async Task<IActionResult> VerifyOTP(TblRegistrationOtpmodel data)
        {
            try
            {
                if (_context.TblRegistrationOtpmodels == null)
                {
                    return Problem("Entity set 'PCC_DEVContext.OTP' is null!");
                }
                var registOtpModels = _context.TblRegistrationOtpmodels.Where(otpModel => otpModel.Email == data.Email && (otpModel.Status == 9 || otpModel.Status == 10)).FirstOrDefault();

                if (registOtpModels != null)
                {
                    if (registOtpModels.Otp == data.Otp)
                    {
                        registOtpModels.Status = 1;
                        _context.Entry(registOtpModels).State = EntityState.Modified;

                        var userModel = _context.TblUsersModels.Where(user => user.Email == data.Email && user.Otp == data.Otp).FirstOrDefault();
                        _context.Entry(userModel).State = EntityState.Modified;
                        userModel.Status = 1;

                        await _context.SaveChangesAsync();
                        return Ok("OTP verification successful!");
                    } else
                    {
                        var userModel = _context.TblUsersModels.Where(user => user.Email == data.Email).FirstOrDefault();
                        _context.Entry(userModel).State = EntityState.Modified;
                        userModel.Status = 10;
                        return BadRequest("OTP verification unsuccessful!");
                    }
                    
                }
                else
                {
                    return BadRequest("Record not found on database!");
                }
            }

            catch (Exception ex)
            {
                String exception = ex.GetBaseException().ToString();
                return Problem(exception);
            }
        }


        [HttpPost]
        public async Task<IActionResult> ResendOTP(TblRegistrationOtpmodel data)
        {
            try
            {
                if (_context.TblRegistrationOtpmodels == null)
                {
                    return Problem("Entity set 'PCC_DEVContext.OTP' is null!");
                }

                var registrationOtpModel = _context.TblRegistrationOtpmodels.Where(otpModel => otpModel.Otp == data.Otp &&  otpModel.Email == data.Email && otpModel.Status == 10).FirstOrDefault();
                if (registrationOtpModel != null)
                {
                    _mailSender.sendOtpMail(data);
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
                return Problem(exception);
            }
        }

    }
}
