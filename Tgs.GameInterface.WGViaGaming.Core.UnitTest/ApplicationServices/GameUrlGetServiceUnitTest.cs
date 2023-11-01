using NSubstitute;
using Tgs.GameInterface.WGViaGaming.Core.ApplicationServices;

namespace Tgs.GameInterface.WGViaGaming.Core.UnitTest.ApplicationServices;

public class GameUrlGetServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var query = new GameUrlQuery(
            ApiUrl: new Uri("http://localhost:8088/"),
            PartnerCode: "partnerCode",
            SecretKey: "secretKey",
            SecretIV: "secretIV",
            PlayerCode: "4ugse0rsnufp",
            CurrencyCode: "RMB",
            Lang: "1");

        var fakeClient = Substitute.For<IGameApiClient>();

        var token = Guid.NewGuid().ToString("N");
        _ = fakeClient
            .PrepareLoginAsync(
            apiUrl: Arg.Is(query.ApiUrl),
            partnerCode: Arg.Is(query.PartnerCode),
            secretKey: Arg.Is(query.SecretKey),
            secretIV: Arg.Is(query.SecretIV),
            memberCode: Arg.Is(query.PlayerCode),
            nickname: Arg.Is(query.PlayerCode),
            currencyCode: Arg.Is(query.CurrencyCode),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(token);

        var service = new GameUrlGetService(
            fakeClient);

        _ = await service.GetAsync(query: query);

        _ = fakeClient
            .Received(1)
            .LoginAsync(
             apiUrl: Arg.Is(query.ApiUrl),
             partnerCode: Arg.Is(query.PartnerCode),
             token: Arg.Is(token),
             lang: Arg.Is(query.Lang),
             cancellationToken: Arg.Any<CancellationToken>());
    }
}
