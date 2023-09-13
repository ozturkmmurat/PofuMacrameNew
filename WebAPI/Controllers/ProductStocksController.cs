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
    public class ProductStocksController : ControllerBase
    {
        IProductStockService _productStockService;
        public ProductStocksController(IProductStockService productStockService)
        {
            _productStockService = productStockService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _productStockService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetByAllDto")]
        public IActionResult GetAllDto(int productId)
        {
            var result = _productStockService.GetByAllDto(productId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            var result = _productStockService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetByVariantId")]
        public IActionResult GetByVariantId(int variantId)
        {
            var result = _productStockService.GetByVariantId(variantId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Add")]
        public IActionResult Add(ProductStock productStock)
        {
            var result = _productStockService.Add(productStock);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Update")]
        public IActionResult Update(ProductStock productStock)
        {
            var result = _productStockService.Update(productStock);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Delete")]
        public IActionResult Delete(ProductStock productStock)
        {
            var result = _productStockService.Delete(productStock);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
