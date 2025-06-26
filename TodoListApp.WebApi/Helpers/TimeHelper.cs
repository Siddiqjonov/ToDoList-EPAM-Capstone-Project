#pragma warning disable IDE0058 // Expression value is never used

namespace TodoListApp.WebApi.Helpers;

public static class TimeHelper
{
    public static DateTime GetDateTime()
    {
        var dtTime = DateTime.UtcNow;
        dtTime.AddHours(5);
        return dtTime;
    }
}
