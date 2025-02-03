namespace az.AI;

using System.Text;

public class ChatApp
{
    public List<AIMessage>? Messages { get; set; }
    public void Start()
    {
        if (Messages == null) Messages = new();
        var config = AIConfig.GetCurrent() ?? AIConfig.GetDefault();
        if (string.IsNullOrEmpty(config.GetAPIKey())) throw new Exception("API key is not set. Please set the AI_API_KEY environment variable. This will soon be moved to data/my/config/keys.json.");
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
