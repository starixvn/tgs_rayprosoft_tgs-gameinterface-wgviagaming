using System.Diagnostics;

namespace Tgs.GameInterface.WGViaGaming;

public class DiagnosticsInterceptorOptions
{
    public ActivitySource InterceptorActivitySource { get; set; } =
        new ActivitySource($"Tgs.GameInterface.WGViaGaming.DiagnosticsInterceptor");
}
