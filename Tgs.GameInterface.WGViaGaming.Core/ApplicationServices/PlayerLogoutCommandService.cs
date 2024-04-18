using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WGViaGaming.Core.ApplicationServices;

internal class PlayerLogoutCommandService(IGameApiClient client) : ICommandService<PlayerLogoutCommand>
{
	public ValueTask ExecuteAsync(
		PlayerLogoutCommand command,
		CancellationToken cancellationToken = default)
		=> client.LogoutAsync(
			apiUrl: command.ApiUrl,
			partnerCode: command.PartnerCode,
			secretKey: command.SecretKey,
			secretIV: command.SecretIV,
			memberCode: command.PlayerCode,
			cancellationToken: cancellationToken);
}
