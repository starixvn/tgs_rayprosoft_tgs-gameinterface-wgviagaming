using System.Runtime.CompilerServices;
using Tgs.GameInterface.WGViaGaming.Core.Extensions;
using Tgs.GameInterface.WGViaGaming.Core.Models;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WGViaGaming.Core.ApplicationServices;

internal class GameRecordQueryService : IQueryService<GameRecordQuery, GameRecordResult>
{
    private readonly IGameApiClient _client;
    private readonly ISystemClock _systemClock;

    public GameRecordQueryService(
        IGameApiClient client,
        ISystemClock systemClock)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _systemClock = systemClock ?? throw new ArgumentNullException(nameof(systemClock));
    }

    public async IAsyncEnumerable<GameRecordResult> QueryAsync(
        GameRecordQuery query,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var showBetInfos = await _client
            .ShowBetListAsync(
            apiUrl: query.ApiUrl,
            partnerCode: query.PartnerCode,
            secretKey: query.SecretKey,
            secretIV: query.SecretIV,
            startTime: query.StartTime,
            endTime: query.EndTime,
            pageIndex: query.PageIndex,
            pageSize: query.PageSize,
            cancellationToken: cancellationToken)
            .ToArrayAsync(
            cancellationToken: cancellationToken)
            .ConfigureAwait(false)
            ?? Enumerable.Empty<ShowBetInfo>();

        if (!showBetInfos.Any())
        {
            yield break;
        }

        foreach (var showBetInfo in showBetInfos)
        {
            var isDraw = showBetInfo.ShowWin.Contains("和局", StringComparison.OrdinalIgnoreCase);
            var isClosed = true;
            var externalCreatedOn = showBetInfo.RecordDate.StringConvertDateTimeOffset();
            var externalClosedOn = showBetInfo.ShowDivideDate.StringConvertDateTimeOffset();
            var bet = decimal.Parse(showBetInfo.ShowAmount);
            var winLoss = 0m;
            var settle = 0m;
            if (showBetInfo.ShowType.Contains("過關", StringComparison.OrdinalIgnoreCase) ||
                showBetInfo.ShowType.Contains("过关", StringComparison.OrdinalIgnoreCase))
            {
                var tomorrowClosedOn = externalClosedOn.AddDays(+1).Date.ToShortDateString().StringConvertDateTimeOffset();
                if (externalClosedOn.CompareTo(tomorrowClosedOn) != -1 ||
                    _systemClock.CurrentTime.CompareTo(tomorrowClosedOn) is -1
                    )
                {
                    isClosed = false;
                    externalClosedOn = DateTimeOffset.UnixEpoch;
                }
            }

            if (isClosed)
            {
                winLoss = isDraw ? 0 : decimal.Parse(showBetInfo.ShowWin);
                settle = bet + winLoss;
            }

            yield return new GameRecordResult(
                ExternalRoundId: showBetInfo.BatchNo,
                PlayerCode: showBetInfo.MemberCode,
                IsClosed: isClosed,
                IsDraw: isDraw,
                ExternalCreatedOn: externalCreatedOn,
                ExternalClosedOn: externalClosedOn,
                Bet: bet,
                WinLoss: winLoss,
                Settle: settle,
                AdditionalData: showBetInfo.AdditionalData);
        }
    }
}
