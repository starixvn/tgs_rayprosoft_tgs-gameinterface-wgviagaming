using NSubstitute;
using Tgs.GameInterface.WGViaGaming.Core.ApplicationServices;

namespace Tgs.GameInterface.WGViaGaming.Core.UnitTest.ApplicationServices;

public class PlayerBalanceGetServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var query = new PlayerBalanceQuery(
			ApiUrl: new Uri("http://localhost:8088/"),
			PartnerCode: "partnerCode",
			SecretKey: "secretKey",
			SecretIV: "secretIV",
			PlayerCode: "4ugse0rsnufp");

		var fakeClient = Substitute.For<IGameApiClient>();

		var service = new PlayerBalanceGetService(
			fakeClient);

		_ = await service.GetAsync(query: query);

		_ = fakeClient
			.Received(1)
			.GetBalanceAsync(
			 apiUrl: Arg.Is(query.ApiUrl),
			 partnerCode: Arg.Is(query.PartnerCode),
			 secretKey: Arg.Is(query.SecretKey),
			 secretIV: Arg.Is(query.SecretIV),
			 memberCode: Arg.Is(query.PlayerCode),
			 cancellationToken: Arg.Any<CancellationToken>());
	}
}
