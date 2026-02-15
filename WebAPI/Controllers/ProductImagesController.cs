using Business.Abstract;
using Entities.Concrete;
using Entities.Dtos.ProductImage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImagesController : ControllerBase
    {
        IProductImageService _productImageService;
        public ProductImagesController(IProductImageService productImageService)
        {
            _productImageService = productImageService;
        }

        [HttpGet("GetAllByProductVariantId")]
        public IActionResult GetAllByProductVariantId(int productVariantId)
        {
            var result = _productImageService.GetAllByProductVariantId(productVariantId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Add")]
        public IActionResult Add([FromForm] ProductImage productImage, [FromForm] IFormFile file)
        {
            var result = _productImageService.Add(productImage,file);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Update")]
        public IActionResult Update([FromForm] ProductImage productImage, [FromForm] IFormFile file)
        {
            var result = _productImageService.Update(productImage, file);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Delete")]
        public IActionResult Delete(ProductImage productImage)
        {
            var result = _productImageService.Delete(productImage);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("AddList")]
        public IActionResult AddList([FromForm] ProductImageDto addProductImageDto)
        {
            var result = _productImageService.AddList(addProductImageDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
