using System.Net.Mime;
using System.Text.Json;
using Tomlyn.Extensions.Configuration;

var app = WebApplication.Create(args);

var config = new ConfigurationBuilder();
foreach (var file in new DirectoryInfo(app.Environment.ContentRootPath).GetFiles())
{
    var loaded = false;
    switch (file.Extension)
    {
        case ".json":
            config.AddJsonFile(file.FullName, optional: true, reloadOnChange: true);
            loaded = true;
            break;

        case ".toml":
            config.AddTomlFile(file.FullName, optional: true, reloadOnChange: true);
            loaded = true;
            break;

        case ".ini":
            config.AddIniFile(file.FullName, optional: true, reloadOnChange: true);
            loaded = true;
            break;
    }

    if (loaded)
        app.Logger.LogInformation("Loaded {Extension} configuration file from {Path}.", file.Extension, file.FullName);
}

var root = config.Build();
var token = app.Configuration["Token"];

if (token is null)
    app.Logger.LogWarning("Token not set! Anyone can access the entire configuration without any authentication.");

app.MapGet("/", async context =>
{
    if (token is not null && context.Request.Headers.Authorization != $"Bearer {token}")
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return;
    }

    context.Response.ContentType = MediaTypeNames.Application.Json;
    await using var writer = new Utf8JsonWriter(context.Response.Body);
    writer.WriteStartObject();
    foreach (var (key, value) in root.AsEnumerable())
        if (value is not null)
            writer.WriteString(key, value);
    writer.WriteEndObject();
});

app.Run();