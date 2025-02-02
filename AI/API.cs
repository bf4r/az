namespace az.AI;

using az.AI.APIObjects;

using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;

using Newtonsoft.Json;

public static class API
{
    public static async IAsyncEnumerable<string> GetStreamAsync(List<AIMessage> messages, AIConfig config, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", config.APIKey);

        using var request = new HttpRequestMessage(HttpMethod.Post, config.BaseUrl + "/chat/completions");
        request.Content = JsonContent.Create(new AIRequestBody
        {
            messages = messages,
            model = config.Model,
            stream = config.Stream
        });

        using var response = await client.SendAsync(
            request,
            HttpCompletionOption.ResponseHeadersRead,
            cancellationToken
        );
        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var reader = new StreamReader(stream);

        while (!reader.EndOfStream)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string? line = await reader.ReadLineAsync();

            if (string.IsNullOrEmpty(line) || line == "data: [DONE]")
                continue;

            if (line.StartsWith("data: "))
            {
                string jsonData = line.Substring("data: ".Length);
                var res = JsonConvert.DeserializeObject<APICompletionResponse>(jsonData);
                var tok = res?.choices?[0]?.delta?.content;
                if (tok != null)
                {
                    yield return tok;
                }
            }
        }
    }
}
