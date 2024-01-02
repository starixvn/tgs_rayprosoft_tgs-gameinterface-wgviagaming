using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WGViaGaming.Core.ApplicationServices;

public class GameUrlGetService(IGameApiClient client) : IGetService<GameUrlQuery, string>
{
    public async ValueTask<string> GetAsync(
        GameUrlQuery query,
        CancellationToken cancellationToken = default)
    {
        var token = await client
            .PrepareLoginAsync(
            apiUrl: query.ApiUrl,
            partnerCode: query.PartnerCode,
            secretKey: query.SecretKey,
            secretIV: query.SecretIV,
            memberCode: query.PlayerCode,
            nickname: query.PlayerCode,
            currencyCode: query.CurrencyCode,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return await client
            .LoginAsync(
            apiUrl: query.ApiUrl,
            partnerCode: query.PartnerCode,
            token: token,
            lang: query.Lang,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}
