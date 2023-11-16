
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var api = new OpenAI_API.OpenAIAPI("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");
var chat = api.Chat.CreateConversation();

app.MapGet("/", (HttpContext context) =>
{
    return context.Response.WriteAsync(@"
        <!DOCTYPE html>
        <html lang=""en"">
        <head>
            <meta charset=""UTF-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <title>OpenAI Chatbot</title>
        </head>
        <body>
            <h1>OpenAI Chatbot</h1>

            <form id=""openAiForm"">
                <label for=""wordInput"">Enter a word:</label>
                <input type=""text"" id=""wordInput"" name=""query"" required />
                <button type=""button"" onclick=""getOpenAiResponse()"">Get OpenAI Response</button>
            </form>

            <textarea id=""responseOutput"" rows=""5"" cols=""50"" readonly></textarea>

            <script>
                async function getOpenAiResponse() {
                    const wordInput = document.getElementById('wordInput').value;
                    const responseOutput = document.getElementById('responseOutput');

                    const response = await fetch('/GetOpenAiResponse', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/x-www-form-urlencoded',
                        },
                        body: new URLSearchParams({ 'query': wordInput }),
                    });

                    const responseData = await response.text();
                    responseOutput.value = responseData;
                }
            </script>
        </body>
        </html>
    ");
});

app.MapPost("/GetOpenAiResponse", async (HttpContext context) =>
{
    var query = context.Request.Form["query"];
    chat.AppendUserInput("Please give a list of semantically related keywords for " + query);
    string response = await chat.GetResponseFromChatbotAsync();
    return response;
});



app.Run();
