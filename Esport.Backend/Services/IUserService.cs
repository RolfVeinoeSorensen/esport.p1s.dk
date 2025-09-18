using Esport.Backend.Entities;
using Esport.Backend.Models.Users;

namespace Esport.Backend.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<User> GetAll();
        User GetById(int id);
        Task<bool> Register(RegisterRequest model);
        Task ForgotPasswordAsync(string email);
        Task<bool> ChangePassword(string token, string password);
    }
}
