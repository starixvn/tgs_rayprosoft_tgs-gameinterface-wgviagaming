using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WGViaGaming.Core.ApplicationServices;

internal class FundOutCommandService : ICommandService<FundOutCommand>
{
    private readonly IGameApiClient _client;

    public FundOutCommandService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask ExecuteAsync(
        FundOutCommand command,
        CancellationToken cancellationToken = default)
        => _client
        .AccessMoneyDepositAsync(
            apiUrl: command.ApiUrl,
            partnerCode: command.PartnerCode,
            secretKey: command.SecretKey,
            secretIV: command.SecretIV,
            memberCode: command.PlayerCode,
            orderId: command.OrderId,
            amount: command.Amount,
            cancellationToken: cancellationToken);
}
