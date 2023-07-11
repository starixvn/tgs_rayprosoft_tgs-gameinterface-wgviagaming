using System.ComponentModel.DataAnnotations;

namespace Tgs.GameInterface.WGViaGaming.ViewModels;

public class GameRecordViewModel
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
    public DateTimeOffset StartTime { get; init; } = default!;

    [Required]
    public DateTimeOffset EndTime { get; init; } = default!;

    [Required]
    public int PageIndex { get; init; } = 1;

    [Required]
    public int PageSize { get; init; } = 1000;
}
