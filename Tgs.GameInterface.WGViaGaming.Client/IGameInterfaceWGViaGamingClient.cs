using Tgs.GameInterface.WGViaGaming.Protos;

namespace Tgs.GameInterface.WGViaGaming.Client;

public interface IGameInterfaceWGViaGamingClient
{
	ValueTask<string> CheckingServiceAsync(
		string machineName,
		CancellationToken cancellationToken = default);

	ValueTask FundInAsync(
		Uri apiUrl,
		string partnerCode,
		string secretKey,
		string secretIV,
		string playerCode,
		string orderId,
		decimal amount,
		CancellationToken cancellationToken = default);

	ValueTask FundOutAsync(
		Uri apiUrl,
		string partnerCode,
		string secretKey,
		string secretIV,
		string playerCode,
		string orderId,
		decimal amount,
		CancellationToken cancellationToken = default);

	ValueTask<bool> FundStatusAsync(
		Uri apiUrl,
		string partnerCode,
		string secretKey,
		string secretIV,
		string orderId,
		CancellationToken cancellationToken = default);

	IAsyncEnumerable<GameRecordInfo> GameRecordAsync(
		Uri apiUrl,
		string partnerCode,
		string secretKey,
		string secretIV,
		DateTimeOffset startTime,
		DateTimeOffset endTime,
		int pageIndex = 1,
		int pageSize = 1000,
		CancellationToken cancellationToken = default);

	ValueTask<string> GameUrlAsync(
		Uri apiUrl,
		string partnerCode,
		string secretKey,
		string secretIV,
		string playerCode,
		string currencyCode,
		string lang,
		CancellationToken cancellationToken = default);

	ValueTask<PlayerBalanceReply> PlayerBalanceAsync(
		Uri apiUrl,
		string partnerCode,
		string secretKey,
		string secretIV,
		string playerCode,
		CancellationToken cancellationToken = default);

	ValueTask PlayerCreateAsync(
		Uri apiUrl,
		string partnerCode,
		string secretKey,
		string secretIV,
		string playerCode,
		string currencyCode,
		CancellationToken cancellationToken = default);

	ValueTask PlayerLogoutAsync(
		Uri apiUrl,
		string partnerCode,
		string secretKey,
		string secretIV,
		string playerCode,
		CancellationToken cancellationToken = default);
}
