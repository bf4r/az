namespace az.AI;

using Newtonsoft.Json;

public class AIConfig
{
    [JsonProperty("apiKey")]
    public string APIKey { get; set; }
    [JsonProperty("model")]
    public string Model { get; set; }
    [JsonProperty("baseUrl")]
    public string BaseUrl { get; set; }
    [JsonProperty("stream")]
    public bool Stream { get; set; }

    [JsonConstructor]
    public AIConfig(string apiKey, string model, string baseUrl, bool stream)
    {
        APIKey = apiKey;
        Model = model;
        BaseUrl = baseUrl;
        Stream = stream;
    }
}
