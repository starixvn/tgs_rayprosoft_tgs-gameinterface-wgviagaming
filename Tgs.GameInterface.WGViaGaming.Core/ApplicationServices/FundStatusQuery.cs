namespace Tgs.GameInterface.WGViaGaming.Core.ApplicationServices;

public record FundStatusQuery(
    Uri ApiUrl,
    string PartnerCode,
    string SecretKey,
    string SecretIV,
    string OrderId);
