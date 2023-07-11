namespace Tgs.GameInterface.WGViaGaming.Core.ApplicationServices;

public record PlayerLogoutCommand(
    Uri ApiUrl,
    string PartnerCode,
    string SecretKey,
    string SecretIV,
    string PlayerCode);
