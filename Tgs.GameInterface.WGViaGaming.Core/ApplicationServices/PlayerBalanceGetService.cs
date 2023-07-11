using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WGViaGaming.Core.ApplicationServices;

public class PlayerBalanceGetService : IGetService<PlayerBalanceQuery, PlayerBalanceResult>
{
    private readonly IGameApiClient _client;

    public PlayerBalanceGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<PlayerBalanceResult> GetAsync(
        PlayerBalanceQuery query,
        CancellationToken cancellationToken = default)
    {
        (bool IsPlayerExist, decimal Balance) = await _client
            .GetBalanceAsync(
            apiUrl: query.ApiUrl,
            partnerCode: query.PartnerCode,
            secretKey: query.SecretKey,
            secretIV: query.SecretIV,
            memberCode: query.PlayerCode,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new PlayerBalanceResult(
            IsPlayerExist: IsPlayerExist,
            Balance: Balance);
    }
}
