using Microsoft.AspNetCore.Mvc;
using Esport.Backend.Authorization;
using Esport.Backend.Entities;
using Esport.Backend.Enums;
using Esport.Backend.Models.Users;
using Esport.Backend.Services;

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
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            var users = userService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("[action]/{id:int}")]
        public ActionResult<AuthenticateResponse> GetUserById(int id)
        {
            // only admins can access other user records
            if (HttpContext.Items["User"] is not User currentUser || (id != currentUser.Id && currentUser.Role != UserRole.Admin))
                return Unauthorized(new { message = "Unauthorized" });

            var user =  userService.GetUserById(id);
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ActionResult<bool>> RegisterUser(RegisterRequest model)
        {
            var response = await userService.RegisterUser(model);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<ActionResult<bool>> ForgotPassword(string email)
        {
            await userService.ForgotPasswordAsync(email);
            return Ok(true);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ActionResult<bool>> ChangePassword(string token, string password)
        {
            var res = await userService.ChangePassword(token,password);
            return Ok(res);
        }
    }
}
