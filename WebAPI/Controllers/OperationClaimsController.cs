using Business.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationClaimsController : ControllerBase
    {
        IOperationClaimService _operationClaimService;
        public OperationClaimsController(IOperationClaimService operationClaimService)
        {
            _operationClaimService = operationClaimService;
        }
        [HttpGet("GetAll")]
        public IActionResult GetAllOperationClaim()
        {
            var result = _operationClaimService.GetAllOperationClaim();

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
