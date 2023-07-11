namespace Tgs.GameInterface.WGViaGaming.Core.Extensions;

public static class DateTimeExtensions
{
    private static readonly TimeZoneInfo _TimeZone;

    static DateTimeExtensions()
    {
        _TimeZone = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
    }

    public static DateTimeOffset StringConvertDateTimeOffset(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return DateTime.UnixEpoch;
        }
        var date = DateTime.Parse(str);

        return new DateTimeOffset(date, _TimeZone.GetUtcOffset(date));
    }

    public static string DateTimeOffsetConvertString(this DateTimeOffset date)
    {
        return TimeZoneInfo.ConvertTime(date.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd");
    }
}
