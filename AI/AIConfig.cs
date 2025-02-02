namespace az.AI;

using Newtonsoft.Json;

public class AIConfig
{
    [JsonProperty("model")]
    public string Model { get; set; }
    [JsonProperty("baseUrl")]
    public string BaseUrl { get; set; }

    [JsonConstructor]
    public AIConfig(string model, string baseUrl)
    {
        Model = model;
        BaseUrl = baseUrl;
    }
}
