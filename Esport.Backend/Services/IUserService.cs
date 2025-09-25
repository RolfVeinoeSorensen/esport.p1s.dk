using Esport.Backend.Entities;
using Esport.Backend.Models.Users;

namespace Esport.Backend.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<AuthUser> GetAllUsers();
        AuthUser GetUserById(int id);
        Task<bool> RegisterUser(RegisterRequest model);
        Task ForgotPasswordAsync(string email);
        Task<bool> ChangePassword(string token, string password);
    }
}
