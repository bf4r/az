namespace az;

using az.AZ;

class Program
{
    public static string GetProjectDirectory()
    {
        string? currentDirectory = Directory.GetCurrentDirectory();
        while (currentDirectory != null && !File.Exists(Path.Combine(currentDirectory, "Program.cs")))
        {
            currentDirectory = Directory.GetParent(currentDirectory)?.FullName;
        }
        return currentDirectory ?? throw new DirectoryNotFoundException("Couldn't find project directory containing Program.cs");
    }
    static void Main(string[] args)
    {
        var argString = string.Join(' ', args);
        Core.Run(argString);

        // var ca = new AI.ChatApp();
        // ca.Start();
    }
}
