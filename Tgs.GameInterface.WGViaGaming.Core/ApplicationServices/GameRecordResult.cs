namespace Tgs.GameInterface.WGViaGaming.Core.ApplicationServices;

public record GameRecordResult(
    string ExternalRoundId,
    string PlayerCode,
    bool IsClosed,
    bool IsDraw,
    DateTimeOffset ExternalCreatedOn,
    DateTimeOffset ExternalClosedOn,
    decimal Bet,
    decimal Settle,
    decimal WinLoss,
    string AdditionalData);
