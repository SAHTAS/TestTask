using System.Threading.Tasks;
using API.Helpers;
using API.Models;
using Application.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [Authorize]
        [HttpGet("{userId:int}")]
        public async Task<ActionResult<User>> Get(int userId)
        {
            var user = await userService.GetUserAsync(userId);

            return new JsonResult(user);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<User>> Get()
        {
            var users = await userService.GetAllUsersAsync();

            return new JsonResult(users);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        public async Task<ActionResult<int>> CreateUser([FromBody] CreateUserRequestModel model)
        {
            var newUserId = await userService.CreateUserAsync(model.Login, model.Password);

            return new JsonResult(newUserId);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("{userId:int}")]
        public async Task<ActionResult> DeleteUser(int userId)
        {
            await userService.DeleteUserAsync(userId);

            return Ok();
        }
    }
}