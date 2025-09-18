using Microsoft.AspNetCore.Mvc;
using Esport.Backend.Authorization;
using Esport.Backend.Entities;
using Esport.Backend.Enums;
using Esport.Backend.Models.Users;
using Esport.Backend.Services;
using Esport.Backend.Services.Message;

namespace Esport.Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService, INotificationService notificationService)
        {
            this.userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public ActionResult<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var response = userService.Authenticate(model);
            
            return Ok(response);
        }

        [Authorize(UserRole.Admin)]
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAll()
        {
            var users = userService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public ActionResult<AuthenticateResponse> GetById(int id)
        {
            // only admins can access other user records
            var currentUser = (User)HttpContext.Items["User"];
            if (id != currentUser.Id && currentUser.Role != UserRole.Admin)
                return Unauthorized(new { message = "Unauthorized" });

            var user =  userService.GetById(id);
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ActionResult<bool>> Register(RegisterRequest model)
        {
            var response = await userService.Register(model);
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
