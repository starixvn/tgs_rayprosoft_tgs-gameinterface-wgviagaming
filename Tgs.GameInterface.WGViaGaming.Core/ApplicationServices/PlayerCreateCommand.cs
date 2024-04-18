namespace Tgs.GameInterface.WGViaGaming.Core.ApplicationServices;

public record PlayerCreateCommand(
	Uri ApiUrl,
	string PartnerCode,
	string SecretKey,
	string SecretIV,
	string PlayerCode,
	string CurrencyCode);
