using NSubstitute;
using Tgs.GameInterface.WGViaGaming.Core.ApplicationServices;
using Tgs.GameInterface.WGViaGaming.Core.Models;

namespace Tgs.GameInterface.WGViaGaming.Core.UnitTest.ApplicationServices;

public class GameRecordQueryServiceUnitTest
{
    [Fact]
    public async Task Parlay_lang_zh_chs_Is_Close()
    {
        var query = new GameRecordQuery(
            ApiUrl: new Uri("http://localhost:8088/"),
            PartnerCode: "partnerCode",
            SecretKey: "secretKey",
            SecretIV: "secretIV",
            StartTime: DateTimeOffset.Parse("2022-04-20T00:00:00+08:00"),
            EndTime: DateTimeOffset.Parse("2022-04-21T00:00:00+08:00"),
            PageIndex: 1,
            PageSize: 1000);

        var fakeClient = Substitute.For<IGameApiClient>();
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var showBetInfos = new ShowBetInfo[]
        {
            new ShowBetInfo(
                MemberCode: "4ued5l50heuf",
                RecordDate: "2022-04-20 14:11:54",
                BatchNo: "819776544",
                ShowType: "过关<br><font color='red'>2x1</font>",
                ShowDiscount: "冇单头@0.8200",
                ShowContent: "&nbsp;HK  1场&nbsp;@&nbsp;2022-03-30 18:43:00<br>&nbsp;@&nbsp;<font color='FireBrick'><b>WIN</b></font>&nbsp;&nbsp;<font color='FireBrick'><b>1+2+3</b></font>&nbsp;&nbsp;<br>&nbsp;HK  3场&nbsp;@&nbsp;2022-03-30 18:43:00<br>&nbsp;@&nbsp;<font color='FireBrick'><b>WIN</b></font>&nbsp;&nbsp;<font color='FireBrick'><b>1+2</b></font>&nbsp;&nbsp;<br>&nbsp;每注金额:10<Br>",
                ShowResult: "<font font-family: Arial,Verdana;>1,5,7</font><br><font font-family: Arial,Verdana;>4,3,8</font><br>",
                ShowAmount: "60",
                ShowWin: "-49.20",
                ShowOddsWin: "0",
                ShowDivideDate: "2022-04-20 14:23:29",
                ShowRaceDate: "2022-03-30 18:43:00",
                AdditionalData: string.Empty  )
        };
        _ = fakeClient
            .ShowBetListAsync(
            apiUrl: Arg.Is(query.ApiUrl),
            partnerCode: Arg.Is(query.PartnerCode),
            secretKey: Arg.Is(query.SecretKey),
            secretIV: Arg.Is(query.SecretIV),
            startTime: Arg.Is(query.StartTime),
            endTime: Arg.Is(query.EndTime),
            pageIndex: Arg.Is(query.PageIndex),
            pageSize: Arg.Is(query.PageSize),
            cancellationToken: Arg.Any<CancellationToken>())
           .Returns(showBetInfos.ToAsyncEnumerable());

        _ = fakeSystemClock.CurrentTime.Returns(returnThis: DateTimeOffset.Parse("2022-04-21T00:00:00+08:00"));

        var service = new GameRecordQueryService(
            fakeClient,
            fakeSystemClock);

        var actual = await service
            .QueryAsync(
            query: query)
            .ToArrayAsync()
            .ConfigureAwait(false)
            ?? Enumerable.Empty<GameRecordResult>();

        Assert.True(actual.Any());
        Assert.Equal("4ued5l50heuf", actual.First().PlayerCode);
        Assert.Equal("819776544", actual.First().ExternalRoundId);
        Assert.True(actual.First().IsClosed);
        Assert.False(actual.First().IsDraw);
        Assert.Equal(DateTimeOffset.Parse("2022-04-20T14:11:54+08:00"), actual.First().ExternalCreatedOn);
        Assert.Equal(DateTimeOffset.Parse("2022-04-20T14:23:29+08:00"), actual.First().ExternalClosedOn);
        Assert.Equal(60, actual.First().Bet);
        Assert.Equal(-49.20m, actual.First().WinLoss);
        Assert.Equal(10.8m, actual.First().Settle);
        Assert.Equal(string.Empty, actual.First().AdditionalData);
    }

    [Fact]
    public async Task Parlay_lang_zh_chs_Is_Open()
    {
        var query = new GameRecordQuery(
            ApiUrl: new Uri("http://localhost:8088/"),
            PartnerCode: "partnerCode",
            SecretKey: "secretKey",
            SecretIV: "secretIV",
            StartTime: DateTimeOffset.Parse("2022-04-20T00:00:00+08:00"),
            EndTime: DateTimeOffset.Parse("2022-04-21T00:00:00+08:00"),
            PageIndex: 1,
            PageSize: 1000);

        var fakeClient = Substitute.For<IGameApiClient>();
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var showBetInfos = new ShowBetInfo[]
        {
            new ShowBetInfo(
                MemberCode: "4ued5l50heuf",
                RecordDate: "2022-04-20 14:11:54",
                BatchNo: "819776544",
                ShowType: "过关<br><font color='red'>2x1</font>",
                ShowDiscount: "冇单头@0.8200",
                ShowContent: "&nbsp;HK  1场&nbsp;@&nbsp;2022-03-30 18:43:00<br>&nbsp;@&nbsp;<font color='FireBrick'><b>WIN</b></font>&nbsp;&nbsp;<font color='FireBrick'><b>1+2+3</b></font>&nbsp;&nbsp;<br>&nbsp;HK  3场&nbsp;@&nbsp;2022-03-30 18:43:00<br>&nbsp;@&nbsp;<font color='FireBrick'><b>WIN</b></font>&nbsp;&nbsp;<font color='FireBrick'><b>1+2</b></font>&nbsp;&nbsp;<br>&nbsp;每注金额:10<Br>",
                ShowResult: "<font font-family: Arial,Verdana;>1,5,7</font><br><font font-family: Arial,Verdana;>4,3,8</font><br>",
                ShowAmount: "60",
                ShowWin: "-49.20",
                ShowOddsWin: "0",
                ShowDivideDate: "2022-04-20 14:23:29",
                ShowRaceDate: "2022-03-30 18:43:00",
                AdditionalData: string.Empty  )
        };
        _ = fakeClient
            .ShowBetListAsync(
            apiUrl: Arg.Is(query.ApiUrl),
            partnerCode: Arg.Is(query.PartnerCode),
            secretKey: Arg.Is(query.SecretKey),
            secretIV: Arg.Is(query.SecretIV),
            startTime: Arg.Is(query.StartTime),
            endTime: Arg.Is(query.EndTime),
            pageIndex: Arg.Is(query.PageIndex),
            pageSize: Arg.Is(query.PageSize),
            cancellationToken: Arg.Any<CancellationToken>())
           .Returns(showBetInfos.ToAsyncEnumerable());

        _ = fakeSystemClock.CurrentTime.Returns(returnThis: DateTimeOffset.Parse("2022-04-20T14:24:00+08:00"));

        var service = new GameRecordQueryService(
            fakeClient,
            fakeSystemClock);

        var actual = await service
            .QueryAsync(
            query: query)
            .ToArrayAsync()
            .ConfigureAwait(false)
            ?? Enumerable.Empty<GameRecordResult>();

        Assert.True(actual.Any());
        Assert.Equal("4ued5l50heuf", actual.First().PlayerCode);
        Assert.Equal("819776544", actual.First().ExternalRoundId);
        Assert.False(actual.First().IsClosed);
        Assert.False(actual.First().IsDraw);
        Assert.Equal(DateTimeOffset.Parse("2022-04-20T14:11:54+08:00"), actual.First().ExternalCreatedOn);
        Assert.Equal(DateTimeOffset.UnixEpoch, actual.First().ExternalClosedOn);
        Assert.Equal(60, actual.First().Bet);
        Assert.Equal(0, actual.First().WinLoss);
        Assert.Equal(0, actual.First().Settle);
        Assert.Equal(string.Empty, actual.First().AdditionalData);
    }

    [Fact]
    public async Task Parlay_lang_zh_cht_Is_Close()
    {
        var query = new GameRecordQuery(
            ApiUrl: new Uri("http://localhost:8088/"),
            PartnerCode: "partnerCode",
            SecretKey: "secretKey",
            SecretIV: "secretIV",
            StartTime: DateTimeOffset.Parse("2022-04-16T00:00:00+08:00"),
            EndTime: DateTimeOffset.Parse("2022-04-17T00:00:00+08:00"),
            PageIndex: 1,
            PageSize: 1000);

        var fakeClient = Substitute.For<IGameApiClient>();
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var showBetInfos = new ShowBetInfo[]
        {
            new ShowBetInfo(
                MemberCode: "4ugl1wyedic5",
                RecordDate: "2022-04-16 17:00:04",
                BatchNo: "123499081",
                ShowType:  "過關<br><font color='red'>2x3</font>",
                ShowDiscount: "有單頭@0.9000",
                ShowContent: "&nbsp;HK  9場&nbsp;@&nbsp;2022-04-16 17:10:00<br>&nbsp;@&nbsp;<font color='FireBrick'><b>QPL</b></font>&nbsp;&nbsp;<font color='FireBrick'><b>1+12</b></font>&nbsp;&nbsp;<br>&nbsp;HK  10場&nbsp;@&nbsp;2022-04-16 17:10:00<br>&nbsp;@&nbsp;<font color='FireBrick'><b>QPL</b></font>&nbsp;&nbsp;<font color='FireBrick'><b>7>12+14</b></font>&nbsp;&nbsp;<br>&nbsp;每注金額:10<Br>",
                ShowResult: "<font font-family: Arial,Verdana;>1,8,12,7,10,4,11,6,5,9,3,2</font><br><font font-family: Arial,Verdana;>8,4,3,9,14,1,5,10,2,11,7,13,12,6</font><br>",
                ShowAmount: "50",
                ShowWin: "129.00",
                ShowOddsWin: "0",
                ShowDivideDate: "2022-04-16 17:39:34",
                ShowRaceDate: "2022-04-16 17:10:00",
                AdditionalData: string.Empty  )
        };
        _ = fakeClient
            .ShowBetListAsync(
            apiUrl: Arg.Is(query.ApiUrl),
            partnerCode: Arg.Is(query.PartnerCode),
            secretKey: Arg.Is(query.SecretKey),
            secretIV: Arg.Is(query.SecretIV),
            startTime: Arg.Is(query.StartTime),
            endTime: Arg.Is(query.EndTime),
            pageIndex: Arg.Is(query.PageIndex),
            pageSize: Arg.Is(query.PageSize),
            cancellationToken: Arg.Any<CancellationToken>())
           .Returns(showBetInfos.ToAsyncEnumerable());

        _ = fakeSystemClock.CurrentTime.Returns(returnThis: DateTimeOffset.Parse("2022-04-17T00:00:00+08:00"));

        var service = new GameRecordQueryService(
            fakeClient,
            fakeSystemClock);

        var actual = await service
            .QueryAsync(
            query: query)
            .ToArrayAsync()
            .ConfigureAwait(false)
            ?? Enumerable.Empty<GameRecordResult>();

        Assert.True(actual.Any());
        Assert.Equal("4ugl1wyedic5", actual.First().PlayerCode);
        Assert.Equal("123499081", actual.First().ExternalRoundId);
        Assert.True(actual.First().IsClosed);
        Assert.False(actual.First().IsDraw);
        Assert.Equal(DateTimeOffset.Parse("2022-04-16T17:00:04+08:00"), actual.First().ExternalCreatedOn);
        Assert.Equal(DateTimeOffset.Parse("2022-04-16T17:39:34+08:00"), actual.First().ExternalClosedOn);
        Assert.Equal(50, actual.First().Bet);
        Assert.Equal(129.00m, actual.First().WinLoss);
        Assert.Equal(179.00m, actual.First().Settle);
        Assert.Equal(string.Empty, actual.First().AdditionalData);
    }

    [Fact]
    public async Task Parlay_lang_zh_cht_Is_Open()
    {
        var query = new GameRecordQuery(
            ApiUrl: new Uri("http://localhost:8088/"),
            PartnerCode: "partnerCode",
            SecretKey: "secretKey",
            SecretIV: "secretIV",
            StartTime: DateTimeOffset.Parse("2022-04-16T00:00:00+08:00"),
            EndTime: DateTimeOffset.Parse("2022-04-17T00:00:00+08:00"),
            PageIndex: 1,
            PageSize: 1000);

        var fakeClient = Substitute.For<IGameApiClient>();
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var showBetInfos = new ShowBetInfo[]
        {
            new ShowBetInfo(
                MemberCode: "4ugl1wyedic5",
                RecordDate: "2022-04-16 17:00:04",
                BatchNo: "123499081",
                ShowType: "過關<br><font color='red'>2x3</font>",
                ShowDiscount: "有單頭@0.9000",
                ShowContent: "&nbsp;HK  9場&nbsp;@&nbsp;2022-04-16 17:10:00<br>&nbsp;@&nbsp;<font color='FireBrick'><b>QPL</b></font>&nbsp;&nbsp;<font color='FireBrick'><b>1+12</b></font>&nbsp;&nbsp;<br>&nbsp;HK  10場&nbsp;@&nbsp;2022-04-16 17:10:00<br>&nbsp;@&nbsp;<font color='FireBrick'><b>QPL</b></font>&nbsp;&nbsp;<font color='FireBrick'><b>7>12+14</b></font>&nbsp;&nbsp;<br>&nbsp;每注金額:10<Br>",
                ShowResult: "<font font-family: Arial,Verdana;>1,8,12,7,10,4,11,6,5,9,3,2</font><br><font font-family: Arial,Verdana;>8,4,3,9,14,1,5,10,2,11,7,13,12,6</font><br>",
                ShowAmount: "50",
                ShowWin: "129.00",
                ShowOddsWin: "0",
                ShowDivideDate: "2022-04-16 17:39:34",
                ShowRaceDate: "2022-04-16 17:10:00",
                AdditionalData: string.Empty  )
        };
        _ = fakeClient
            .ShowBetListAsync(
            apiUrl: Arg.Is(query.ApiUrl),
            partnerCode: Arg.Is(query.PartnerCode),
            secretKey: Arg.Is(query.SecretKey),
            secretIV: Arg.Is(query.SecretIV),
            startTime: Arg.Is(query.StartTime),
            endTime: Arg.Is(query.EndTime),
            pageIndex: Arg.Is(query.PageIndex),
            pageSize: Arg.Is(query.PageSize),
            cancellationToken: Arg.Any<CancellationToken>())
           .Returns(showBetInfos.ToAsyncEnumerable());

        _ = fakeSystemClock.CurrentTime.Returns(returnThis: DateTimeOffset.Parse("2022-04-16T17:39:35+08:00"));

        var service = new GameRecordQueryService(
            fakeClient,
            fakeSystemClock);

        var actual = await service
            .QueryAsync(
            query: query)
            .ToArrayAsync()
            .ConfigureAwait(false)
            ?? Enumerable.Empty<GameRecordResult>();

        Assert.True(actual.Any());
        Assert.Equal("4ugl1wyedic5", actual.First().PlayerCode);
        Assert.Equal("123499081", actual.First().ExternalRoundId);
        Assert.False(actual.First().IsClosed);
        Assert.False(actual.First().IsDraw);
        Assert.Equal(DateTimeOffset.Parse("2022-04-16T17:00:04+08:00"), actual.First().ExternalCreatedOn);
        Assert.Equal(DateTimeOffset.UnixEpoch, actual.First().ExternalClosedOn);
        Assert.Equal(50, actual.First().Bet);
        Assert.Equal(0.00m, actual.First().WinLoss);
        Assert.Equal(0.00m, actual.First().Settle);
        Assert.Equal(string.Empty, actual.First().AdditionalData);
    }

    [Fact]
    public async Task Draw()
    {
        var query = new GameRecordQuery(
            ApiUrl: new Uri("http://localhost:8088/"),
            PartnerCode: "partnerCode",
            SecretKey: "secretKey",
            SecretIV: "secretIV",
            StartTime: DateTimeOffset.Parse("2021-10-30T00:00:00+08:00"),
            EndTime: DateTimeOffset.Parse("2021-11-01T00:00:00+08:00"),
            PageIndex: 1,
            PageSize: 1000);

        var fakeClient = Substitute.For<IGameApiClient>();
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var showBetInfos = new ShowBetInfo[]
        {
            new ShowBetInfo(
                MemberCode: "4uf2njegpusz",
                RecordDate: "2021-10-31 13:04:19",
                BatchNo: "123499081",
                ShowType: "獨贏+位置",
                ShowDiscount: "早@0.8300<br>早@0.8700<br>",
                ShowContent: "HK 2場&nbsp;@&nbsp;2021-10-31 13:15:00<br>@&nbsp;<font color='FireBrick'><b>WP</b></font>&nbsp;&nbsp;<font color='FireBrick'><B>1</font></B></s>&nbsp;&nbsp;<br>每注金額:200<Br>",
                ShowResult: "7,1,4,5,2,6,3",
                ShowAmount: "400",
                ShowWin: "0.00<br>和局",
                ShowOddsWin: "0",
                ShowDivideDate: "2021-10-31 13:22:25",
                ShowRaceDate: "2021-10-31 13:15:00",
                AdditionalData: string.Empty  )
        };
        _ = fakeClient
            .ShowBetListAsync(
            apiUrl: Arg.Is(query.ApiUrl),
            partnerCode: Arg.Is(query.PartnerCode),
            secretKey: Arg.Is(query.SecretKey),
            secretIV: Arg.Is(query.SecretIV),
            startTime: Arg.Is(query.StartTime),
            endTime: Arg.Is(query.EndTime),
            pageIndex: Arg.Is(query.PageIndex),
            pageSize: Arg.Is(query.PageSize),
            cancellationToken: Arg.Any<CancellationToken>())
           .Returns(showBetInfos.ToAsyncEnumerable());

        var service = new GameRecordQueryService(
            fakeClient,
            fakeSystemClock);

        var actual = await service
            .QueryAsync(
            query: query)
            .ToArrayAsync()
            .ConfigureAwait(false)
            ?? Enumerable.Empty<GameRecordResult>();

        Assert.True(actual.Any());
        Assert.Equal("4uf2njegpusz", actual.First().PlayerCode);
        Assert.Equal("123499081", actual.First().ExternalRoundId);
        Assert.True(actual.First().IsClosed);
        Assert.True(actual.First().IsDraw);
        Assert.Equal(DateTimeOffset.Parse("2021-10-31T13:04:19+08:00"), actual.First().ExternalCreatedOn);
        Assert.Equal(DateTimeOffset.Parse("2021-10-31T13:22:25+08:00"), actual.First().ExternalClosedOn);
        Assert.Equal(400, actual.First().Bet);
        Assert.Equal(0, actual.First().WinLoss);
        Assert.Equal(400, actual.First().Settle);
        Assert.Equal(string.Empty, actual.First().AdditionalData);
    }

    [Fact]
    public async Task Each_Way_Q_Is_Close()
    {
        var query = new GameRecordQuery(
            ApiUrl: new Uri("http://localhost:8088/"),
            PartnerCode: "partnerCode",
            SecretKey: "secretKey",
            SecretIV: "secretIV",
            StartTime: DateTimeOffset.Parse("2022-04-16T00:00:00+08:00"),
            EndTime: DateTimeOffset.Parse("2022-04-17T00:00:00+08:00"),
            PageIndex: 1,
            PageSize: 1000);

        var fakeClient = Substitute.For<IGameApiClient>();
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var showBetInfos = new ShowBetInfo[]
        {
            new ShowBetInfo(
                MemberCode: "4uf2njegpusz",
                RecordDate: "2022-04-16 14:56:44",
                BatchNo: "933761975",
                ShowType: "位置Q",
                ShowDiscount: "早@0.9100<br>",
                ShowContent: "HK 5場&nbsp;@&nbsp;2022-04-16 15:00:00<br>@&nbsp;<font color='FireBrick'><b>QPL 4+7 </b></font>",
                ShowResult: "6,2,5,7",
                ShowAmount: "50",
                ShowWin: "-45.50",
                ShowOddsWin:"0.00",
                ShowDivideDate:  "2022-04-16 15:11:29",
                ShowRaceDate: "2022-04-16 15:00:00",
                AdditionalData: string.Empty  )
        };
        _ = fakeClient
            .ShowBetListAsync(
            apiUrl: Arg.Is(query.ApiUrl),
            partnerCode: Arg.Is(query.PartnerCode),
            secretKey: Arg.Is(query.SecretKey),
            secretIV: Arg.Is(query.SecretIV),
            startTime: Arg.Is(query.StartTime),
            endTime: Arg.Is(query.EndTime),
            pageIndex: Arg.Is(query.PageIndex),
            pageSize: Arg.Is(query.PageSize),
            cancellationToken: Arg.Any<CancellationToken>())
           .Returns(showBetInfos.ToAsyncEnumerable());

        var service = new GameRecordQueryService(
            fakeClient,
            fakeSystemClock);

        var actual = await service
            .QueryAsync(
            query: query)
            .ToArrayAsync()
            .ConfigureAwait(false)
            ?? Enumerable.Empty<GameRecordResult>();

        Assert.True(actual.Any());
        Assert.Equal("4uf2njegpusz", actual.First().PlayerCode);
        Assert.Equal("933761975", actual.First().ExternalRoundId);
        Assert.True(actual.First().IsClosed);
        Assert.False(actual.First().IsDraw);
        Assert.Equal(DateTimeOffset.Parse("2022-04-16T14:56:44+08:00"), actual.First().ExternalCreatedOn);
        Assert.Equal(DateTimeOffset.Parse("2022-04-16T15:11:29+08:00"), actual.First().ExternalClosedOn);
        Assert.Equal(50, actual.First().Bet);
        Assert.Equal(-45.5m, actual.First().WinLoss);
        Assert.Equal(4.5m, actual.First().Settle);
        Assert.Equal(string.Empty, actual.First().AdditionalData);
    }

    [Fact]
    public async Task Single_Is_Close()
    {
        var query = new GameRecordQuery(
            ApiUrl: new Uri("http://localhost:8088/"),
            PartnerCode: "partnerCode",
            SecretKey: "secretKey",
            SecretIV: "secretIV",
            StartTime: DateTimeOffset.Parse("2022-04-16T00:00:00+08:00"),
            EndTime: DateTimeOffset.Parse("2022-04-17T00:00:00+08:00"),
            PageIndex: 1,
            PageSize: 1000);

        var fakeClient = Substitute.For<IGameApiClient>();
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var showBetInfos = new ShowBetInfo[]
        {
            new ShowBetInfo(
                MemberCode: "4ugl1wyedic5",
                RecordDate: "2022-04-16 14:54:53",
                BatchNo: "833757162",
                ShowType: "獨贏",
                ShowDiscount: "代@0.9000<br>",
                ShowContent: "HK 5場&nbsp;@&nbsp;2022-04-16 15:00:00<br>@&nbsp;<font color='FireBrick'><b>WIN 7 </b></font>",
                ShowResult: "6,2,5,7",
                ShowAmount: "50",
                ShowWin: "-45.50",
                ShowOddsWin:"0.00",
                ShowDivideDate:  "2022-04-16 15:11:29",
                ShowRaceDate: "2022-04-16 15:00:00",
                AdditionalData: string.Empty  )
        };
        _ = fakeClient
            .ShowBetListAsync(
            apiUrl: Arg.Is(query.ApiUrl),
            partnerCode: Arg.Is(query.PartnerCode),
            secretKey: Arg.Is(query.SecretKey),
            secretIV: Arg.Is(query.SecretIV),
            startTime: Arg.Is(query.StartTime),
            endTime: Arg.Is(query.EndTime),
            pageIndex: Arg.Is(query.PageIndex),
            pageSize: Arg.Is(query.PageSize),
            cancellationToken: Arg.Any<CancellationToken>())
           .Returns(showBetInfos.ToAsyncEnumerable());

        var service = new GameRecordQueryService(
            fakeClient,
            fakeSystemClock);

        var actual = await service
            .QueryAsync(
            query: query)
            .ToArrayAsync()
            .ConfigureAwait(false)
            ?? Enumerable.Empty<GameRecordResult>();

        Assert.True(actual.Any());
        Assert.Equal("4ugl1wyedic5", actual.First().PlayerCode);
        Assert.Equal("833757162", actual.First().ExternalRoundId);
        Assert.True(actual.First().IsClosed);
        Assert.False(actual.First().IsDraw);
        Assert.Equal(DateTimeOffset.Parse("2022-04-16T14:54:53+08:00"), actual.First().ExternalCreatedOn);
        Assert.Equal(DateTimeOffset.Parse("2022-04-16T15:11:29+08:00"), actual.First().ExternalClosedOn);
        Assert.Equal(50, actual.First().Bet);
        Assert.Equal(-45.5m, actual.First().WinLoss);
        Assert.Equal(4.5m, actual.First().Settle);
        Assert.Equal(string.Empty, actual.First().AdditionalData);
    }
}
