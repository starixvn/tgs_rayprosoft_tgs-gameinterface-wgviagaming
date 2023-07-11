using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WGViaGaming.Core.ApplicationServices;

internal class PlayerCreateCommandService : ICommandService<PlayerCreateCommand>
{
    private readonly IGameApiClient _client;

    public PlayerCreateCommandService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask ExecuteAsync(PlayerCreateCommand command, CancellationToken cancellationToken = default)
        => _ = await _client
        .PrepareLoginAsync(
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
