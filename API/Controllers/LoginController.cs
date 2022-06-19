using System.Threading.Tasks;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService loginService;

        public LoginController(ILoginService loginService)
        {
            this.loginService = loginService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<AuthenticateResponseModel>> Authenticate([FromBody] AuthenticateRequestModel model)
        {
            var token = await loginService.GenerateTokenAsync(model.Login, model.Password);

            return new JsonResult(new AuthenticateResponseModel { Login = model.Login, Token = token });
        }
    }
}