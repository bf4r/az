namespace az.AI;

using Newtonsoft.Json;

public class AIConfig
{
    [JsonProperty("apiKey")]
    public string APIKey { get; set; }
    [JsonProperty("apiKeyEnvVarName")]
    public string APIKeyEnvVarName { get; set; }
    [JsonProperty("model")]
    public string Model { get; set; }
    [JsonProperty("baseUrl")]
    public string BaseUrl { get; set; }
    [JsonProperty("stream")]
    public bool Stream { get; set; }

    [JsonConstructor]
    public AIConfig(string apiKey, string apiKeyEnvVarName, string model, string baseUrl, bool stream)
    {
        APIKey = apiKey;
        APIKeyEnvVarName = apiKeyEnvVarName;
        Model = model;
        BaseUrl = baseUrl;
        Stream = stream;
    }
    public string? GetAPIKey()
    {
        if (APIKey != "" && APIKey != null)
        {
            return APIKey;
        }
        if (APIKeyEnvVarName != "" && APIKeyEnvVarName != null)
        {
            var val = Environment.GetEnvironmentVariable(APIKeyEnvVarName);
            return val;
        }
        return null;
    }
    public static AIConfig? GetCurrent()
    {
        var projDir = Program.GetProjectDirectory();
        var configJson = File.ReadAllText(Path.Combine(projDir, "data", "my", "config", "ai.json"));
        var config = JsonConvert.DeserializeObject<AIConfig>(configJson);
        return config;
    }
    public static AIConfig GetDefault()
    {
        var defaultAIConfig = new AIConfig(
        apiKey: "",
        apiKeyEnvVarName: "AI_API_KEY",
        model: "meta-llama/llama-3.3-70b-instruct",
        baseUrl: "https://openrouter.ai/api/v1",
        stream: true
        );
        return defaultAIConfig;
    }
}
