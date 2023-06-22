using Business.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Dtos.Variant;
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
    public class VariantsController : ControllerBase
    {
        IVariantService _variantService;
        public VariantsController(IVariantService variantService)
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

        [HttpPost("TsaAddList")]
        public IActionResult TsaAdd(List<AddVariantDto> addVariantDtos)
        {
            var result = _variantService.TsaAddList(addVariantDtos);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("TsaUpdateList")]
        public IActionResult TsaUpdate(List<AddVariantDto> addVariantDtos)
        {
            var result = _variantService.TsaUpdateList(addVariantDtos);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Delete")]
        public IActionResult TsaUpdate(Variant variant)
        {
            var result = _variantService.Delete(variant);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
