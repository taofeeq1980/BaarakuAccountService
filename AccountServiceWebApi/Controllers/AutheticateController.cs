using AccountServiceApi.Controllers;
using ApplicationServices.Login;
using ApplicationServices.Login.ViewModel;
using ApplicationServices.Shared.BaseResponse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Threading.Tasks;

namespace InterBank.Api.Controllers
{
    [AllowAnonymous]
    public class AutheticateController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        public AutheticateController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// call this end point to get access token to access other endpoints
        /// </summary>
        [HttpPost("~/connect/token")]
        [ProducesResponseType(typeof(Result<TokenViewModel>), (int)HttpStatusCode.OK)]
        [SwaggerOperation(Summary = "Get Authetication Token")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] ConnectTokenCommand tokenCommand) 
        {
            var response = await _mediator.Send(tokenCommand);
            return Ok(response);
        }
    }
}
