using NSubstitute;
using Tgs.GameInterface.WGViaGaming.Core.ApplicationServices;

namespace Tgs.GameInterface.WGViaGaming.Core.UnitTest.ApplicationServices;

public class PlayerCreateCommandServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var command = new PlayerCreateCommand(
			ApiUrl: new Uri("http://localhost:8088/"),
			PartnerCode: "partnerCode",
			SecretKey: "secretKey",
			SecretIV: "secretIV",
			PlayerCode: "4ugse0rsnufp",
			CurrencyCode: "RMB");

		var fakeClient = Substitute.For<IGameApiClient>();

		var service = new PlayerCreateCommandService(
			fakeClient);

		await service.ExecuteAsync(command: command);

		_ = fakeClient
			.Received(1)
			.PrepareLoginAsync(
			apiUrl: Arg.Is(command.ApiUrl),
			partnerCode: Arg.Is(command.PartnerCode),
			secretKey: Arg.Is(command.SecretKey),
			secretIV: Arg.Is(command.SecretIV),
			memberCode: Arg.Is(command.PlayerCode),
			nickname: Arg.Is(command.PlayerCode),
			currencyCode: Arg.Is(command.CurrencyCode),
			cancellationToken: Arg.Any<CancellationToken>());
	}
}
