namespace az.AI;

public class AIRequestBody
{
    public string? model { get; set; }
    public List<AIMessage>? messages { get; set; }
    public bool? stream { get; set; }
}
