using NSubstitute;
using Tgs.GameInterface.WGViaGaming.Core.ApplicationServices;

namespace Tgs.GameInterface.WGViaGaming.Core.UnitTest.ApplicationServices;

public class FundInCommandServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var command = new FundInCommand(
			ApiUrl: new Uri("http://localhost:8088/"),
			PartnerCode: "partnerCode",
			SecretKey: "secretKey",
			SecretIV: "secretIV",
			PlayerCode: "4ugse0rsnufp",
			OrderId: "01893e7e846a0000000107e702147a17",
			Amount: 5056m);

		var fakeClient = Substitute.For<IGameApiClient>();

		var service = new FundInCommandService(
			fakeClient);

		await service.ExecuteAsync(command: command);

		_ = fakeClient
			.Received(1)
			.AccessMoneyWithdrawalAsync(
			 apiUrl: Arg.Is(command.ApiUrl),
			 partnerCode: Arg.Is(command.PartnerCode),
			 secretKey: Arg.Is(command.SecretKey),
			secretIV: Arg.Is(command.SecretIV),
			memberCode: Arg.Is(command.PlayerCode),
			orderId: Arg.Is(command.OrderId),
			amount: Arg.Is(command.Amount),
			cancellationToken: Arg.Any<CancellationToken>());
	}
}
