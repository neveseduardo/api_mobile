namespace WebApi.Helpers;
public static class ApiResponseHelper
{
    public static object Response(int status, object data, string message, bool success)
    {
        return new
        {
            success,
            status,
            data,
            message
        };
    }
}
