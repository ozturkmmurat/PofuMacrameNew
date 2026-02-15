using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly IProductCategoryService _productCategoryService;

        public ProductCategoriesController(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        [HttpGet("GetByProductId")]
        public IActionResult GetByProductId(int productId)
        {
            var result = _productCategoryService.GetAllByProductIdAsNoTracking(productId);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
