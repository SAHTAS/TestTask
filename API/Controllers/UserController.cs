using System.Linq;
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
        public async Task<ActionResult<GetUserResponseModel>> Get(int userId)
        {
            var user = await userService.GetUserAsync(userId);
            
            return new JsonResult(BuildGetUserResponseModel(user));
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<GetUserResponseModel>> Get()
        {
            var users = await userService.GetAllUsersAsync();

            return new JsonResult(users.Select(BuildGetUserResponseModel));
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

        // TODO: Use a better mapping technique
        private GetUserResponseModel BuildGetUserResponseModel(User user)
        {
            return new GetUserResponseModel
            {
                UserId = user.UserId,
                Login = user.Login,
                CreatedDate = user.CreatedDate,
                UserGroupId = user.UserGroupId,
                UserStateId = user.UserStateId,
                UserGroup = new UserGroupResponseModel
                {
                    UserGroupId = user.UserGroup.UserGroupId,
                    Code = user.UserGroup.Code.ToString(),
                    Description = user.UserGroup.Description
                },
                UserState = new UserStateResponseModel
                {
                    UserStateId = user.UserState.UserStateId,
                    Code = user.UserState.Code.ToString(),
                    Description = user.UserState.Description
                }
            };
        }
    }
}