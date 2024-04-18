namespace Tgs.GameInterface.WGViaGaming.Core.ApplicationServices;

public record GameUrlQuery(
	Uri ApiUrl,
	string PartnerCode,
	string SecretKey,
	string SecretIV,
	string PlayerCode,
	string CurrencyCode,
	string Lang);
