using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Tgs.GameInterface.WGViaGaming.Core.ApplicationServices;
using Tgs.GameInterface.WGViaGaming.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WGViaGaming.GrpcServices;

public class GameInterfaceOBetGrpcService : GameInterfaceWGViaGaming.GameInterfaceWGViaGamingBase
{
    private readonly ICommandService<FundInCommand> _fundInService;
    private readonly ICommandService<FundOutCommand> _fundOutService;
    private readonly IGetService<FundStatusQuery, bool> _fundStatusService;
    private readonly IQueryService<GameRecordQuery, GameRecordResult> _gameRecordService;
    private readonly IGetService<GameUrlQuery, string> _gameUrlService;
    private readonly ILogger<GameInterfaceOBetGrpcService> _logger;
    private readonly IGetService<PlayerBalanceQuery, PlayerBalanceResult> _playerBalanceService;
    private readonly ICommandService<PlayerCreateCommand> _playerCreateService;
    private readonly ICommandService<PlayerLogoutCommand> _playerLogoutService;

    public GameInterfaceOBetGrpcService(
        ILogger<GameInterfaceOBetGrpcService> logger,
        ICommandService<FundInCommand> fundInService,
        ICommandService<FundOutCommand> fundOutService,
        IGetService<FundStatusQuery, bool> fundStatusService,
        IQueryService<GameRecordQuery, GameRecordResult> gameRecordService,
        IGetService<GameUrlQuery, string> gameUrlService,
        IGetService<PlayerBalanceQuery, PlayerBalanceResult> playerBalanceService,
        ICommandService<PlayerCreateCommand> playerCreateService,
        ICommandService<PlayerLogoutCommand> PlayerLogoutService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fundInService = fundInService ?? throw new ArgumentNullException(nameof(fundInService));
        _fundOutService = fundOutService ?? throw new ArgumentNullException(nameof(fundOutService));
        _fundStatusService = fundStatusService ?? throw new ArgumentNullException(nameof(fundStatusService));
        _gameRecordService = gameRecordService ?? throw new ArgumentNullException(nameof(gameRecordService));
        _gameUrlService = gameUrlService ?? throw new ArgumentNullException(nameof(gameUrlService));
        _playerBalanceService = playerBalanceService ?? throw new ArgumentNullException(nameof(playerBalanceService));
        _playerCreateService = playerCreateService ?? throw new ArgumentNullException(nameof(playerCreateService));
        _playerLogoutService = PlayerLogoutService ?? throw new ArgumentNullException(nameof(PlayerLogoutService));
    }

    public override Task<CheckingServiceReply> CheckingService(CheckingServiceRequest request, ServerCallContext context)
    {
        _logger.LogWarning("LogOn:{logOn} Message: Create a check connection.({machineName})", DateTime.Now.ToString("o"), request.MachineName);

        return Task.FromResult(new CheckingServiceReply
        {
            MachineName = Environment.MachineName
        });
    }

    public override async Task<BaseReply> FundIn(FundRequest request, ServerCallContext context)
    {
        var command = new FundInCommand(
            ApiUrl: new(request.ApiUrl),
            PartnerCode: request.PartnerCode,
            SecretKey: request.SecretKey,
            SecretIV: request.SecretIV,
            PlayerCode: request.PlayerCode,
            OrderId: request.OrderId,
            Amount: request.Amount);

        await _fundInService
            .ExecuteAsync(
            command: command,
            cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new BaseReply
        {
            State = true
        };
    }

    public override async Task<BaseReply> FundOut(FundRequest request, ServerCallContext context)
    {
        var command = new FundOutCommand(
            ApiUrl: new(request.ApiUrl),
            PartnerCode: request.PartnerCode,
            SecretKey: request.SecretKey,
            SecretIV: request.SecretIV,
            PlayerCode: request.PlayerCode,
            OrderId: request.OrderId,
            Amount: request.Amount);

        await _fundOutService
            .ExecuteAsync(
            command: command,
            cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new BaseReply
        {
            State = true
        };
    }

    public override async Task<BaseReply> FundStatus(FundStatusRequest request, ServerCallContext context)
    {
        var query = new FundStatusQuery(
            ApiUrl: new(request.ApiUrl),
            PartnerCode: request.PartnerCode,
            SecretKey: request.SecretKey,
            SecretIV: request.SecretIV,
            OrderId: request.OrderId);

        var result = await _fundStatusService
            .GetAsync(
            query: query,
            cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new BaseReply
        {
            State = result
        };
    }

    public override async Task<GameRecordReply> GameRecord(GameRecordRequest request, ServerCallContext context)
    {
        var query = new GameRecordQuery(
            ApiUrl: new(request.ApiUrl),
            PartnerCode: request.PartnerCode,
            SecretKey: request.SecretKey,
            SecretIV: request.SecretIV,
            StartTime: request.StartTime.ToDateTimeOffset(),
            EndTime: request.EndTime.ToDateTimeOffset(),
            PageIndex: request.PageIndex,
            PageSize: request.PageSize);

        var results = await _gameRecordService
            .QueryAsync(
                query: query,
                cancellationToken: context.CancellationToken)
            .ToArrayAsync(
                cancellationToken: context.CancellationToken)
            .ConfigureAwait(false)
            ?? Enumerable.Empty<GameRecordResult>();

        var reply = new GameRecordReply();

        if (results.Any())
        {
            foreach (var result in results)
            {
                reply.Items.Add(new GameRecordInfo
                {
                    ExternalRoundId = result.ExternalRoundId,
                    PlayerCode = result.PlayerCode,
                    IsClosed = result.IsClosed,
                    IsDraw = result.IsDraw,
                    ExternalCreatedOn = result.ExternalCreatedOn.ToTimestamp(),
                    ExternalClosedOn = result.ExternalClosedOn.ToTimestamp(),
                    Bet = result.Bet,
                    Settle = result.Settle,
                    WinLoss = result.WinLoss,
                    AdditionalData = result.AdditionalData
                });
            }
        }

        return reply;
    }

    public override async Task<GameUrlReply> GameUrl(GameUrlRequest request, ServerCallContext context)
    {
        var query = new GameUrlQuery(
            ApiUrl: new(request.ApiUrl),
            PartnerCode: request.PartnerCode,
            SecretKey: request.SecretKey,
            SecretIV: request.SecretIV,
            PlayerCode: request.PlayerCode,
            CurrencyCode: request.CurrencyCode,
            Lang: request.Lang);

        var result = await _gameUrlService
            .GetAsync(
                query: query,
                cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new GameUrlReply
        {
            GameUrl = result
        };
    }

    public override async Task<PlayerBalanceReply> PlayerBalance(PlayerBalanceRequest request, ServerCallContext context)
    {
        var query = new PlayerBalanceQuery(
            ApiUrl: new(request.ApiUrl),
            PartnerCode: request.PartnerCode,
            SecretKey: request.SecretKey,
            SecretIV: request.SecretIV,
            PlayerCode: request.PlayerCode);

        var result = await _playerBalanceService
            .GetAsync(
                query: query,
                cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new PlayerBalanceReply
        {
            IsPlayerExist = result.IsPlayerExist,
            Balance = result.Balance
        };
    }

    public override async Task<BaseReply> PlayerCreate(PlayerCreateRequest request, ServerCallContext context)
    {
        var command = new PlayerCreateCommand(
            ApiUrl: new(request.ApiUrl),
            PartnerCode: request.PartnerCode,
            SecretKey: request.SecretKey,
            SecretIV: request.SecretIV,
            PlayerCode: request.PlayerCode,
            CurrencyCode: request.CurrencyCode);

        await _playerCreateService
            .ExecuteAsync(
                command: command,
               cancellationToken: context.CancellationToken)
           .ConfigureAwait(false);

        return new BaseReply
        {
            State = true
        };
    }

    public override async Task<BaseReply> PlayerLogout(PlayerLogoutRequest request, ServerCallContext context)
    {
        var command = new PlayerLogoutCommand(
            ApiUrl: new(request.ApiUrl),
            PartnerCode: request.PartnerCode,
            SecretKey: request.SecretKey,
            SecretIV: request.SecretIV,
            PlayerCode: request.PlayerCode);

        await _playerLogoutService
            .ExecuteAsync(
                command: command,
               cancellationToken: context.CancellationToken)
           .ConfigureAwait(false);

        return new BaseReply
        {
            State = true
        };
    }
}
