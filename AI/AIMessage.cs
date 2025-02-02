namespace az.AI;

using Newtonsoft.Json;

public class AIMessage
{
    [JsonProperty("role")]
    private string _role { get; set; }

    [JsonProperty("content")]
    public object Content { get; set; }

    [JsonConstructor]
    public AIMessage(string role, object content)
    {
        List<string> validRoles = ["user", "assistant", "system"];
        if (!validRoles.Contains(role)) throw new Exception("Could not create message, invalid role. Only user, assistant and system roles are supported.");
        _role = role;
        Content = content;
    }
}
