using Microsoft.AspNetCore.Mvc;

namespace swagerVersionning.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/products")]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("Liste des produits - V1");
    }
}