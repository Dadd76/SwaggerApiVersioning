# SwaggerApiVersioning
intégration de Swashbuckle dans un projet ASP.NET Core Web API, avec versioning et documentation enrichie

Swashbuckle permet de :

- Générer une interface interactive (Swagger UI) pour tester vos endpoints REST.
- Documenter automatiquement vos API à partir de vos attributs, routes et types.
- Gérer la versioning, la sécurité (JWT, OAuth), et d'autres options via configuration.
- Exporter une spécification OpenAPI (fichier JSON/YAML) pour l'utiliser avec d'autres outils (Postman, API Gateway...).

# 🔧 Étape 1 : Installation du package
Dans le terminal à la racine du projet :

dotnet add package Swashbuckle.AspNetCore
dotnet add package Microsoft.AspNetCore.Mvc.Versioning

# 🧱 Étape 2 : Configuration dans Program.cs (ou Startup.cs selon la version)

var builder = WebApplication.CreateBuilder(args);

// Ajout du versioning de l'API
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

// Swagger avec support des versions
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// SwaggerGen avec documentation XML
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

// Ajout des contrôleurs
builder.Services.AddControllers();

var app = builder.Build();

// Middleware Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty; // Swagger UI à la racine
});

app.MapControllers();
app.Run();

# 📁 Étape 3 : Activer la génération des commentaires XML
Dans le fichier .csproj :

<PropertyGroup>
  <GenerateDocumentationFile>true</GenerateDocumentationFile>
  <NoWarn>1591</NoWarn>
</PropertyGroup>

# Étape 4 : Exemple de contrôleur versionné
namespace MyApi.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ProductsController : ControllerBase
{
    /// <summary>
    /// Récupère la liste des produits.
    /// </summary>
    /// <returns>Liste des produits</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Get()
    {
        return Ok(new[] { "Product A", "Product B" });
    }
}
Avec cette configuration :

Vous avez un Swagger UI à http://localhost:xxxx/.

Les endpoints sont bien versionnés (/api/v1/products).

La documentation est enrichie avec vos commentaires XML.

http://127.0.0.1:5185/api/v1/products

http://127.0.0.1:5185/api/v2/products

http://127.0.0.1:5185/swagger/index.html


# Regénérer le certificat développeur
Dans le terminal (PowerShell, CMD, ou Bash), exécute la commande suivante :
dotnet dev-certs https --clean
dotnet dev-certs https --trust
