using Microsoft.AspNetCore.Mvc;

namespace swagerVersionning.Controllers.v2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/products")]
    public class ProductsController : ControllerBase
    {
        /// <summary>
        /// Récupère la liste des produits disponibles.
        /// </summary>
        /// <returns>Retourne la liste des produits sous forme de texte.</returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Liste des produits - V2");
        }
    }
}