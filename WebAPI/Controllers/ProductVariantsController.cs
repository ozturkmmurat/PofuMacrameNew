using Business.Abstract.ProductVariants;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Dtos.ProductVariant;
using Entities.Dtos.ProductVariant.Select;
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
    public class ProductVariantsController : ControllerBase
    {
        IProductVariantService _variantService;
        public ProductVariantsController(IProductVariantService variantService)
        {
            _variantService = variantService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _variantService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetAllByProductId")]
        public IActionResult GetAllByProductId(int productId)
        {
            var result = _variantService.GetAllByProductId(productId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            var result = _variantService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        //Urun detay sayfasındaki varyant ozelliklerini listelemek için default olarak
        [HttpGet("GetDefaultProductVariantDetail")]
        public IActionResult GetDefaultProductVariantDetail(int productId, int parentId)
        {
            var result = _variantService.GetDefaultProductVariantDetail(productId, parentId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        //Secilen ana varyantların alt varyantlarını getirmek icin kullanilan yerler(urun detay sayfasi)
        [HttpPost("GetSubProductVariantDetail")]
        public IActionResult GetSubProductVariantDetail(List<ProductVariantGroupDetailDto> productVariantGroups, int productId, int parentId)
        {
            var result = _variantService.GetSubProductVariantDetail(productVariantGroups, productId, parentId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Delete")]
        public IActionResult Delete(ProductVariant productVariant)
        {
            var result = _variantService.Delete(productVariant);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
