namespace Tgs.GameInterface.WGViaGaming.Core;

internal class SystemClock : ISystemClock
{
	public DateTimeOffset CurrentTime => DateTimeOffset.UtcNow;
}
