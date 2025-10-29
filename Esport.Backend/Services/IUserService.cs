using Esport.Backend.Entities;
using Esport.Backend.Models;
using Esport.Backend.Models.Users;

namespace Esport.Backend.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        Task<List<AuthUser>> GetAllUsers();
        AuthUser GetUserById(int id);
        Task<SubmitResponse> RegisterUser(RegisterRequest model);
        Task<SubmitResponse> ForgotPasswordAsync(ForgotPasswordRequest model);
        Task<bool> ChangePassword(string token, string password);
        Task<bool> ActivateUser(string token);
        Task<SubmitResponse> UpdateUser(UpdateUserRequest model);
        Task<List<Team>> GetAllTeams();
        Task<List<AuthUser>> GetAllTeamUsers(int teamId);
    }
}
