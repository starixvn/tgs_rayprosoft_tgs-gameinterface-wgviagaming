using System.Diagnostics;

namespace Tgs.GameInterface.WGViaGaming.Client;

public class DiagnosticsInterceptorOptions
{
	public ActivitySource InterceptorActivitySource { get; set; } =
		new ActivitySource($"Tgs.GameInterface.WGViaGaming.Client.DiagnosticsInterceptor");
}
