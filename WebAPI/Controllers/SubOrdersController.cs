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
    public class SubOrdersController : ControllerBase
    {
        ISubOrderService _subOrderService;
        public SubOrdersController(ISubOrderService subOrderService)
        {
            _subOrderService = subOrderService;
        }


        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _subOrderService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetAllOrderedProduct")]
        public IActionResult GetAllOrderedProduct()
        {
            var result = _subOrderService.GetAllOrderedProduct();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("OrderId")]
        public IActionResult OrderId(int orderId)
        {
            var result = _subOrderService.OrderId(orderId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Add")]
        public IActionResult GetAllDto(SubOrder subOrder)
        {
            var result = _subOrderService.Add(subOrder);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Update")]
        public IActionResult Update(SubOrder subOrder)
        {
            var result = _subOrderService.Update(subOrder);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Delete")]
        public IActionResult Delete(SubOrder subOrder)
        {
            var result = _subOrderService.Delete(subOrder);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
