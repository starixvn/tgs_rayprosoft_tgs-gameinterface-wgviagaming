namespace Tgs.GameInterface.WGViaGaming.Core;

public interface ISystemClock
{
	DateTimeOffset CurrentTime { get; }
}
