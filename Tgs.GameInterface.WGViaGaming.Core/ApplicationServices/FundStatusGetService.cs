using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WGViaGaming.Core.ApplicationServices;

public class FundStatusGetService : IGetService<FundStatusQuery, bool>
{
    private readonly IGameApiClient _client;

    public FundStatusGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask<bool> GetAsync(
        FundStatusQuery query,
        CancellationToken cancellationToken = default)
        => _client
        .AccessMoneyQueryAsync(
            apiUrl: query.ApiUrl,
            partnerCode: query.PartnerCode,
            secretKey: query.SecretKey,
            secretIV: query.SecretIV,
            orderId: query.OrderId,
            cancellationToken: cancellationToken);
}
