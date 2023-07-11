using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WGViaGaming.Core.ApplicationServices;

internal class FundInCommandService : ICommandService<FundInCommand>
{
    private readonly IGameApiClient _client;

    public FundInCommandService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask ExecuteAsync(
        FundInCommand command,
        CancellationToken cancellationToken = default)
        => _client
        .AccessMoneyWithdrawalAsync(
            apiUrl: command.ApiUrl,
            partnerCode: command.PartnerCode,
            secretKey: command.SecretKey,
            secretIV: command.SecretIV,
            memberCode: command.PlayerCode,
            orderId: command.OrderId,
            amount: command.Amount,
            cancellationToken: cancellationToken);
}
