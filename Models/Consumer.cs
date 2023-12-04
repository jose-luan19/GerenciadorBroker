using System.Text.Json.Serialization;

namespace Models
{
    public class Consumer
    {
        [JsonPropertyName("consumer_tag")]
        public string ConsumerTag { get; set; }
    }
}
