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

public class AccessMoneyQueryUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("http://localhost:8088/");
        var partnerCode = "partnerCode";
        var secretKey = "secretKey";
        var secretIV = "secretIV";
        var orderId = "01893e7e846a0000000107e702147a17";

        var fakeCryptography = Substitute.For<IHashCryptography>();
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var hash = "EbZunjq4QYMIDZBVXS5h6HnDiq08sOkVEscrt0RrZAWWJH+jfbE5vZyDbvNORGsn";
        var data = $"{orderId}";

        _ = fakeCryptography.GetAesEncryptAsync(
            data: Arg.Is(data),
            secretKey: Arg.Is(secretKey),
            secretIV: Arg.Is(secretIV),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(hash);

        var uriBuilder = new UriBuilder(new Uri(apiUrl, "/index.php"));
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["m"] = "api";
        query["c"] = "index";
        query["a"] = "AccessMoneyQuery";
        query["Hash"] = hash;
        query["PartnerCode"] = partnerCode;
        uriBuilder.Query = query.ToString();

        var response = new JsonObject
        {
            ["ReturnCode"] = 1
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

        var actual = await client.AccessMoneyQueryAsync(
            apiUrl: apiUrl,
            partnerCode: partnerCode,
            secretKey: secretKey,
            secretIV: secretIV,
            orderId: orderId);

        Assert.True(actual);
    }

    [Fact]
    public async Task Order_Not_Valid()
    {
        var apiUrl = new Uri("http://localhost:8088/");
        var partnerCode = "partnerCode";
        var secretKey = "secretKey";
        var secretIV = "secretIV";
        var orderId = "01893e7e846a0000000107e702147a18";

        var fakeCryptography = Substitute.For<IHashCryptography>();
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var hash = "EbZunjq4QYMIDZBVXS5h6AzI0eJ/hhR57l5VvUMgdpHjZVXxv9MkuSUsmIBjzKY6";
        var data = $"{orderId}";

        _ = fakeCryptography.GetAesEncryptAsync(
            data: Arg.Is(data),
            secretKey: Arg.Is(secretKey),
            secretIV: Arg.Is(secretIV),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(hash);

        var uriBuilder = new UriBuilder(new Uri(apiUrl, "/index.php"));
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["m"] = "api";
        query["c"] = "index";
        query["a"] = "AccessMoneyQuery";
        query["Hash"] = hash;
        query["PartnerCode"] = partnerCode;
        uriBuilder.Query = query.ToString();

        var response = new JsonObject
        {
            ["ReturnCode"] = 402
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

        var actual = await client.AccessMoneyQueryAsync(
            apiUrl: apiUrl,
            partnerCode: partnerCode,
            secretKey: secretKey,
            secretIV: secretIV,
            orderId: orderId);

        Assert.False(actual);
    }

    [Fact]
    public async Task Invalid_Hash()
    {
        var apiUrl = new Uri("http://localhost:8088/");
        var partnerCode = "partnerCode";
        var secretKey = "secretKey";
        var secretIV = "secretIV";
        var orderId = "01893e7e846a0000000107e702147a18";

        var fakeCryptography = Substitute.For<IHashCryptography>();
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var hash = "EbZunjq4QYMIDZBVXS5h6AzI0eJ/hhR57l5VvUMgdpHjZVXxv9MkuSUsmIBjzKY6";
        var data = $"{orderId}";

        _ = fakeCryptography.GetAesEncryptAsync(
            data: Arg.Is(data),
            secretKey: Arg.Is(secretKey),
            secretIV: Arg.Is(secretIV),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(hash);

        var uriBuilder = new UriBuilder(new Uri(apiUrl, "/index.php"));
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["m"] = "api";
        query["c"] = "index";
        query["a"] = "AccessMoneyQuery";
        query["Hash"] = hash;
        query["PartnerCode"] = partnerCode;
        uriBuilder.Query = query.ToString();

        var response = new JsonObject
        {
            ["ReturnCode"] = 401
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
            () => client.AccessMoneyQueryAsync(
                apiUrl: apiUrl,
                partnerCode: partnerCode,
                secretKey: secretKey,
                secretIV: secretIV,
                orderId: orderId)
            .AsTask());
    }
}
