namespace WebToApp2.Helpers;

public static class DateTimeUtility
{
    public static DateTime BakuLocalTime() => DateTime.UtcNow.AddHours(4);
}