using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductPriceFactorsController : ControllerBase
    {
        IProductPriceFactorService _productPriceFactorService;

        public ProductPriceFactorsController(IProductPriceFactorService productPriceFactorService)
        {
            _productPriceFactorService = productPriceFactorService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _productPriceFactorService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetAllAsNoTracking")]
        public IActionResult GetAllAsNoTracking()
        {
            var result = _productPriceFactorService.GetAllAsNoTracking();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            var result = _productPriceFactorService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Add")]
        public IActionResult Add(ProductPriceFactor productPriceFactor)
        {
            var result = _productPriceFactorService.Add(productPriceFactor);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Update")]
        public IActionResult Update(ProductPriceFactor productPriceFactor)
        {
            var result = _productPriceFactorService.Update(productPriceFactor);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Delete")]
        public IActionResult Delete(ProductPriceFactor productPriceFactor)
        {
            var result = _productPriceFactorService.Delete(productPriceFactor);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
