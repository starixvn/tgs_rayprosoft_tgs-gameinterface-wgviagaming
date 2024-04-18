using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WGViaGaming.Core.ApplicationServices;

internal class PlayerCreateCommandService(IGameApiClient client) : ICommandService<PlayerCreateCommand>
{
	public async ValueTask ExecuteAsync(PlayerCreateCommand command, CancellationToken cancellationToken = default)
		=> _ = await client.PrepareLoginAsync(
			apiUrl: command.ApiUrl,
			partnerCode: command.PartnerCode,
			secretKey: command.SecretKey,
			secretIV: command.SecretIV,
			memberCode: command.PlayerCode,
			nickname: command.PlayerCode,
			currencyCode: command.CurrencyCode,
			cancellationToken: cancellationToken)
		.ConfigureAwait(false);
}
