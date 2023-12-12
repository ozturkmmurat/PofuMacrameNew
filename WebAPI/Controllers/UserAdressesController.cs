using Business.Abstract;
using Core.Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAdressesController : ControllerBase
    {
        IUserAddressService _userAddressService;
        public UserAdressesController(IUserAddressService userAddressService)
        {
            _userAddressService = userAddressService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _userAddressService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetUserAddresses")]
        public IActionResult GetUserAddresses()
        {
            
            var result = _userAddressService.GetUserAddresses();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Add")]
        public IActionResult Add(UserAddress userAddress)
        {
            var result = _userAddressService.Add(userAddress);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Update")]
        public IActionResult Update(UserAddress userAddress)
        {
            var result = _userAddressService.Update(userAddress);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Delete")]
        public IActionResult Delete(UserAddress userAddress)
        {
            var result = _userAddressService.Delete(userAddress);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
