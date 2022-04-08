using ApplicationServices.Shared.BaseResponse;
using ApplicationServices.Transactions.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Threading.Tasks;

namespace AccountServiceApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class TransactionController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        public TransactionController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// call this end point to top-up customer account
        /// </summary>
        /// <param name="topUpAccountCommand"></param>
        /// <returns></returns>
        [HttpPost("topup-account")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [SwaggerOperation(Summary = "TopUp Customer Account Balance")]
        public async Task<IActionResult> TopUpAccountAsync([FromBody] TopUpAccountCommand topUpAccountCommand)
        {
            var response = await _mediator.Send(topUpAccountCommand);
            return Ok(response);
        }

        /// <summary>
        /// call this end point to create new customer account
        /// </summary>
        /// <param name="fundTransferCommand"></param>
        /// <returns></returns>
        [HttpPost("fund-transfer")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [SwaggerOperation(Summary = "Transfer from Customer Account")] 
        public async Task<IActionResult> FundTransferAsync([FromBody] FundTransferCommand fundTransferCommand)
        {
            var response = await _mediator.Send(fundTransferCommand);
            return Ok(response);
        }
    }
}
