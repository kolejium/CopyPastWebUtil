using System.ComponentModel.DataAnnotations;

namespace CommunityOfDevelopers.ExternalShare.Dto.Features.Common;

public class ProxyRequest
{
    [Required]
    [MinLength(2)]
    [MaxLength(256)]
    public string Url { get; set; } = "";

    public string? Proxy { get; set; } = null;
}