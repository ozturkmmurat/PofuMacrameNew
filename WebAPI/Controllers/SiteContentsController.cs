using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteContentsController : ControllerBase
    {
        private readonly ISiteContentService _siteContentService;

        public SiteContentsController(ISiteContentService siteContentService)
        {
            _siteContentService = siteContentService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _siteContentService.GetAll();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            var result = _siteContentService.GetById(id);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetAllByContentKey")]
        public IActionResult GetAllByContentKey(string contentKey)
        {
            var result = _siteContentService.GetAllByContentKey(contentKey);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("Add")]
        public IActionResult Add([FromForm] SiteContent siteContent, [FromForm] IFormFile file)
        {
            var result = _siteContentService.Add(siteContent, file);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("Update")]
        public IActionResult Update([FromForm] SiteContent siteContent, [FromForm] IFormFile file)
        {
            var result = _siteContentService.Update(siteContent, file);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("Delete")]
        public IActionResult Delete([FromBody] SiteContent siteContent)
        {
            var result = _siteContentService.Delete(siteContent);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
