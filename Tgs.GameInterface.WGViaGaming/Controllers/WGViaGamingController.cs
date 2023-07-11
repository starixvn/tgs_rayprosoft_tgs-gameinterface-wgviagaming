using Microsoft.AspNetCore.Mvc;
using Tgs.GameInterface.WGViaGaming.Core.ApplicationServices;
using Tgs.GameInterface.WGViaGaming.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WGViaGaming.Controllers;

[Route("[controller]")]
[ApiController]
public class WGViaGamingController : ControllerBase
{
    /// <summary>
    /// Funds the in asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://via.itd888.com/",
    ///             "partnerCode": "PT",
    ///             "secretKey": "a888888888888888",
    ///             "secretIV": "b888888888888888",
    ///             "playerCode": "4ugse0rsnufp",
    ///             "orderId": "DD381C9E-B3E1-21A8-2ED9-5E0C9010F16E",
    ///             "amount": 1
    ///         }
    /// </remarks>
    [HttpPost("fund/in", Name = "FundIn")]
    public ValueTask FundInAsync(
        [FromServices] ICommandService<FundInCommand> service,
        FundViewModel source)
    {
        var command = new FundInCommand(
            ApiUrl: source.ApiUrl,
            PartnerCode: source.PartnerCode,
            SecretKey: source.SecretKey,
            SecretIV: source.SecretIV,
            PlayerCode: source.PlayerCode,
            OrderId: source.OrderId,
            Amount: source.Amount);

        return service
            .ExecuteAsync(
            command: command,
            cancellationToken: HttpContext.RequestAborted);
    }

    /// <summary>
    /// Funds the out asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://via.itd888.com/",
    ///             "partnerCode": "PT",
    ///             "secretKey": "a888888888888888",
    ///             "secretIV": "b888888888888888",
    ///             "playerCode": "4ugse0rsnufp",
    ///             "orderId": "B29C2434-C14F-77C1-6D83-1EE182E82AB8",
    ///             "amount": 1
    ///         }
    /// </remarks>
    [HttpPost("fund/out", Name = "FundOut")]
    public ValueTask FundOutAsync(
        [FromServices] ICommandService<FundOutCommand> service,
        FundViewModel source)
    {
        var command = new FundOutCommand(
            ApiUrl: source.ApiUrl,
            PartnerCode: source.PartnerCode,
            SecretKey: source.SecretKey,
            SecretIV: source.SecretIV,
            PlayerCode: source.PlayerCode,
            OrderId: source.OrderId,
            Amount: source.Amount);

        return service
            .ExecuteAsync(
            command: command,
            cancellationToken: HttpContext.RequestAborted);
    }

    /// <summary>
    /// Funds the status asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://via.itd888.com/",
    ///             "partnerCode": "PT",
    ///             "secretKey": "a888888888888888",
    ///             "secretIV": "b888888888888888",
    ///             "orderId": "B29C2434-C14F-77C1-6D83-1EE182E82AB8"
    ///         }
    /// </remarks>
    [HttpPost("fund/Status", Name = "FundStatus")]
    public ValueTask<bool> FundStatusAsync(
        [FromServices] IGetService<FundStatusQuery, bool> service,
        FundStatusViewModel source)
    {
        var query = new FundStatusQuery(
            ApiUrl: source.ApiUrl,
            PartnerCode: source.PartnerCode,
            SecretKey: source.SecretKey,
            SecretIV: source.SecretIV,
            OrderId: source.OrderId);

        return service.GetAsync(
            query: query,
            cancellationToken: HttpContext.RequestAborted);
    }

    /// <summary>
    /// Games the record asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://via.itd888.com/",
    ///             "partnerCode": "PT",
    ///             "secretKey": "a888888888888888",
    ///             "secretIV": "b888888888888888",
    ///             "startTime": "2023-06-28T00:00:00+08:00",
    ///             "endTime": "2023-06-29T00:00:00+08:00",
    ///             "pageIndex": 1,
    ///             "pageSize": 1000
    ///         }
    /// </remarks>
    [HttpPost("game/record")]
    public async IAsyncEnumerable<GameRecordResultViewModel> GameRecordAsync(
        [FromServices] IQueryService<GameRecordQuery, GameRecordResult> service,
        GameRecordViewModel source)
    {
        var query = new GameRecordQuery(
            ApiUrl: source.ApiUrl,
            PartnerCode: source.PartnerCode,
            SecretKey: source.SecretKey,
            SecretIV: source.SecretIV,
            StartTime: source.StartTime,
            EndTime: source.EndTime,
            PageIndex: source.PageIndex,
            PageSize: source.PageSize);

        var results = await service
            .QueryAsync(
                query: query,
                cancellationToken: HttpContext.RequestAborted)
            .ToArrayAsync(
                cancellationToken: HttpContext.RequestAborted)
            .ConfigureAwait(false)
            ?? Enumerable.Empty<GameRecordResult>();

        if (!results.Any())
        {
            yield break;
        }

        foreach (var result in results)
        {
            yield return new GameRecordResultViewModel(
                ExternalRoundId: result.ExternalRoundId,
                PlayerCode: result.PlayerCode,
                IsClosed: result.IsClosed,
                IsDraw: result.IsDraw,
                ExternalCreatedOn: result.ExternalCreatedOn,
                ExternalClosedOn: result.ExternalClosedOn,
                Bet: result.Bet,
                Settle: result.Settle,
                WinLoss: result.WinLoss,
                AdditionalData: result.AdditionalData);
        }
    }

    /// <summary>
    /// Games the URL asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://via.itd888.com/",
    ///             "partnerCode": "PT",
    ///             "secretKey": "a888888888888888",
    ///             "secretIV": "b888888888888888",
    ///             "playerCode": "4ugse0rsnufp",
    ///             "currencyCode": "RMB",
    ///             "lang": "1"
    ///         }
    /// </remarks>
    [HttpPost("gameurl")]
    public ValueTask<string> GameUrlAsync(
        [FromServices] IGetService<GameUrlQuery, string> service,
        GameUrlQueryViewModel source)
    {
        var query = new GameUrlQuery(
            ApiUrl: source.ApiUrl,
            PartnerCode: source.PartnerCode,
            SecretKey: source.SecretKey,
            SecretIV: source.SecretIV,
            PlayerCode: source.PlayerCode,
            CurrencyCode: source.CurrencyCode,
            Lang: source.Lang);

        return service
            .GetAsync(
            query: query,
            cancellationToken: HttpContext.RequestAborted);
    }

    /// <summary>
    /// Players the balance asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://via.itd888.com/",
    ///             "partnerCode": "PT",
    ///             "secretKey": "a888888888888888",
    ///             "secretIV": "b888888888888888",
    ///             "playerCode": "4ugse0rsnufp"
    ///         }
    /// </remarks>
    [HttpPost("player/balance", Name = "PlayerBalance")]
    public async ValueTask<PlayerBalanceResultViewModel> PlayerBalanceAsync(
        [FromServices] IGetService<PlayerBalanceQuery, PlayerBalanceResult> service,
        PlayerBalanceViewModel source)
    {
        var query = new PlayerBalanceQuery(
            ApiUrl: source.ApiUrl,
            PartnerCode: source.PartnerCode,
            SecretKey: source.SecretKey,
            SecretIV: source.SecretIV,
            PlayerCode: source.PlayerCode);

        var result = await service
            .GetAsync(
            query: query,
            cancellationToken: HttpContext.RequestAborted)
            .ConfigureAwait(false);

        return new PlayerBalanceResultViewModel(
            IsPlayer: result.IsPlayerExist,
            Balanc: result.Balance);
    }

    /// <summary>
    /// Players the create asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://via.itd888.com/",
    ///             "partnerCode": "PT",
    ///             "secretKey": "a888888888888888",
    ///             "secretIV": "b888888888888888",
    ///             "playerCode": "4ugse0rsnufp",
    ///             "currencyCode": "RMB"
    ///         }
    /// </remarks>
    [HttpPost("player/create", Name = "PlayerCreate")]
    public ValueTask PlayerCreateAsync(
        [FromServices] ICommandService<PlayerCreateCommand> service,
        PlayerCreateViewModel source)
    {
        var command = new PlayerCreateCommand(
            ApiUrl: source.ApiUrl,
            PartnerCode: source.PartnerCode,
            SecretKey: source.SecretKey,
            SecretIV: source.SecretIV,
            PlayerCode: source.PlayerCode,
            CurrencyCode: source.CurrencyCode);

        return service
            .ExecuteAsync(
            command: command,
            cancellationToken: HttpContext.RequestAborted);
    }

    /// <summary>
    /// Players the logout asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://via.itd888.com/",
    ///             "partnerCode": "PT",
    ///             "secretKey": "a888888888888888",
    ///             "secretIV": "b888888888888888",
    ///             "playerCode":"4ugse0rsnufp"
    ///         }
    /// </remarks>
    [HttpPost("player/logout", Name = "PlayerLogout")]
    public ValueTask PlayerLogoutAsync(
        [FromServices] ICommandService<PlayerLogoutCommand> service,
        PlayerLogoutViewModel source)
    {
        var command = new PlayerLogoutCommand(
            ApiUrl: source.ApiUrl,
            PartnerCode: source.PartnerCode,
            SecretKey: source.SecretKey,
            SecretIV: source.SecretIV,
            PlayerCode: source.PlayerCode);

        return service
            .ExecuteAsync(
            command: command,
            cancellationToken: HttpContext.RequestAborted);
    }
}
