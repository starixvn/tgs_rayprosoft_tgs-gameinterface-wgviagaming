using NSubstitute;
using Tgs.GameInterface.WGViaGaming.Core.ApplicationServices;

namespace Tgs.GameInterface.WGViaGaming.Core.UnitTest.ApplicationServices;

public class FundStatusGetServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var query = new FundStatusQuery(
			ApiUrl: new Uri("http://localhost:8088/"),
			PartnerCode: "partnerCode",
			SecretKey: "secretKey",
			SecretIV: "secretIV",
			OrderId: "01893e7e846a0000000107e702147a17");

		var fakeClient = Substitute.For<IGameApiClient>();

		var service = new FundStatusGetService(
			fakeClient);

		_ = await service.GetAsync(query: query);

		_ = fakeClient
			.Received(1)
			.AccessMoneyQueryAsync(
			 apiUrl: Arg.Is(query.ApiUrl),
			 partnerCode: Arg.Is(query.PartnerCode),
			 secretKey: Arg.Is(query.SecretKey),
			 secretIV: Arg.Is(query.SecretIV),
			 orderId: Arg.Is(query.OrderId),
			 cancellationToken: Arg.Any<CancellationToken>());
	}
}
