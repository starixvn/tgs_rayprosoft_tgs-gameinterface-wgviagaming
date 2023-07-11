using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WGViaGaming.Core.ApplicationServices;

internal class PlayerLogoutCommandService : ICommandService<PlayerLogoutCommand>
{
    private readonly IGameApiClient _client;

    public PlayerLogoutCommandService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask ExecuteAsync(
        PlayerLogoutCommand command,
        CancellationToken cancellationToken = default)
        => _client
            .LogoutAsync(
                command.ApiUrl,
                command.PartnerCode,
                command.SecretKey,
                command.SecretIV,
                command.PlayerCode,
                cancellationToken);
}
