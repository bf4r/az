namespace az.AZ;

using az.AI;

public static class Core
{
    public static void Run(string request)
    {
        Console.WriteLine(request);
        var apiKey = Environment.GetEnvironmentVariable("AI_API_KEY");
        if (apiKey == null) throw new Exception("API key is not set. Please set the AI_API_KEY environment variable. This will soon be moved to data/my/config/keys.json.");

        var config = new AIConfig(
                apiKey: apiKey,
                model: "meta-llama/llama-3-70b-instruct",
                // model: "meta-llama/llama-3.2-3b-instruct",
                baseUrl: "https://openrouter.ai/api/v1",
                stream: true
                );


        var projDir = Program.GetProjectDirectory();
        var systemMdPath = Path.Combine(projDir, "system.md");
        var systemMessage = File.ReadAllText(systemMdPath);

        var messages = new List<AIMessage>();

        messages.Add(new AIMessage("system", systemMessage));
        messages.Add(new AIMessage("user", request));

        string outputs = "";
        while (true)
        {
            if (outputs != "")
            {
                messages.Add(new AIMessage("user", outputs));
            }
            Console.WriteLine("Sending request...");
            var response = ChatApp.Send(messages, config, (x) => Console.Write(x)).Result;
            Console.WriteLine("Done!");
            messages.Add(new AIMessage("assistant", response));
            var tags = OutputParser.GetTags(response);
            if (tags.ContainsKey("python"))
            {
                var code = tags["python"];
                bool runCode = false;
                while (true)
                {
                    Console.Write("Proceed? (y/n): ");
                    var input = Console.ReadLine() ?? "";
                    if (input.StartsWith("y"))
                    {
                        runCode = true;
                        break;
                    }
                    else if (input.StartsWith("n"))
                    {
                        runCode = false;
                        break;
                    }
                }
                if (runCode)
                {
                    var output = PythonExecutor.RunPythonScript(code);
                    if (!string.IsNullOrEmpty(output))
                    {
                        outputs = $"""
                            <output>
                            {output}
                            </output>
                            """;
                    }
                }
            }
            if (tags.ContainsKey("response"))
            {
                Console.WriteLine(tags["response"]);
                break;
            }
        }
    }
}
