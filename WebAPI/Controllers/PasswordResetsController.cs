using Business.Abstract;
using Business.VirtualPos.Iyzico.Abstract;
using Entities.Concrete;
using Entities.Dtos.User;
using Entities.EntityParameter.PasswordReset;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordResetsController : ControllerBase
    {
        IPasswordResetService _passwordResetService;
        public PasswordResetsController(
            IPasswordResetService passwordResetService)
        {
            _passwordResetService = passwordResetService;
        }

        [HttpGet("GetByUrl")]
        public IActionResult GetByUrl(string url)
        {
            var result = _passwordResetService.GetByUrl(url);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetByCodeUrl")]
        public IActionResult GetByCodeUrl(string codeUrl)
        {
            var result = _passwordResetService.GetByCodeUrl(codeUrl);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("SendPasswordResetCode")]
        public IActionResult SendPasswordResetCode(PasswordResetParameter passwordResetParameter)
        {
            var result = _passwordResetService.SendPasswordResetCode(passwordResetParameter);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("SendPasswordResetLink")]
        public IActionResult SendPasswordResetLink(PasswordResetParameter passwordResetParameter)
        {
            var result = _passwordResetService.SendPasswordResetLink(passwordResetParameter);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("PasswordReset")]
        public IActionResult SendPasswordResetLink(UserPasswordResetDto userPasswordResetDto)
        {
            var result = _passwordResetService.PasswordReset(userPasswordResetDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
