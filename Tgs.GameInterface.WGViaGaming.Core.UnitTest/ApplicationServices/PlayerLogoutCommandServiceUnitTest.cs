using NSubstitute;
using Tgs.GameInterface.WGViaGaming.Core.ApplicationServices;

namespace Tgs.GameInterface.WGViaGaming.Core.UnitTest.ApplicationServices;

public class PlayerLogoutCommandServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var command = new PlayerLogoutCommand(
            ApiUrl: new Uri("http://localhost:8088/"),
            PartnerCode: "partnerCode",
            SecretKey: "secretKey",
            SecretIV: "secretIV",
            PlayerCode: "4ugse0rsnufp");

        var fakeClient = Substitute.For<IGameApiClient>();

        var service = new PlayerLogoutCommandService(
            fakeClient);

        await service.ExecuteAsync(command: command);

        _ = fakeClient
            .Received(1)
            .LogoutAsync(
            apiUrl: Arg.Is(command.ApiUrl),
            partnerCode: Arg.Is(command.PartnerCode),
            secretKey: Arg.Is(command.SecretKey),
            secretIV: Arg.Is(command.SecretIV),
            memberCode: Arg.Is(command.PlayerCode),
            cancellationToken: Arg.Any<CancellationToken>());
    }
}
