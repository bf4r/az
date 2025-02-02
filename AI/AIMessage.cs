namespace az.AI;

using Newtonsoft.Json;

public class AIMessage
{
    [JsonProperty("role")]
    private string _role { get; set; }

    [JsonIgnore]
    public AIRole Role
    {
        get
        {
            return _role switch
            {
                "user" => AIRole.User,
                "assistant" => AIRole.Assistant,
                "system" => AIRole.System,
                _ => AIRole.System
            };
        }
    }

    [JsonProperty("content")]
    public object Content { get; set; }

    // allow only creation using AIRole enum value
    public AIMessage(AIRole role, object content) : this(role.ToString().ToLower(), content) { }

    [JsonConstructor]
    private AIMessage(string role, object content)
    {
        List<string> validRoles = ["user", "assistant", "system"];
        if (!validRoles.Contains(role)) throw new Exception("Could not create message, invalid role. Only user, assistant and system roles are supported.");
        _role = role;
        Content = content;
    }
}
