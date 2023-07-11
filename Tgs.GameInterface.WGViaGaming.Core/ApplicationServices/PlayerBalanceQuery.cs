namespace Tgs.GameInterface.WGViaGaming.Core.ApplicationServices;

public record PlayerBalanceQuery(
    Uri ApiUrl,
    string PartnerCode,
    string SecretKey,
    string SecretIV,
    string PlayerCode);
