using Microsoft.AspNetCore.Mvc;

namespace swagerVersionning.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0",Deprecated = true)]
    [Route("api/v{version:apiVersion}/products")]
    public class ProductsController : ControllerBase
    {
        /// <summary>
        /// Récupère un produit spécifique par son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant unique du produit.</param>
        /// <response code="200">Retourne le produit correspondant à l'identifiant.</response>
        /// <response code="404">Si le produit n'est pas trouvé.</response>
        /// <response code="400">Si l'identifiant est invalide.</response>
        /// <returns>Un objet contenant les informations du produit ou un message d'erreur.</returns>
        [HttpGet]
        public IActionResult Get()
        {
            HttpContext.Response.Headers.Append("Deprecation", "true");
            HttpContext.Response.Headers.Append("Link", "</api/v2/products>; rel=\"successor-version\"");
            return Ok("Liste des produits - V1");
        }
    }
}