namespace Tgs.GameInterface.WGViaGaming.Core.UnitTest;

public class HashCryptographyUnitTest
{
	[Fact]
	public async void GetBalance()
	{
		var cryptography = new HashCryptography();
		string secretKey = "a888888888888888";
		string secretIV = "b888888888888888";

		var hash = await cryptography.GetAesEncryptAsync(
			data: "4ugse0rsnufp",
			secretKey: secretKey,
			secretIV: secretIV);

		Assert.Equal("LPyNrrYzpDLHBShlTW59Ow==", hash);
	}

	[Fact]
	public async void AccessMoneyDeposit()
	{
		var cryptography = new HashCryptography();
		string secretKey = "a888888888888888";
		string secretIV = "b888888888888888";

		var hash = await cryptography.GetAesEncryptAsync(
			data: "4ugse0rsnufp|1|01893e7b4c3b0000000107e702147a17|1",
			secretKey: secretKey,
			secretIV: secretIV);

		Assert.Equal("4wFv5KpsBzYP28xObdUNBUdYrgDeg8RH91rws7nKUnIzwcGjk0eTDqajaUv+m2/52wM/7AIJqXsUQlVcfC3f6Q==", hash);
	}

	[Fact]
	public async void AccessMoneyWithdrawal()
	{
		var cryptography = new HashCryptography();
		string secretKey = "a888888888888888";
		string secretIV = "b888888888888888";

		var hash = await cryptography.GetAesEncryptAsync(
			data: "4ugse0rsnufp|2|01893e7e846a0000000107e702147a17|5056",
			secretKey: secretKey,
			secretIV: secretIV);

		Assert.Equal("yFgVIVRL+BCh5lPx5d/t4U5xPjZ7u/COHqN+VfOphCHtVeR45OFBKSCDxHr7VGRb9OtuLrW9DyEdymuNULfriQ==", hash);
	}

	[Fact]
	public async void PrepareLogin()
	{
		var cryptography = new HashCryptography();
		string secretKey = "a888888888888888";
		string secretIV = "b888888888888888";

		var hash = await cryptography.GetAesEncryptAsync(
			data: "1688970737|4ugse0rsnufp|4ugse0rsnufp|RMB",
			secretKey: secretKey,
			secretIV: secretIV);

		Assert.Equal("XM8raolODyQnbr0aJQ5T+iVyHBPsmFFyxEJz3MelDddhr8PCaM/Jtzu0GlEUO5Ia", hash);
	}
}
