using Business.Abstract;
using Entities.Concrete;
using Entities.Dtos.Product;
using Entities.Dtos.ProductVariant;
using Entities.EntitiyParameter.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _productService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetAllProductDto")]
        public IActionResult GetAllProductDto()
        {
            var result = _productService.GetallProductDto();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        //Ürünün Ana varyantlarını çekiyoruz.
        [HttpPost("GetAllProductVariantDtoGroupVariant")]
        public IActionResult GetAllProductVariantDtoGroupVariant(FilterProduct filterProduct)
        {
            var result = _productService.GetAllProductVariantDtoGroupVariant(filterProduct.CategoryId, filterProduct.Attributes);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            var result = _productService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetByProductDto")]
        public IActionResult GetByProductDto(int productId)
        {
            var result = _productService.GetByProductDto(productId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetProductDetailDtoByPvId")]
        public IActionResult GetProductDetailDtoByPvId(int productVariantId)
        {
            var result = _productService.GetProductDetailDtoByPvId(productVariantId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Add")]
        public IActionResult Add(Product product)
        {
            var result = _productService.Add(product);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("TsaAdd")]
        public IActionResult TsaAdd(AddProductVariant addProductVariant)
        {
            var result = _productService.TsaAdd(addProductVariant);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Update")]
        public IActionResult Update(Product product)
        {
            var result = _productService.Update(product);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Delete")]
        public IActionResult Delete(Product product)
        {
            var result = _productService.Delete(product);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
