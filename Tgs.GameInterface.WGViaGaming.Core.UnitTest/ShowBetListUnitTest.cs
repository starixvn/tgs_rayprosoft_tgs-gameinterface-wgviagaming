using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json.Nodes;
using System.Web;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.WGViaGaming.Core.Exceptions;
using Tgs.GameInterface.WGViaGaming.Core.Models;

namespace Tgs.GameInterface.WGViaGaming.Core.UnitTest;

public class ShowBetListUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("http://localhost:8088/");
        var partnerCode = "partnerCode";
        var secretKey = "secretKey";
        var secretIV = "secretIV";
        var startTime = DateTimeOffset.Parse("2023-06-27T16:00:00+00:00");
        var endTime = DateTimeOffset.Parse("2023-06-29T16:00:00+00:00");
        var pageIndex = 1;
        var pageSize = 500;

        var fakeCryptography = Substitute.For<IHashCryptography>();
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var hash = "rc8XJTM1r8P3XQ5F7ND4PTqrnxFOV9JKuxqpINIo2WY=";
        var data = $"|2023-06-28|2023-06-30|{pageIndex}|{pageSize}";

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
        query["a"] = "ShowBetList";
        query["Hash"] = hash;
        query["PartnerCode"] = partnerCode;
        uriBuilder.Query = query.ToString();

        var response = new JsonObject
        {
            ["ReturnCode"] = 1,
            ["Page"] = 1,
            ["PageTotal"] = 1,
            ["PageSize"] = 500,
            ["RaceDateFrom"] = "2023-06-28",
            ["RaceDateTo"] = "2023-06-30",
            ["ArrayInvoice"] = new JsonArray
            {
                new JsonObject
                {
                    ["memberCode"]="4ugse0rsnufp",
                    ["recordDate"]="2023-06-28 10:37:29",
                    ["batchNo"]="342797987",
                    ["showType"]="独赢",
                    ["showDiscount"]="早@0.7800<br>",
                    ["showContent"]="HK 1场&nbsp;@&nbsp;2023-06-28 10:40:00<br>@&nbsp;<font color='FireBrick'><b>WIN 1 </b></font>",
                    ["showResult"]="1,6,3,9,5,2,4,7,8",
                    ["showAmount"]= "47",
                    ["showWin"]="290.34",
                    ["showOddsWin"]="6.95",
                    ["showDivideDate"]= "2023-06-28 10:53:55",
                    ["showRaceDate"]="2023-06-28 10:40:00",
                    ["ArrayInvoiceChild"]=new JsonArray{ }
                }
            }
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

        var actual = await client
            .ShowBetListAsync(
            apiUrl: apiUrl,
            partnerCode: partnerCode,
            secretKey: secretKey,
            secretIV: secretIV,
            startTime: startTime,
            endTime: endTime,
            pageIndex: pageIndex,
            pageSize: pageSize)
            .ToArrayAsync()
            .ConfigureAwait(false)
            ?? Enumerable.Empty<ShowBetInfo>();

        Assert.True(actual.Any());
        Assert.Equal(response["ArrayInvoice"]!.AsArray()!.First()!["memberCode"]!.ToString(), actual.First().MemberCode);
        Assert.Equal(response["ArrayInvoice"]!.AsArray()!.First()!["recordDate"]!.ToString(), actual.First().RecordDate);
        Assert.Equal(response["ArrayInvoice"]!.AsArray()!.First()!["batchNo"]!.ToString(), actual.First().BatchNo);
        Assert.Equal(response["ArrayInvoice"]!.AsArray()!.First()!["showType"]!.ToString(), actual.First().ShowType);
        Assert.Equal(response["ArrayInvoice"]!.AsArray()!.First()!["showDiscount"]!.ToString(), actual.First().ShowDiscount);
        Assert.Equal(response["ArrayInvoice"]!.AsArray()!.First()!["showContent"]!.ToString(), actual.First().ShowContent);
        Assert.Equal(response["ArrayInvoice"]!.AsArray()!.First()!["showResult"]!.ToString(), actual.First().ShowResult);
        Assert.Equal(response["ArrayInvoice"]!.AsArray()!.First()!["showAmount"]!.ToString(), actual.First().ShowAmount);
        Assert.Equal(response["ArrayInvoice"]!.AsArray()!.First()!["showWin"]!.ToString(), actual.First().ShowWin);
        Assert.Equal(response["ArrayInvoice"]!.AsArray()!.First()!["showOddsWin"]!.ToString(), actual.First().ShowOddsWin);
        Assert.Equal(response["ArrayInvoice"]!.AsArray()!.First()!["showDivideDate"]!.ToString(), actual.First().ShowDivideDate);
        Assert.Equal(response["ArrayInvoice"]!.AsArray()!.First()!["showRaceDate"]!.ToString(), actual.First().ShowRaceDate);
        Assert.Equal(response["ArrayInvoice"]!.AsArray()!.First()!.ToJsonString(), actual.First().AdditionalData);
    }

    [Fact]
    public async Task PageIndex_Two()
    {
        var apiUrl = new Uri("http://localhost:8088/");
        var partnerCode = "partnerCode";
        var secretKey = "secretKey";
        var secretIV = "secretIV";
        var startTime = DateTimeOffset.Parse("2023-06-27T16:00:00+00:00");
        var endTime = DateTimeOffset.Parse("2023-06-29T16:00:00+00:00");
        var pageIndex = 2;
        var pageSize = 500;

        var fakeCryptography = Substitute.For<IHashCryptography>();
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var hash = "rc8XJTM1r8P3XQ5F7ND4PSbbLwT4aDpKyUcM6ylnpHk=";
        var data = $"|2023-06-28|2023-06-30|{pageIndex}|{pageSize}";

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
        query["a"] = "ShowBetList";
        query["Hash"] = hash;
        query["PartnerCode"] = partnerCode;
        uriBuilder.Query = query.ToString();

        var response = new JsonObject
        {
            ["ReturnCode"] = 1,
            ["Page"] = 2,
            ["PageTotal"] = 1,
            ["PageSize"] = 500,
            ["RaceDateFrom"] = "2023-06-28",
            ["RaceDateTo"] = "2023-06-30",
            ["ArrayInvoice"] = new JsonArray { }
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

        var actual = await client
            .ShowBetListAsync(
            apiUrl: apiUrl,
            partnerCode: partnerCode,
            secretKey: secretKey,
            secretIV: secretIV,
            startTime: startTime,
            endTime: endTime,
            pageIndex: pageIndex,
            pageSize: pageSize)
            .ToArrayAsync()
            .ConfigureAwait(false)
            ?? Enumerable.Empty<ShowBetInfo>();

        Assert.False(actual.Any());
    }

    [Fact]
    public async Task Invalid_Hash()
    {
        var apiUrl = new Uri("http://localhost:8088/");
        var partnerCode = "partnerCode";
        var secretKey = "secretKey";
        var secretIV = "secretIV";
        var startTime = DateTimeOffset.Parse("2023-06-27T16:00:00+00:00");
        var endTime = DateTimeOffset.Parse("2023-06-29T16:00:00+00:00");
        var pageIndex = 1;
        var pageSize = 500;

        var fakeCryptography = Substitute.For<IHashCryptography>();
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var hash = "rc8XJTM1r8P3XQ5F7ND4PTqrnxFOV9JKuxqpINIo2WY=";
        var data = $"|2023-06-28|2023-06-30|{pageIndex}|{pageSize}";

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
        query["a"] = "ShowBetList";
        query["Hash"] = hash;
        query["PartnerCode"] = partnerCode;
        uriBuilder.Query = query.ToString();

        var response = new JsonObject
        {
            ["ReturnCode"] = 501
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
            () => client
            .ShowBetListAsync(
                apiUrl: apiUrl,
                partnerCode: partnerCode,
                secretKey: secretKey,
                secretIV: secretIV,
                startTime: startTime,
                endTime: endTime,
                pageIndex: pageIndex,
                pageSize: pageSize)
            .ToArrayAsync()
            .AsTask())
            .ConfigureAwait(false);
    }
}
