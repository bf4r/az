namespace az.AZ;

using System.Text;
using System.Diagnostics;

public static class PythonExecutor
{
    public static string RunPythonScript(string pythonScript, string input = "")
    {
        var output = new StringBuilder();

        try
        {
            using var process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = "python",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            process.OutputDataReceived += (sender, args) =>
            {
                if (args.Data != null)
                {
                    Console.WriteLine(args.Data);
                    output.AppendLine(args.Data);
                }
            };

            process.ErrorDataReceived += (sender, args) =>
            {
                if (args.Data != null)
                {
                    Console.WriteLine(args.Data);
                    output.AppendLine(args.Data);
                }
            };
            process.Start();

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            using (var sw = process.StandardInput)
            {
                sw.Write(pythonScript);

                if (!string.IsNullOrEmpty(input))
                {
                    sw.Write(input);
                }
            }
            process.WaitForExit();
            return output.ToString();
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }
}
