using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WGViaGaming.Core.ApplicationServices;

public class PlayerBalanceGetService(IGameApiClient client) : IGetService<PlayerBalanceQuery, PlayerBalanceResult>
{
	public async ValueTask<PlayerBalanceResult> GetAsync(
		PlayerBalanceQuery query,
		CancellationToken cancellationToken = default)
	{
		(bool IsPlayerExist, decimal Balance) = await client.GetBalanceAsync(
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
