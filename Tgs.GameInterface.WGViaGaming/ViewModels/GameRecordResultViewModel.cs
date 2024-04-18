namespace Tgs.GameInterface.WGViaGaming.ViewModels;

public record GameRecordResultViewModel(
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
