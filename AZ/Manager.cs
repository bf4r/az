namespace az.AZ;

using az.AI;

using Newtonsoft.Json;

public static class Manager
{
    public static Guid? SessionID { get; set; } = null;
    public static void CreateSession()
    {
        CreateStructure();
        SessionID = Guid.NewGuid();
        CreateSessionFiles();
    }
    public static void CreateSessionFiles()
    {
        var parent = Path.Combine(Program.GetProjectDirectory(), "data", "my", "sessions");
        var idString = SessionID.ToString();
        if (Directory.Exists(Path.Combine("data", "my", "sessions")) && idString != null)
        {
            var sessionPath = Path.Combine(parent, idString);
            Directory.CreateDirectory(sessionPath);
            Directory.CreateDirectory(Path.Combine(sessionPath, "logs"));
            File.WriteAllText(Path.Combine(sessionPath, "logs", "chat.md"), "");
            File.WriteAllText(Path.Combine(sessionPath, "logs", "chat.json"), "");
            File.WriteAllText(Path.Combine(sessionPath, "logs", "llm-logs.txt"), "");
            File.WriteAllText(Path.Combine(sessionPath, "info.json"), "");
        }
    }
    public static void CreateStructure()
    {
        List<string> paths = [
          "/data",
          "/data/my",
          "/data/my/config",
          "/data/my/sessions"
        ];
        var parent = Program.GetProjectDirectory();
        foreach (var path in paths)
        {
            var newPath = path.Replace('/', Path.DirectorySeparatorChar);
            if (!Directory.Exists(parent + newPath))
            {
                Directory.CreateDirectory(parent + newPath);
            }
        }
        var systemMdPath = Path.Combine(parent, "defaults", "system.md");
        var defaultSystemMessage = "";
        if (File.Exists(systemMdPath))
        {
            defaultSystemMessage = File.ReadAllText(systemMdPath);
        }
        var defaultAIConfig = AIConfig.GetDefault();
        var defaultAIConfigS = JsonConvert.SerializeObject(defaultAIConfig, Formatting.Indented);
        Dictionary<string, string> filesToCreate = new()
        {
            { "data/my/config/ai.json", defaultAIConfigS},
            { "data/my/config/system.md", defaultSystemMessage},
        };
        foreach ((var path, var content) in filesToCreate)
        {
            var fullPath = Path.Combine(parent, path);
            if (!File.Exists(fullPath))
            {
                File.WriteAllText(fullPath, content);
            }
        }
    }
}
