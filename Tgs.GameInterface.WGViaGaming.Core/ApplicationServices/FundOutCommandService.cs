using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WGViaGaming.Core.ApplicationServices;

internal class FundOutCommandService(IGameApiClient client) : ICommandService<FundOutCommand>
{
	public ValueTask ExecuteAsync(
		FundOutCommand command,
		CancellationToken cancellationToken = default)
		=> client.AccessMoneyDepositAsync(
			apiUrl: command.ApiUrl,
			partnerCode: command.PartnerCode,
			secretKey: command.SecretKey,
			secretIV: command.SecretIV,
			memberCode: command.PlayerCode,
			orderId: command.OrderId,
			amount: command.Amount,
			cancellationToken: cancellationToken);
}
