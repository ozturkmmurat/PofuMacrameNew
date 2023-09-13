using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryAttributesController : ControllerBase
    {
        ICategoryAttributeService _categoryAttributeService;
        public CategoryAttributesController(ICategoryAttributeService categoryAttributeService)
        {
            _categoryAttributeService = categoryAttributeService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _categoryAttributeService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetByAttributeIdCategoryId")]
        public IActionResult GetByAttributeIdCategoryId(int attributeId, int categoryId)
        {
            var result = _categoryAttributeService.GetByAttributeIdCategoryId(attributeId, categoryId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetAllByCategoryId")]
        public IActionResult GetAllByCategoryId(int categoryId)
        {
            var result = _categoryAttributeService.GetAllByCategoryId(categoryId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetAllSlctCategoryByCategoryId")]
        public IActionResult GetAllSlctCategoryByCategoryId(int categoryId)
        {
            var result = _categoryAttributeService.GetAllSlctCategoryByCategoryId(categoryId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetAllDtoTrueSlicerAttribute")]
        public IActionResult GetAllViewDtoTrueVariantAttribute(int categoryId)
        {
            var result = _categoryAttributeService.GetAllDtoTrueSlicer(categoryId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetAllViewDtoTrueSlicerAttribute")]
        public IActionResult GetAllViewDtoTrueSlicerAttribute(int categoryId)
        {
            var result = _categoryAttributeService.GetAllViewDtoTrueSlicerAttribute(categoryId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Add")]
        public IActionResult Add(CategoryAttribute categoryAttribute)
        {
            var result = _categoryAttributeService.Add(categoryAttribute);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Update")]
        public IActionResult Update(CategoryAttribute categoryAttribute)
        {
            var result = _categoryAttributeService.Update(categoryAttribute);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Delete")]
        public IActionResult Delete(CategoryAttribute categoryAttribute)
        {
            var result = _categoryAttributeService.Delete(categoryAttribute);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
