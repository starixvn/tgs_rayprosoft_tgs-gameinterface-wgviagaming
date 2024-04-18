using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WGViaGaming.Core.ApplicationServices;

internal class FundInCommandService(IGameApiClient client) : ICommandService<FundInCommand>
{
	public ValueTask ExecuteAsync(
		FundInCommand command,
		CancellationToken cancellationToken = default)
		=> client.AccessMoneyWithdrawalAsync(
			apiUrl: command.ApiUrl,
			partnerCode: command.PartnerCode,
			secretKey: command.SecretKey,
			secretIV: command.SecretIV,
			memberCode: command.PlayerCode,
			orderId: command.OrderId,
			amount: command.Amount,
			cancellationToken: cancellationToken);
}
