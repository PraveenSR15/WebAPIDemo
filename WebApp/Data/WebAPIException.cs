using System.Text.Json;

namespace WebApp.Data
{
    public class WebAPIException:Exception
    {
        public ErrorResponse? ErrorResponse { get; }

        public WebAPIException(string errorJson)
        {
            ErrorResponse = JsonSerializer.Deserialize<ErrorResponse>(errorJson);
        }
    }
}
