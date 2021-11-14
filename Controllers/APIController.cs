using FH5RP.Hubs;
using FH5RP.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace FH5RP.Controllers
{
    [ApiController]
    [Route("api")]
    public class APIController : Controller
    {
        private IHubContext<TelemetryDataHub> ctx;
        public APIController(IHubContext<TelemetryDataHub> context)
        {
            ctx = context;
        }

        [HttpGet("test")]
        public ActionResult<string> ApiTest()
        {
            return Ok("hello!");
        }

        [HttpGet("datatest")]
        public ActionResult<string> DataTest([FromForm] string data)
        {
            return Ok(data);
        }

        [HttpPost("updatedata")]
        [Consumes("application/json")]
        public async Task<IActionResult> UpdateData([FromForm] string data)
        {
            await ctx.Clients.All.SendAsync("SendData", TelemetryData.FromString(data));
            Console.WriteLine("[api] Received update data");
            return Accepted();
        }
    }
}
