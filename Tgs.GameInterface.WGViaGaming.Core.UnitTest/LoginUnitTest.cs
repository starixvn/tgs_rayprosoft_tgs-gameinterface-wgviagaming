using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json.Nodes;
using System.Web;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.WGViaGaming.Core.Exceptions;

namespace Tgs.GameInterface.WGViaGaming.Core.UnitTest;

public class LoginUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("http://localhost:8088/");
		var partnerCode = "partnerCode";
		var token = "idMmn7HaZyfb9pNGl6QtEtxk2blZK+fL0t221isVL5U=";
		var lang = "1";

		var fakeCryptography = Substitute.For<IHashCryptography>();
		var fakeSystemClock = Substitute.For<ISystemClock>();

		var uriBuilder = new UriBuilder(new Uri(apiUrl, "/index.php"));
		var query = HttpUtility.ParseQueryString(uriBuilder.Query);
		query["m"] = "api";
		query["c"] = "index";
		query["a"] = "Login";
		query["Token"] = token;
		query["PartnerCode"] = partnerCode;
		query["Lang"] = lang;
		uriBuilder.Query = query.ToString();

		var response = new JsonObject
		{
			["ReturnCode"] = 1,
			["LoginUrl"] = "http:\\/\\/via.itd888.com\\/index.php?m=Homeweb&c=Login&a=login&is_api=1&username=PT%234ugse0rsnufp&session_id=132af27c5548b292dc&l=zh-tw"
		};

		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Get, uriBuilder.ToString())
			.Respond(HttpStatusCode.OK, new StringContent(response.ToJsonString(), Encoding.UTF8, MediaTypeNames.Application.Json));

		var fakeHttpClient = handler.ToHttpClient();
		var fakeLog = NullLogger<GameApiClient>.Instance;

		var client = new GameApiClient(
			fakeLog,
			fakeCryptography,
			fakeSystemClock,
			fakeHttpClient);

		var actual = await client.LoginAsync(
			apiUrl: apiUrl,
			partnerCode: partnerCode,
			token: token,
			lang: lang);

		Assert.Equal(response["LoginUrl"]!.ToString(), actual);
	}

	[Fact]
	public async Task Token_Decryption_Exception()
	{
		var apiUrl = new Uri("http://localhost:8088/");
		var partnerCode = "partnerCode";
		var token = "idMmn7HaZyfb9pNGl6QtEtxk2blZK+fL0t221isVL5U=";
		var lang = "1";

		var fakeCryptography = Substitute.For<IHashCryptography>();
		var fakeSystemClock = Substitute.For<ISystemClock>();

		var uriBuilder = new UriBuilder(new Uri(apiUrl, "/index.php"));
		var query = HttpUtility.ParseQueryString(uriBuilder.Query);
		query["m"] = "api";
		query["c"] = "index";
		query["a"] = "Login";
		query["Token"] = token;
		query["PartnerCode"] = partnerCode;
		query["Lang"] = lang;
		uriBuilder.Query = query.ToString();

		var response = new JsonObject
		{
			["ReturnCode"] = 201
		};

		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Get, uriBuilder.ToString())
			.Respond(HttpStatusCode.OK, new StringContent(response.ToJsonString(), Encoding.UTF8, MediaTypeNames.Application.Json));

		var fakeHttpClient = handler.ToHttpClient();
		var fakeLog = NullLogger<GameApiClient>.Instance;

		var client = new GameApiClient(
			fakeLog,
			fakeCryptography,
			fakeSystemClock,
			fakeHttpClient);

		_ = await Assert.ThrowsAsync<ReturnCodeException>(
			() => client.LoginAsync(
				apiUrl: apiUrl,
				partnerCode: partnerCode,
				token: token,
				lang: lang)
			.AsTask());
	}
}
