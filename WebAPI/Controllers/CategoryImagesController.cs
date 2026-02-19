using Business.Abstract;
using Entities.Concrete;
using Entities.Dtos.CategoryImage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryImagesController : ControllerBase
    {
        private readonly ICategoryImageService _categoryImageService;

        public CategoryImagesController(ICategoryImageService categoryImageService)
        {
            _categoryImageService = categoryImageService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _categoryImageService.GetAll();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetAllByCategoryId")]
        public IActionResult GetAllByCategoryId(int categoryId)
        {
            var result = _categoryImageService.GetAllByCategoryId(categoryId);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            var result = _categoryImageService.GetById(id);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("Add")]
        public IActionResult Add([FromForm] CategoryImage categoryImage, [FromForm] IFormFile file)
        {
            var result = _categoryImageService.Add(categoryImage, file);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("AddList")]
        public IActionResult AddList([FromForm] CategoryImageDto addCategoryImageDto)
        {
            var result = _categoryImageService.AddList(addCategoryImageDto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("Update")]
        public IActionResult Update([FromForm] CategoryImage categoryImage, [FromForm] IFormFile file)
        {
            var result = _categoryImageService.Update(categoryImage, file);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("Delete")]
        public IActionResult Delete(CategoryImage categoryImage)
        {
            var result = _categoryImageService.Delete(categoryImage);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
