using System.ComponentModel.DataAnnotations;

namespace Tgs.GameInterface.WGViaGaming.ViewModels;

public class PlayerBalanceViewModel
{
    [Required]
    public Uri ApiUrl { get; init; } = default!;

    [Required]
    public string PartnerCode { get; init; } = default!;

    [Required]
    public string SecretKey { get; init; } = default!;

    [Required]
    public string SecretIV { get; init; } = default!;

    [Required]
    public string PlayerCode { get; init; } = default!;
}
