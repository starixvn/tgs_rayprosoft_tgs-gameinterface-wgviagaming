namespace Tgs.GameInterface.WGViaGaming.Core.ApplicationServices;

public record PlayerBalanceResult(
    bool IsPlayerExist,
    decimal Balance);
