using Azure;
using Esport.Backend.Authorization;
using Esport.Backend.Entities;
using Esport.Backend.Enums;
using Esport.Backend.Models;
using Esport.Backend.Models.Users;
using Esport.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Esport.Backend.Controllers
{
    public class UsersController(IUserService userService) : ControllerBase
    {
        private readonly IUserService userService = userService;

        [HttpPost("[action]")]
        public ActionResult<AuthenticateResponse> Authenticate([FromBody] AuthenticateRequest model)
        {
            var response = userService.Authenticate(model);

            return Ok(response);
        }

        [Authorize(UserRole.Admin)]
        [HttpGet("[action]")]
        public async Task<ActionResult<List<AuthUser>>> GetAllUsers()
        {
            var users = await userService.GetAllUsers();
            return Ok(users);
        }

        [Authorize(UserRole.Admin)]
        [HttpGet("[action]")]
        public async Task<ActionResult<List<Team>>> GetAllTeams()
        {
            var teams = await userService.GetAllTeams();
            return Ok(teams);
        }

        [Authorize(UserRole.Admin)]
        [HttpGet("[action]")]
        public async Task<ActionResult<List<AuthUser>>> GetAllTeamUsers(int teamId)
        {
            var teams = await userService.GetAllTeamUsers(teamId);
            return Ok(teams);
        }

        [Authorize([UserRole.Admin, UserRole.MemberAdult, UserRole.MemberKid, UserRole.Editor])]
        [HttpGet("[action]/{id:int}")]
        public ActionResult<AuthUser> GetUserById(int id)
        {
            // only admins can access other user records
            if (HttpContext.Items["User"] is not AuthUser currentUser || (currentUser.Roles.Any(x => x.Role.Equals(UserRole.Admin)) == false && currentUser.Id != id))
                return Unauthorized(new { message = "Unauthorized" });

            var user = userService.GetUserById(id);
            return Ok(user);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<SubmitResponse>> RegisterUser([FromBody] RegisterRequest req)
        {
            var response = await userService.RegisterUser(req);
            return Ok(response);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<SubmitResponse>> ForgotPassword([FromBody] ForgotPasswordRequest req)
        {
            var response = await userService.ForgotPasswordAsync(req);
            return Ok(response);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<SubmitResponse>> ChangePassword([FromBody] ChangePasswordRequest req)
        {
            var changed = await userService.ChangePassword(req.Token, req.Password);
            var response = new SubmitResponse { Ok = changed, Message = changed == true ? "Password blev skiftet" : "Password blev ikke skiftet" };
            return Ok(response);
        }
        [HttpPost("[action]")]
        public async Task<ActionResult<SubmitResponse>> ActivateUser(string token)
        {
            var activated = await userService.ActivateUser(token);
            var response = new SubmitResponse { Ok = activated, Message = activated == true ? "Din bruger er aktiveret" : "Din bruger kunne ikke aktiveres" };
            return Ok(response);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<SubmitResponse>> UpdateUser([FromBody] UpdateUserRequest req)
        {
            // only admins can access other user records
            if (HttpContext.Items["User"] is not AuthUser currentUser || (currentUser.Roles.Any(x => x.Role.Equals(UserRole.Admin)) == false && currentUser.Id != req.Id))
                return Unauthorized(new { message = "Unauthorized" });
            var response = await userService.UpdateUser(req);
            return Ok(response);
        }

        [Authorize(UserRole.Admin)]
        [HttpPost("[action]")]
        public async Task<ActionResult<SubmitResponse>> AddUserToTeam([FromBody] UserToTeamRequest req)
        {
            var response = await userService.AddUserToTeam(req);
            return Ok(response);
        }
        [Authorize(UserRole.Admin)]
        [HttpPost("[action]")]
        public async Task<ActionResult<SubmitResponse>> RemoveUserFromTeam([FromBody] UserToTeamRequest req)
        {
            var response = await userService.RemoveUserFromTeam(req);
            return Ok(response);
        }
        [Authorize(UserRole.Admin)]
        [HttpPost("[action]")]
        public async Task<ActionResult<SubmitResponse>> CreateOrUpdateTeam([FromBody] Team req)
        {
            var response = await userService.CreateOrUpdateTeam(req);
            return Ok(response);
        }
    }
}
