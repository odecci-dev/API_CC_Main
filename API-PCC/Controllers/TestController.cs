using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using GoldOneAPI.Manager;

namespace GoldOneAPI.Controllers
{
    [Authorize("ApiKey")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        DbManager db = new DbManager();
        DBMethods dbmet = new DBMethods();


        [HttpGet]
        public async Task<IActionResult> TestList()
        {
            try
            {
                var result = "TEST";
                return Ok(result);
            }

            catch (Exception ex)
            {
                return BadRequest("ERROR");
            }
        }
    }
}