using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Versioning;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

// === Versioning API ===
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// === Swagger ===
builder.Services.AddEndpointsApiExplorer();

// SwaggerGen avec documentation XML
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
    else
    {
        Console.WriteLine($"❌ Fichier XML de documentation introuvable : {xmlPath}");
    }

    options.DocInclusionPredicate((version, desc) =>
    {
        var versions = desc.ActionDescriptor.EndpointMetadata
                            .OfType<ApiVersionAttribute>()
                            .SelectMany(attr => attr.Versions);

        var cleanVersion = version.Replace("v", "");

        // 🔄 Comparaison souple entre "1" et "1.0"
        return versions.Any(v => v.ToString() == cleanVersion || v.ToString() == $"{cleanVersion}.0");
    });
});

builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

var app = builder.Build();

var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.DocumentTitle = "Documentation API - Versioning Example";

    foreach (var desc in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", $"API {desc.GroupName.ToUpperInvariant()}");
    }
});

app.UseAuthorization();
app.MapControllers();
app.Run();

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;
    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var desc in _provider.ApiVersionDescriptions)
        {
            var info = new OpenApiInfo()
            {
                Title = $"My API {desc.ApiVersion}",
                Version = desc.ApiVersion.ToString(),
                Description = desc.IsDeprecated 
                    ? "⚠️ Cette version de l'API est obsolète. Merci de migrer vers une version plus récente." 
                    : "Version stable."
            };

            options.SwaggerDoc(desc.GroupName, info);  
        }
    }
}