using Tgs.GameInterface.WGViaGaming.Core.Models;

namespace Tgs.GameInterface.WGViaGaming.Core;

public interface IGameApiClient
{
	ValueTask AccessMoneyWithdrawalAsync(
		Uri apiUrl,
		string partnerCode,
		string secretKey,
		string secretIV,
		string memberCode,
		string orderId,
		decimal amount,
		CancellationToken cancellationToken = default);

	ValueTask AccessMoneyDepositAsync(
		Uri apiUrl,
		string partnerCode,
		string secretKey,
		string secretIV,
		string memberCode,
		string orderId,
		decimal amount,
		CancellationToken cancellationToken = default);

	ValueTask<bool> AccessMoneyQueryAsync(
		Uri apiUrl,
		string partnerCode,
		string secretKey,
		string secretIV,
		string orderId,
		CancellationToken cancellationToken = default);

	ValueTask<(bool IsPlayerExist, decimal Balance)> GetBalanceAsync(
		Uri apiUrl,
		string partnerCode,
		string secretKey,
		string secretIV,
		string memberCode,
		CancellationToken cancellationToken = default);

	IAsyncEnumerable<ShowBetInfo> ShowBetListAsync(
		Uri apiUrl,
		string partnerCode,
		string secretKey,
		string secretIV,
		DateTimeOffset startTime,
		DateTimeOffset endTime,
		int pageIndex = 1,
		int pageSize = 1000,
		CancellationToken cancellationToken = default);

	ValueTask<string> LoginAsync(
		Uri apiUrl,
		string partnerCode,
		string token,
		string lang,
		CancellationToken cancellationToken = default);

	ValueTask<string> PrepareLoginAsync(
		Uri apiUrl,
		string partnerCode,
		string secretKey,
		string secretIV,
		string memberCode,
		string nickname,
		string currencyCode,
		CancellationToken cancellationToken = default);

	ValueTask LogoutAsync(
		Uri apiUrl,
		string partnerCode,
		string secretKey,
		string secretIV,
		string memberCode,
		CancellationToken cancellationToken = default);
}
