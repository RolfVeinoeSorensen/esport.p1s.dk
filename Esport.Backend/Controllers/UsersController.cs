using Microsoft.AspNetCore.Mvc;
using Esport.Backend.Authorization;
using Esport.Backend.Entities;
using Esport.Backend.Enums;
using Esport.Backend.Models.Users;
using Esport.Backend.Services;
using Esport.Backend.Models;

namespace Esport.Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController(IUserService userService) : ControllerBase
    {
        private readonly IUserService userService = userService;

        [AllowAnonymous]
        [HttpPost("[action]")]
        public ActionResult<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var response = userService.Authenticate(model);
            
            return Ok(response);
        }

        [Authorize(UserRole.Admin)]
        [HttpGet("[action]")]
        public ActionResult<IEnumerable<AuthUser>> GetAllUsers()
        {
            var users = userService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("[action]/{id:int}")]
        public ActionResult<AuthenticateResponse> GetUserById(int id)
        {
            // only admins can access other user records
            if (HttpContext.Items["User"] is not AuthUser currentUser || currentUser.Roles.Any(x => x.Role.Equals(UserRole.Admin)) == true || currentUser.Id != id)
                return Unauthorized(new { message = "Unauthorized" });

            var user =  userService.GetUserById(id);
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ActionResult<SubmitResponse>> RegisterUser([FromBody]RegisterRequest req)
        {
            var response = await userService.RegisterUser(req);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ActionResult<SubmitResponse>> ForgotPassword([FromBody]ForgotPasswordRequest req)
        {
            await userService.ForgotPasswordAsync(req);
            return Ok(true);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ActionResult<bool>> ChangePassword([FromBody] ChangePasswordRequest req)
        {
            var res = await userService.ChangePassword(req.Token,req.Password);
            return Ok(res);
        }
        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ActionResult<bool>> ActivateUser(string token)
        {
            var res = await userService.ActivateUser(token);
            return Ok(res);
        }
    }
}
