using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommunityOfDevelopers.ExternalShare.Features.Common
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        [HttpGet("site")]
        public async Task<IActionResult> GetSite([FromQuery]string url, CancellationToken token)
        {
            using var client = new HttpClient();

            return Ok(await client.GetStringAsync(url, token));
        }
    }
}
