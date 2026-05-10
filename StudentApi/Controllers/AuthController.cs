using Cortex.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentApi.Features.AuthFeatures.Login;

namespace StudentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IMediator _mediator) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LoginResponseModel>> Login([FromBody] LoginRequestModel request)
        {
            var result = await _mediator.SendCommandAsync<LoginRequestModel, LoginResponseModel>(request);
            if (result.status == 401)
            {
                return Unauthorized(result);
            }
            return StatusCode(result.status, result);
        }
    }
}
