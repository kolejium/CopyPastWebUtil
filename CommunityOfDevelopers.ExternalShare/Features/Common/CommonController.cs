using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using CommunityOfDevelopers.ExternalShare.Dto.Features.Common;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using MediaTypeHeaderValue = System.Net.Http.Headers.MediaTypeHeaderValue;
using Uri = System.Uri;

namespace CommunityOfDevelopers.ExternalShare.Features.Common;

[Route("api/[controller]")]
[ApiController]
public class CommonController : ControllerBase
{
    [HttpGet("proxy")]
    public async Task<IActionResult> GetSite([FromQuery] ProxyRequest request, CancellationToken token)
    {
        try
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(request.Url, token);

            if (response.IsSuccessStatusCode)
            {
                switch (response.Content.Headers.ContentType?.MediaType)
                {
                    case var s when s != null && s.StartsWith("text/html"):
                        if (string.IsNullOrEmpty(request.Proxy))
                            return BadRequest();

                        var document = new HtmlDocument();

                        document.LoadHtml(await response.Content.ReadAsStringAsync(token));

                        ChangeLinks(ref document, request.Url, request.Proxy!);

                        var html = document.DocumentNode;//.SelectSingleNode("/html");

                        //var ms = new MemoryStream(Encoding.UTF8.GetBytes(html.OuterHtml));

                        return Content(html.OuterHtml, "text/html");// Ok(html.OuterHtml);
                    case var s when s != null && s.StartsWith("image") || s.StartsWith("audio"):
                        return File(await response.Content.ReadAsStreamAsync(token),
                            response.Content.Headers.ContentType?.MediaType!);
                    default:
                        var value = await response.Content.ReadAsStringAsync(token);
                        return Content(ChangeLinks(value, request.Url, request.Proxy), response.Content.Headers.ContentType?.MediaType!);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return BadRequest();


        //var webGet = new HtmlWeb();
        ////var response = await client.GetAsync(url, token);
        ////var site = await response.Content.ReadAsStringAsync(token);
        //var document = webGet.Load(url);

        //ChangeLinks(ref document, url, proxy);

        //var html = document.DocumentNode.SelectSingleNode("/html");


        //return Ok(html.OuterHtml);

        //var regex = new Regex(
        //    @"[(http(s)?):\/\/(www\.)?a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)");
        //var builder = new StringBuilder();
        //var matches = regex.Matches(site);
        //var prevIndex = 0;

        //var @str = "";
        //foreach (Match match in matches)
        //{
        //    builder.Append(site, prevIndex, match.Index - prevIndex);
        //    builder.Append($"[proxy]{match.Value}");
        //    prevIndex = match.Index + match.Length;
        //}

        //builder.Append(site, prevIndex, site.Length - prevIndex);
        //return Ok(builder.ToString());

    }

    private static string ChangeLinks(string value, string url, string proxy)
    {
        var regex = new Regex(@"url\(([\""|\']?)([^\)])+\)");
        var sb = new StringBuilder();
        var index = 0;
        foreach (Match match in regex.Matches(value))
        {
            if (match.Value.StartsWith("url(\"data:"))
                continue;

            sb.Append(value, index, match.Index - index);

            var quotationMarks = match.Value[4];
            var hasQuotationMarks = quotationMarks is '\'' or '\"';
            try
            {
                var innerUrl = hasQuotationMarks ? match.Value.Remove(0, 5).Remove(match.Value.Length - 5 - 2) : match.Value.Remove(0, 4).Remove(match.Value.Length - 4 - 1);

                innerUrl = IsRelative(innerUrl) ? proxy + HandleUrl(innerUrl, url) : HandleUrl(innerUrl, url);
                index = match.Index + match.Length;

                sb.Append(hasQuotationMarks ? $"url({quotationMarks}{innerUrl}{quotationMarks})" : $"url({innerUrl}");
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        sb.Append(value, index, value.Length - index);

        return sb.ToString();
    }

    private static string HandleUrl(string value, string url)
    {
        if (value.StartsWith("../"))
        {
            url = url[..url.LastIndexOf('/')];

            while (value.StartsWith("../"))
            {
                url = url[..url.LastIndexOf('/')];
                value = value.Remove(0, "../".Length);
            }
        }

        return url + (url.EndsWith('/') || value.StartsWith('/') ? "" : "/") + value;
    }

    private static void ChangeLinks(ref HtmlDocument doc, string url, string proxy)
    {
        //process all tage with link references
        HtmlNodeCollection links = doc.DocumentNode.SelectNodes("//*[@background or @lowsrc or @src or @href]");
        if (links == null)
            return;

        foreach (HtmlNode link in links)
        {

            if (link.Attributes["background"] != null)
                link.Attributes["background"].Value = IsRelative(link.Attributes["background"].Value) ? proxy + url + link.Attributes["background"].Value : proxy + link.Attributes["background"].Value;
            if (link.Attributes["href"] != null)
                link.Attributes["href"].Value = IsRelative(link.Attributes["href"].Value) ? proxy + url + link.Attributes["href"].Value : proxy + link.Attributes["href"].Value;
            if (link.Attributes["lowsrc"] != null)
                link.Attributes["lowsrc"].Value = IsRelative(link.Attributes["lowsrc"].Value) ? proxy + url + link.Attributes["lowsrc"].Value : proxy + link.Attributes["lowsrc"].Value;

            if (link.Attributes["src"] != null)
                link.Attributes["src"].Value = IsRelative(link.Attributes["src"].Value) ? proxy + url + link.Attributes["src"].Value : proxy + link.Attributes["src"].Value;
        }
    }

    private static bool IsRelative(string url) => Uri.TryCreate(url, UriKind.Relative, out _);


    [HttpGet("proxy-site")]
    public async Task<IActionResult> ProxySite([FromQuery] string url, CancellationToken token)
    {
        using var client = new HttpClient();

        var response = await client.GetAsync(url, token);

        if (response.Content.Headers.ContentType?.MediaType != null && response.Content.Headers.ContentType.MediaType.StartsWith("image"))
            return File(await response.Content.ReadAsStreamAsync(token), response.Content.Headers.ContentType.MediaType!);

        //if (response.Content.Headers.ContentType?.MediaType != null && response.Content.Headers.ContentType.MediaType.StartsWith("application/javascript"))
        //    return Content(await response.Content.ReadAsStringAsync(token), response.Content.Headers.ContentType.MediaType!);

        return Content(await response.Content.ReadAsStringAsync(token), response.Content.Headers.ContentType?.MediaType!);

        //return Ok(await response.Content.ReadAsStringAsync(token));
    }
}