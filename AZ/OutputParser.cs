namespace az.AZ;

using System.Text;
using System.Text.RegularExpressions;

public static class OutputParser
{
    public static List<string> AcceptableTags = ["thoughts", "python", "response"];

    public static Dictionary<string, string> GetTags(string output)
    {
        var lines = output.Split(Environment.NewLine);
        var activeMatches = new List<string>();
        var result = new Dictionary<string, string>();
        var sb = new StringBuilder();
        foreach (var line in lines)
        {
            Match mStart = Regex.Match(line, @"^\s*\<(\w+)\>\s*$");
            Match mEnd = Regex.Match(line, @"^\s*\</(\w+)\>\s*$");
            if (mEnd.Success)
            {
                var tag = mEnd.Groups[1].Value;
                if (!AcceptableTags.Contains(tag) && activeMatches.Count > 0)
                {
                    sb.AppendLine(line);
                    continue;
                }
                if (activeMatches.Contains(tag)) activeMatches.Remove(tag);
                if (!result.ContainsKey(tag)) result[tag] = "";
                result[tag] += sb.ToString();
                sb.Clear();
            }
            // the "else" here is very important, because otherwise it would for example match "/python" as a start tag
            else if (mStart.Success)
            {
                var tag = mStart.Groups[1].Value;
                if (!AcceptableTags.Contains(tag) && activeMatches.Count > 0)
                {
                    sb.AppendLine(line);
                    continue;
                }
                if (!activeMatches.Contains(tag)) activeMatches.Add(tag);
            }
            else if (activeMatches.Count > 0) sb.AppendLine(line);
        }
        // trim all texts in tags
        result.Keys.ToList().ForEach(key => result[key] = result[key].Trim());
        return result;
    }
}
