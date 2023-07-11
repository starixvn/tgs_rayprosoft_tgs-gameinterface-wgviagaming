using System.Reflection;
using Tgs.GameInterface.WGViaGaming.Core;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollecitonExtensions
{
    public static IServiceCollection AddGameInterfaceCore(
        this IServiceCollection services)
        => services
        .RegisterAllServiceUtilityTypes(
            Assembly.GetExecutingAssembly(),
            ServiceLifetime.Transient)
        .AddHttpClient<IGameApiClient, GameApiClient>()
        .Services
        .AddSingleton<IHashCryptography, HashCryptography>()
        .AddSingleton<ISystemClock, SystemClock>();
}
