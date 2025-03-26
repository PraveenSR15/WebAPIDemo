using System.Text.Json.Serialization;

namespace WebApp.Data
{
    public class ErrorResponse
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        [JsonPropertyName("status")]
        public int StstusCode { get; set; }
        [JsonPropertyName("errors")]
        public Dictionary<string,List<string>>? Errors { get; set; }
    }
}
