using System.Runtime.CompilerServices;
using Google.Protobuf.WellKnownTypes;
using Tgs.GameInterface.WGViaGaming.Protos;

namespace Tgs.GameInterface.WGViaGaming.Client;

internal class GameInterfaceWGViaGamingClient : IGameInterfaceWGViaGamingClient
{
    private readonly GameInterfaceWGViaGaming.GameInterfaceWGViaGamingClient _client;

    public GameInterfaceWGViaGamingClient(GameInterfaceWGViaGaming.GameInterfaceWGViaGamingClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<string> CheckingServiceAsync(
        string machineName,
        CancellationToken cancellationToken = default)
    {
        var request = new CheckingServiceRequest
        {
            MachineName = machineName,
        };

        var reply = await _client
            .CheckingServiceAsync(
                request: request,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return reply.MachineName;
    }

    public async ValueTask FundInAsync(
        Uri apiUrl,
        string partnerCode,
        string secretKey,
        string secretIV,
        string playerCode,
        string orderId,
        decimal amount,
        CancellationToken cancellationToken = default)
        => _ = await _client
        .FundInAsync(
            request: new FundRequest
            {
                ApiUrl = apiUrl.ToString(),
                PartnerCode = partnerCode,
                SecretKey = secretKey,
                SecretIV = secretIV,
                PlayerCode = playerCode,
                OrderId = orderId,
                Amount = amount
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask FundOutAsync(
        Uri apiUrl,
        string partnerCode,
        string secretKey,
        string secretIV,
        string playerCode,
        string orderId,
        decimal amount,
        CancellationToken cancellationToken = default)
        => _ = await _client
        .FundOutAsync(
            request: new FundRequest
            {
                ApiUrl = apiUrl.ToString(),
                PartnerCode = partnerCode,
                SecretKey = secretKey,
                SecretIV = secretIV,
                PlayerCode = playerCode,
                OrderId = orderId,
                Amount = amount
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask<bool> FundStatusAsync(
        Uri apiUrl,
        string partnerCode,
        string secretKey,
        string secretIV,
        string orderId,
        CancellationToken cancellationToken = default)
    {
        var reply = await _client
        .FundStatusAsync(
            request: new FundStatusRequest
            {
                ApiUrl = apiUrl.ToString(),
                PartnerCode = partnerCode,
                SecretKey = secretKey,
                SecretIV = secretIV,
                OrderId = orderId
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

        return reply.State;
    }

    public async IAsyncEnumerable<GameRecordInfo> GameRecordAsync(
        Uri apiUrl,
        string partnerCode,
        string secretKey,
        string secretIV,
        DateTimeOffset startTime,
        DateTimeOffset endTime,
        int pageIndex = 1,
        int pageSize = 1000,
       [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var reply = await _client
        .GameRecordAsync(
            request: new GameRecordRequest
            {
                ApiUrl = apiUrl.ToString(),
                PartnerCode = partnerCode,
                SecretKey = secretKey,
                SecretIV = secretIV,
                StartTime = startTime.ToTimestamp(),
                EndTime = endTime.ToTimestamp(),
                PageIndex = pageIndex,
                PageSize = pageSize
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

        var gameRecords = reply.Items
            ?? Enumerable.Empty<GameRecordInfo>();

        if (!gameRecords.Any())
        {
            yield break;
        }

        foreach (var gameRecord in gameRecords)
        {
            yield return gameRecord;
        }
    }

    public async ValueTask<string> GameUrlAsync(
        Uri apiUrl,
        string partnerCode,
        string secretKey,
        string secretIV,
        string playerCode,
        string currencyCode,
        string lang,
        CancellationToken cancellationToken = default)
    {
        var reply = await _client
             .GameUrlAsync(
            request: new GameUrlRequest
            {
                ApiUrl = apiUrl.ToString(),
                PartnerCode = partnerCode,
                SecretKey = secretKey,
                SecretIV = secretIV,
                PlayerCode = playerCode,
                CurrencyCode = currencyCode,
                Lang = lang
            },
             cancellationToken: cancellationToken)
         .ConfigureAwait(false);

        return reply.GameUrl.ToString();
    }

    public async ValueTask<PlayerBalanceReply> PlayerBalanceAsync(
        Uri apiUrl,
        string partnerCode,
        string secretKey,
        string secretIV,
        string playerCode,
        CancellationToken cancellationToken = default)
        => await _client
        .PlayerBalanceAsync(
            request: new PlayerBalanceRequest
            {
                ApiUrl = apiUrl.ToString(),
                PartnerCode = partnerCode,
                SecretKey = secretKey,
                SecretIV = secretIV,
                PlayerCode = playerCode
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask PlayerCreateAsync(
        Uri apiUrl,
        string partnerCode,
        string secretKey,
        string secretIV,
        string playerCode,
        string currencyCode,
        CancellationToken cancellationToken = default)
        => _ = await _client
        .PlayerCreateAsync(
            request: new PlayerCreateRequest
            {
                ApiUrl = apiUrl.ToString(),
                PartnerCode = partnerCode,
                SecretKey = secretKey,
                SecretIV = secretIV,
                PlayerCode = playerCode,
                CurrencyCode = currencyCode,
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask PlayerLogoutAsync(
        Uri apiUrl,
        string partnerCode,
        string secretKey,
        string secretIV,
        string playerCode,
        CancellationToken cancellationToken = default)
        => _ = await _client
        .PlayerLogoutAsync(
            request: new PlayerLogoutRequest
            {
                ApiUrl = apiUrl.ToString(),
                PartnerCode = partnerCode,
                SecretKey = secretKey,
                SecretIV = secretIV,
                PlayerCode = playerCode
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);
}
