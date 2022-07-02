using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace CommunityOfDevelopers.ExternalShare.Features.Common;

[Route("api/[controller]")]
[ApiController]
public class CommonController : ControllerBase
{
    [HttpGet("site")]
    public async Task<IActionResult> GetSite([FromQuery] string url, CancellationToken token)
    {
        using var client = new HttpClient();

        var response = await client.GetAsync(url, token);


        return Ok(Convert.ToBase64String(Encoding.UTF8.GetBytes(await response.Content.ReadAsStringAsync(token))));
    }
    [HttpGet("proxy-site")]
    public async Task<IActionResult> ProxySite([FromQuery] string url, CancellationToken token)
    {
        using var client = new HttpClient();

        var response = await client.GetAsync(url, token);

        return Ok(await response.Content.ReadAsStringAsync(token));
    }
}