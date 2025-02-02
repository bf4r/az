namespace az;

using az.AI;

class Program
{
    static async Task Main(string[] args)
    {
        var apiKey = Environment.GetEnvironmentVariable("AI_API_KEY");
        if (apiKey == null) throw new Exception("API key is not set. Please set the AI_API_KEY environment variable. This will soon be moved to data/my/config/keys.json.");
        var config = new AIConfig(
                apiKey: apiKey,
                // testing model
                model: "meta-llama/llama-3.2-3b-instruct",
                // for actual use
                // model: "openai/gpt-4o-mini",
                baseUrl: "https://openrouter.ai/api/v1",
                stream: true
                );
        var messages = new List<AIMessage>()
        {
            new("system", "You are a helpful assistant."),
            new("user", "write a paragraph about AI")
        };
        var cts = new CancellationTokenSource();
        await foreach (var token in API.GetStreamAsync(messages, config, cts.Token))
        {
            Console.Write(token);
        }
        Console.WriteLine();
    }
}
