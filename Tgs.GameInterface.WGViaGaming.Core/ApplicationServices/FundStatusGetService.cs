using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WGViaGaming.Core.ApplicationServices;

public class FundStatusGetService(IGameApiClient client) : IGetService<FundStatusQuery, bool>
{
	public ValueTask<bool> GetAsync(
		FundStatusQuery query,
		CancellationToken cancellationToken = default)
		=> client.AccessMoneyQueryAsync(
			apiUrl: query.ApiUrl,
			partnerCode: query.PartnerCode,
			secretKey: query.SecretKey,
			secretIV: query.SecretIV,
			orderId: query.OrderId,
			cancellationToken: cancellationToken);
}
