using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAttributesController : ControllerBase
    {
        IProductAttributeService _productAttributeService;
        public ProductAttributesController(IProductAttributeService productAttributeService)
        {
            _productAttributeService = productAttributeService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _productAttributeService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetAllByProductId")]
        public IActionResult GetAllByProductId(int productId)
        {
            var result = _productAttributeService.GetAllByProductId(productId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Add")]
        public IActionResult Add(ProductAttribute productAttribute)
        {
            var result = _productAttributeService.Add(productAttribute);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("AddRange")]
        public IActionResult AddRange([FromBody] List<ProductAttribute> productAttributes)
        {
            var result = _productAttributeService.AddRange(productAttributes);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Update")]
        public IActionResult Update(ProductAttribute productAttribute)
        {
            var result = _productAttributeService.Update(productAttribute);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Delete")]
        public IActionResult Delete(ProductAttribute productAttribute)
        {
            var result = _productAttributeService.Delete(productAttribute);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
