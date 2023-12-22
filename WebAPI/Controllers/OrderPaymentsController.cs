using Business.Abstract;
using Business.VirtualPos.Iyzico.Abstract;
using Entities.Concrete;
using Entities.Dtos.User;
using Entities.EntityParameter.CartItem;
using Entities.EntityParameter.Iyzico;
using Entities.LibraryEntities.Iyzico;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderPaymentsController : ControllerBase
    {
        IIyzicoPaymentService _iyzicoPaymentService;
        public OrderPaymentsController(IIyzicoPaymentService iyzicoPaymentService)
        {
            _iyzicoPaymentService = iyzicoPaymentService;
        }

        [HttpPost("Test")]
        public IActionResult Test()
        {
            var result = _iyzicoPaymentService.Test();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Test2")]
        public IActionResult Test2()
        {
            var result = _iyzicoPaymentService.Test2("","");
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("PaymentResult")]
        public IActionResult PaymentResult(PaymentResultPostParameter paymentResultPostParameter)
        {
            var result = _iyzicoPaymentService.PaymentResult(paymentResultPostParameter);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("TsaPayment")]
        public IActionResult TsaPayment(TsaPaymentParameter tsaPaymentParameter)
        {
            var result = _iyzicoPaymentService.TsaPayment(tsaPaymentParameter);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("RefundProduct")]
        public IActionResult RefundProduct(ReturningProduct returningProduct)
        {
            var result = _iyzicoPaymentService.RefundProduct(returningProduct);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("CancelOrder")]
        public IActionResult CancelOrder(CancelOrder cancelOrder)
        {
            var result = _iyzicoPaymentService.CancelOrder(cancelOrder);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

    }
}
