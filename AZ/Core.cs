namespace az.AZ;

using az.AI;

public static class Core
{
    public static void Run(string request)
    {
        var projDir = Program.GetProjectDirectory();
        Manager.CreateSession();
        Console.WriteLine("New session: " + Manager.SessionID);
        Console.WriteLine(request);

        var config = AIConfig.GetCurrent() ?? AIConfig.GetDefault();

        var systemMdPath = Path.Combine(projDir, "data", "my", "config", "system.md");
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
            var response = ChatApp.Send(messages, config!, (x) => Console.Write(x)).Result;
            Console.WriteLine("Done!");
            messages.Add(new AIMessage("assistant", response));
            var tags = OutputParser.GetTags(response);
            if (tags.ContainsKey("python"))
            {
                var code = tags["python"];
                bool runCode = false;
                Console.Write("Run Python code? Or tell me what I did wrong! (y/n/...): ");
                var input = Console.ReadLine() ?? "";
                if (input == "y") runCode = true;
                else if (input == "n") runCode = false;
                else
                {
                    outputs = "<feedback>\n{input}\n</feedback>";
                    runCode = false;
                    continue;
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
