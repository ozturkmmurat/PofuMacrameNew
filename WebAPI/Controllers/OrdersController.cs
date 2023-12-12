using Business.Abstract;
using Entities.Concrete;
using Entities.Dtos.Order;
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
    public class OrdersController : ControllerBase
    {
        IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _orderService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetAllByUserId")]
        public IActionResult GetAllByUserId(int userId)
        {
            var result = _orderService.GetAllByUserId(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetAllUserOrderDto")]
        public IActionResult GetAllUserOrderDto()
        {
            var result = _orderService.GetAllUserOrderDto();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpGet("GetUserOrderDtoDetail")]
        public IActionResult GetUserOrderDtoDetail(int orderId)
        {
            var result = _orderService.GetUserOrderDtoDetail(orderId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("OrderCode")]
        public IActionResult OrderCode(string orderCode)
        {
            var result = _orderService.OrderCode(orderCode);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("TsaAdd")]
        public IActionResult TsaAdd(OrderDto orderDto)
        {
            var result = _orderService.TsaAdd(orderDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("TsaUpdate")]
        public IActionResult TsaUpdate(OrderDto orderDto)
        {
            var result = _orderService.TsaUpdate(orderDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Delete")]
        public IActionResult Delete(Order order)
        {
            var result = _orderService.Delete(order);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
