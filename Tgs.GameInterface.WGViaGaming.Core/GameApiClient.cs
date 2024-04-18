using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Web;
using Microsoft.Extensions.Logging;
using Tgs.GameInterface.WGViaGaming.Core.Exceptions;
using Tgs.GameInterface.WGViaGaming.Core.Extensions;
using Tgs.GameInterface.WGViaGaming.Core.Models;

namespace Tgs.GameInterface.WGViaGaming.Core;

internal class GameApiClient(
	ILogger<GameApiClient> logger,
	IHashCryptography cryptography,
	ISystemClock systemClock,
	HttpClient client) : IGameApiClient
{
	public async ValueTask AccessMoneyDepositAsync(
		Uri apiUrl,
		string partnerCode,
		string secretKey,
		string secretIV,
		string memberCode,
		string orderId,
		decimal amount,
		CancellationToken cancellationToken = default)
	{
		var data = $"{memberCode}|1|{orderId}|{amount}";

		var jsonNode = await GetAsync(
			apiUrl: apiUrl,
			partnerCode: partnerCode,
			secretKey: secretKey,
			secretIV: secretIV,
			action: "AccessMoney",
			data: data,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		if (!jsonNode["ReturnCode"]!.ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
		{
			throw new ReturnCodeException(jsonNode["ReturnCode"]!.ToString());
		}
	}

	public async ValueTask<bool> AccessMoneyQueryAsync(
		Uri apiUrl,
		string partnerCode,
		string secretKey,
		string secretIV,
		string orderId,
		CancellationToken cancellationToken = default)
	{
		var data = orderId;

		var jsonNode = await GetAsync(
			apiUrl: apiUrl,
			partnerCode: partnerCode,
			secretKey: secretKey,
			secretIV: secretIV,
			action: "AccessMoneyQuery",
			data: data,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return jsonNode["ReturnCode"]!.ToString().Equals("1", StringComparison.OrdinalIgnoreCase)
			|| (!jsonNode["ReturnCode"]!.ToString().Equals("402", StringComparison.OrdinalIgnoreCase)
			? throw new ReturnCodeException(jsonNode["ReturnCode"]!.ToString())
			: false);
	}

	public async ValueTask AccessMoneyWithdrawalAsync(
		Uri apiUrl,
		string partnerCode,
		string secretKey,
		string secretIV,
		string memberCode,
		string orderId,
		decimal amount,
		CancellationToken cancellationToken = default)
	{
		var data = $"{memberCode}|2|{orderId}|{amount}";

		var jsonNode = await GetAsync(
			apiUrl: apiUrl,
			partnerCode: partnerCode,
			secretKey: secretKey,
			secretIV: secretIV,
			action: "AccessMoney",
			data: data,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		if (!jsonNode["ReturnCode"]!.ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
		{
			throw new ReturnCodeException(jsonNode["ReturnCode"]!.ToString());
		}
	}

	public async ValueTask<(bool IsPlayerExist, decimal Balance)> GetBalanceAsync(
		Uri apiUrl,
		string partnerCode,
		string secretKey,
		string secretIV,
		string memberCode,
		CancellationToken cancellationToken = default)
	{
		var data = memberCode;

		var jsonNode = await GetAsync(
			apiUrl: apiUrl,
			partnerCode: partnerCode,
			secretKey: secretKey,
			secretIV: secretIV,
			action: "GetBalance",
			data: data,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return !jsonNode["ReturnCode"]!.ToString().Equals("1", StringComparison.OrdinalIgnoreCase)
			? !jsonNode["ReturnCode"]!.ToString().Equals("602", StringComparison.OrdinalIgnoreCase)
				? throw new ReturnCodeException(jsonNode["ReturnCode"]!.ToString())
				: new(false, 0)
			: (true, Convert.ToDecimal(jsonNode["Balance"]!.ToString()));
	}

	public async IAsyncEnumerable<ShowBetInfo> ShowBetListAsync(
		Uri apiUrl,
		string partnerCode,
		string secretKey,
		string secretIV,
		DateTimeOffset startTime,
		DateTimeOffset endTime,
		int pageIndex,
		int pageSize,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var data = $"|{startTime.DateTimeOffsetConvertShortDateString()}|{endTime.DateTimeOffsetConvertShortDateString()}|{pageIndex}|{pageSize}";
		var jsonNode = await GetAsync(
			apiUrl: apiUrl,
			partnerCode: partnerCode,
			secretKey: secretKey,
			secretIV: secretIV,
			action: "ShowBetList",
			data: data,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		if (!jsonNode["ReturnCode"]!.ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
		{
			throw new ReturnCodeException(jsonNode["ReturnCode"]!.ToString());
		}

		var jsonArray = jsonNode["ArrayInvoice"]!.AsArray();

		if (jsonArray.Count == 0)
		{
			yield break;
		}

		foreach (var json in jsonArray)
		{
			if (json is null)
			{
				continue;
			}

			yield return new ShowBetInfo(
				MemberCode: json["memberCode"]!.GetValue<string>(),
				RecordDate: json["recordDate"]!.GetValue<string>(),
				BatchNo: json["batchNo"]!.GetValue<string>(),
				ShowType: json["showType"]!.GetValue<string>(),
				ShowDiscount: json["showDiscount"]!.GetValue<string>(),
				ShowContent: json["showContent"]!.GetValue<string>(),
				ShowResult: json["showResult"]!.GetValue<string>(),
				ShowAmount: json["showAmount"]!.GetValue<string>(),
				ShowWin: json["showWin"]!.GetValue<string>(),
				ShowOddsWin: json["showOddsWin"]!.GetValue<string>(),
				ShowDivideDate: json["showDivideDate"]!.GetValue<string>(),
				ShowRaceDate: json["showRaceDate"]!.GetValue<string>(),
				AdditionalData: json.ToJsonString());
		}
	}

	public async ValueTask<string> LoginAsync(
		Uri apiUrl,
		string partnerCode,
		string token,
		string lang,
		CancellationToken cancellationToken = default)
	{
		var jsonNode = await GetLoginAsync(
			apiUrl: apiUrl,
			partnerCode: partnerCode,
			token: token,
			lang: lang,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return !jsonNode["ReturnCode"]!.ToString().Equals("1", StringComparison.OrdinalIgnoreCase)
			? throw new ReturnCodeException(jsonNode["ReturnCode"]!.ToString())
			: jsonNode["LoginUrl"]!.GetValue<string>();
	}

	public async ValueTask<string> PrepareLoginAsync(
		Uri apiUrl,
		string partnerCode,
		string secretKey,
		string secretIV,
		string memberCode,
		string nickname,
		string currencyCode,
		CancellationToken cancellationToken = default)
	{
		var timestamp = systemClock.CurrentTime.ToUnixTimeSeconds();
		var data = $"{timestamp}|{memberCode}|{nickname}|{currencyCode}";

		var jsonNode = await GetAsync(
			apiUrl: apiUrl,
			partnerCode: partnerCode,
			secretKey: secretKey,
			secretIV: secretIV,
			action: "PrepareLogin",
			data: data,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return !jsonNode["ReturnCode"]!.ToString().Equals("1", StringComparison.OrdinalIgnoreCase)
			? throw new ReturnCodeException(jsonNode["ReturnCode"]!.ToString())
			: jsonNode["Token"]!.ToString();
	}

	public async ValueTask LogoutAsync(
		Uri apiUrl,
		string partnerCode,
		string secretKey,
		string secretIV,
		string memberCode,
		CancellationToken cancellationToken = default)
	{
		var data = memberCode;

		var jsonNode = await GetAsync(
			apiUrl: apiUrl,
			partnerCode: partnerCode,
			secretKey: secretKey,
			secretIV: secretIV,
			action: "Logout",
			data: data,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		if (!jsonNode["ReturnCode"]!.ToString().Equals("1", StringComparison.OrdinalIgnoreCase)
			&&
			!jsonNode["ReturnCode"]!.ToString().Equals("703", StringComparison.OrdinalIgnoreCase))
		{
			throw new ReturnCodeException(jsonNode["ReturnCode"]!.ToString());
		}
	}

	private async ValueTask<JsonNode> GetAsync(
		Uri apiUrl,
		string partnerCode,
		string secretKey,
		string secretIV,
		string action,
		string data,
		CancellationToken cancellationToken = default)
	{
		var hash = await cryptography
			.GetAesEncryptAsync(
			data: data,
			secretKey: secretKey,
			secretIV: secretIV,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		var uriBuilder = new UriBuilder(new Uri(apiUrl, "/index.php"));
		var query = HttpUtility.ParseQueryString(uriBuilder.Query);
		query["m"] = "api";
		query["c"] = "index";
		query["a"] = action;
		query["Hash"] = hash;
		query["PartnerCode"] = partnerCode;
		uriBuilder.Query = query.ToString();

		var logPrefix = $"{nameof(GameApiClient)}.{action}";

		var sw = Stopwatch.StartNew();

		var httpResponseMessage = await client
			.GetAsync(
				requestUri: uriBuilder.ToString(),
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		sw.Stop();

		var responseMessage = default(string);
		using (var sr = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).Result))
		{
			responseMessage = await sr.ReadToEndAsync(cancellationToken: cancellationToken).ConfigureAwait(false) ?? string.Empty;
		}

		var logEvent = new GameApiLogEvent
		{
			Path = httpResponseMessage.RequestMessage?.RequestUri?.AbsoluteUri.NoLongerThan(512) ?? string.Empty,
			Method = httpResponseMessage.RequestMessage?.Method.ToString().NoLongerThan(8) ?? string.Empty,
			Prefix = logPrefix,
			ServerName = Environment.MachineName.NoLongerThan(128),
			RequestHeaders = httpResponseMessage.RequestMessage?.Headers + httpResponseMessage.RequestMessage?.Content?.Headers?.ToString(),
			RequestContent = data.NoLongerThan(4096),
			ResponseHeaders = httpResponseMessage.Headers.ToString(),
			ResponseContent = responseMessage.NoLongerThan(4096),
			ResponseCode = (short)httpResponseMessage.StatusCode,
			ExecutionTimeMs = (int)sw.ElapsedMilliseconds,
			RecordedOn = DateTimeOffset.UtcNow
		};

		logger.LogInformation("{logEvent}", JsonSerializer.Serialize(logEvent));

		_ = httpResponseMessage.EnsureSuccessStatusCode();

		return string.IsNullOrEmpty(responseMessage)
			? throw new Exception("Game provider response content is empty.")
			: JsonNode.Parse(responseMessage)!;
	}

	private async ValueTask<JsonNode> GetLoginAsync(
		Uri apiUrl,
		string partnerCode,
		string token,
		string lang,
		CancellationToken cancellationToken = default)
	{
		var uriBuilder = new UriBuilder(new Uri(apiUrl, "/index.php"));
		var query = HttpUtility.ParseQueryString(uriBuilder.Query);
		query["m"] = "api";
		query["c"] = "index";
		query["a"] = "Login";
		query["Token"] = token;
		query["PartnerCode"] = partnerCode;
		query["Lang"] = lang;
		uriBuilder.Query = query.ToString();

		var logPrefix = $"{nameof(GameApiClient)}.Login";

		var sw = Stopwatch.StartNew();

		var httpResponseMessage = await client
			.GetAsync(
			requestUri: uriBuilder.ToString(),
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		sw.Stop();

		var responseMessage = default(string);
		using (var sr = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).Result))
		{
			responseMessage = await sr
				.ReadToEndAsync(
				cancellationToken: cancellationToken)
				.ConfigureAwait(false)
				?? string.Empty;
		}

		var logEvent = new GameApiLogEvent
		{
			Path = httpResponseMessage.RequestMessage?.RequestUri?.AbsoluteUri.NoLongerThan(512) ?? string.Empty,
			Method = httpResponseMessage.RequestMessage?.Method.ToString().NoLongerThan(8) ?? string.Empty,
			Prefix = logPrefix,
			ServerName = Environment.MachineName.NoLongerThan(128),
			RequestHeaders = httpResponseMessage.RequestMessage?.Headers + httpResponseMessage.RequestMessage?.Content?.Headers?.ToString(),
			RequestContent = string.Empty,
			ResponseHeaders = httpResponseMessage.Headers.ToString(),
			ResponseContent = responseMessage.NoLongerThan(4096),
			ResponseCode = (short)httpResponseMessage.StatusCode,
			ExecutionTimeMs = (int)sw.ElapsedMilliseconds,
			RecordedOn = DateTimeOffset.UtcNow
		};

		logger.LogInformation("{logEvent}", JsonSerializer.Serialize(logEvent));

		_ = httpResponseMessage.EnsureSuccessStatusCode();

		return string.IsNullOrEmpty(responseMessage)
			? throw new Exception("Game provider response content is empty.")
			: JsonNode.Parse(responseMessage)!;
	}
}
