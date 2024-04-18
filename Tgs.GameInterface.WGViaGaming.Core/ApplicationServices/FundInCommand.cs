namespace Tgs.GameInterface.WGViaGaming.Core.ApplicationServices;

public record FundInCommand(
	Uri ApiUrl,
	string PartnerCode,
	string SecretKey,
	string SecretIV,
	string PlayerCode,
	string OrderId,
	decimal Amount);
