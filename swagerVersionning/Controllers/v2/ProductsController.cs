using Microsoft.AspNetCore.Mvc;

namespace MyApi.Controllers.v2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/products")]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("Liste des produits - V2 avec nouveaux champs !");
    }
}