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
    public class AttributeValuesController : ControllerBase
    {
        IAttributeValueService _attributeValueService;
        public AttributeValuesController(IAttributeValueService attributeValueService)
        {
            _attributeValueService = attributeValueService;
        }
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _attributeValueService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetAllByAttributeId")]
        public IActionResult GetAllByAttributeId(int attributeId)
        {
            var result = _attributeValueService.GetAllByAttributeId(attributeId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            var result = _attributeValueService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Add")]
        public IActionResult Add(AttributeValue attributeValue)
        {
            var result = _attributeValueService.Add(attributeValue);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Update")]
        public IActionResult Update(AttributeValue attributeValue)
        {
            var result = _attributeValueService.Update(attributeValue);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Delete")]
        public IActionResult Delete(AttributeValue attributeValue)
        {
            var result = _attributeValueService.Delete(attributeValue);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
