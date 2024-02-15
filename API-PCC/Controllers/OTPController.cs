using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API_PCC.Data;
using API_PCC.Models;
using static API_PCC.Controllers.UserController;

namespace API_PCC.Controllers
{
    [Authorize("ApiKey")]
    [Route("[controller]/[action]")]
    [ApiController]

    public class OTPController : ControllerBase
    {
        private readonly PCC_DEVContext _context;

        public OTPController(PCC_DEVContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async void resendOTP(int id)
        {

        }

        [HttpGet("{id}")]
        public async void verifyOTP(int id)
        {

        }
    }
}
