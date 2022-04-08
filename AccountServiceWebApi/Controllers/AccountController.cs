using ApplicationServices.Accounts.Command;
using ApplicationServices.Shared.BaseResponse;
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
    public class AccountController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        public AccountController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// call this end point to create new customer account
        /// </summary>
        /// <param name="addAccountCommand"></param>
        /// <returns></returns>
        [HttpPost("create-account")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [SwaggerOperation(Summary = "Create New Customer Account")] 
        public async Task<IActionResult> CreateAccountAsync([FromBody] AddAccountCommand addAccountCommand) 
        {
            var response = await _mediator.Send(addAccountCommand);
            return Ok(response);
        }
    }
}
