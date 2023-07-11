namespace Tgs.GameInterface.WGViaGaming.Core.ApplicationServices;

public record GameRecordQuery(
    Uri ApiUrl,
    string PartnerCode,
    string SecretKey,
    string SecretIV,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime,
    int PageIndex = 1,
    int PageSize = 1000);
