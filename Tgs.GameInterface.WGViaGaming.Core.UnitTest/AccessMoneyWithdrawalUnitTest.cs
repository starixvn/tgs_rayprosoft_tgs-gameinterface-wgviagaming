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

public class AccessMoneyWithdrawalUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("http://localhost:8088/");
        var partnerCode = "partnerCode";
        var secretKey = "secretKey";
        var secretIV = "secretIV";
        var memberCode = "4ugse0rsnufp";
        var orderId = "01893e7e846a0000000107e702147a17";
        var amount = 5056m;

        var fakeCryptography = Substitute.For<IHashCryptography>();
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var hash = "yFgVIVRL+BCh5lPx5d/t4U5xPjZ7u/COHqN+VfOphCHtVeR45OFBKSCDxHr7VGRb9OtuLrW9DyEdymuNULfriQ==";
        var data = $"{memberCode}|2|{orderId}|{amount}";

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
        query["a"] = "AccessMoney";
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

        await client.AccessMoneyWithdrawalAsync(
            apiUrl: apiUrl,
            partnerCode: partnerCode,
            secretKey: secretKey,
            secretIV: secretIV,
            memberCode: memberCode,
            orderId: orderId,
            amount: amount);
    }

    [Fact]
    public async Task Invalid_User()
    {
        var apiUrl = new Uri("http://localhost:8088/");
        var partnerCode = "partnerCode";
        var secretKey = "secretKey";
        var secretIV = "secretIV";
        var memberCode = "4ugse0rsnufp";
        var orderId = "01893e7e846a0000000107e702147a17";
        var amount = 5056m;

        var fakeCryptography = Substitute.For<IHashCryptography>();
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var hash = "yFgVIVRL+BCh5lPx5d/t4U5xPjZ7u/COHqN+VfOphCHtVeR45OFBKSCDxHr7VGRb9OtuLrW9DyEdymuNULfriQ==";
        var data = $"{memberCode}|2|{orderId}|{amount}";

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
        query["a"] = "AccessMoney";
        query["Hash"] = hash;
        query["PartnerCode"] = partnerCode;
        uriBuilder.Query = query.ToString();

        var response = new JsonObject
        {
            ["ReturnCode"] = 302
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
            () => client.AccessMoneyWithdrawalAsync(
                apiUrl: apiUrl,
                partnerCode: partnerCode,
                secretKey: secretKey,
                secretIV: secretIV,
                memberCode: memberCode,
                orderId: orderId,
                amount: amount)
            .AsTask());
    }

    [Fact]
    public async Task Invalid_Hash()
    {
        var apiUrl = new Uri("http://localhost:8088/");
        var partnerCode = "partnerCode";
        var secretKey = "secretKey";
        var secretIV = "secretIV";
        var memberCode = "4ugse0rsnufp";
        var orderId = "01893e7e846a0000000107e702147a17";
        var amount = 5056m;

        var fakeCryptography = Substitute.For<IHashCryptography>();
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var hash = "yFgVIVRL+BCh5lPx5d/t4U5xPjZ7u/COHqN+VfOphCHtVeR45OFBKSCDxHr7VGRb9OtuLrW9DyEdymuNULfriQ==";
        var data = $"{memberCode}|2|{orderId}|{amount}";

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
        query["a"] = "AccessMoney";
        query["Hash"] = hash;
        query["PartnerCode"] = partnerCode;
        uriBuilder.Query = query.ToString();

        var response = new JsonObject
        {
            ["ReturnCode"] = 301
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
            () => client.AccessMoneyWithdrawalAsync(
                apiUrl: apiUrl,
                partnerCode: partnerCode,
                secretKey: secretKey,
                secretIV: secretIV,
                memberCode: memberCode,
                orderId: orderId,
                amount: amount)
            .AsTask());
    }
}
