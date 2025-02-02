namespace az.AI;

using System.Text;

public class ChatApp
{
    public List<AIMessage>? Messages { get; set; }
    public void Start()
    {
        if (Messages == null) Messages = new();
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
        Messages.Add(new("system", "You are a helpful assistant."));
        while (true)
        {
            Console.Write("> ");
            var input = Console.ReadLine() ?? "";
            if (!string.IsNullOrEmpty(input))
            {
                Messages.Add(new("user", input));
                var response = Send(Messages, config, (x) => Console.Write(x)).GetAwaiter().GetResult();
                Messages.Add(new("assistant", response));
            }
        }
    }
    public static async Task<string> Send(List<AIMessage> messages, AIConfig config, Action<string>? onToken = null)
    {
        var cts = new CancellationTokenSource();
        var sb = new StringBuilder();
        await foreach (var token in API.GetStreamAsync(messages, config, cts.Token))
        {
            sb.Append(token);
            onToken?.Invoke(token);
        }
        Console.WriteLine();
        return sb.ToString();
    }
}
