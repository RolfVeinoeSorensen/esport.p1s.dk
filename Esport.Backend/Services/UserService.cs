using BCryptNet = BCrypt.Net.BCrypt;
using Microsoft.Extensions.Options;
using Esport.Backend.Authorization;
using Esport.Backend.Entities;
using Esport.Backend.Helpers;
using Esport.Backend.Models.Users;
using Esport.Backend.Data;
using Microsoft.EntityFrameworkCore;
using Esport.Backend.Services.Message;

namespace Esport.Backend.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> logger;
        private readonly DataContext db;
        private readonly IJwtUtils _jwtUtils;
        private readonly AppSettings _appSettings;
        private readonly INotificationService notificationService;

        public UserService(
            ILogger<UserService> logger,
            DataContext context,
            IJwtUtils jwtUtils,
            IOptions<AppSettings> appSettings,
            INotificationService notificationService)
        {
            db = context;
            _jwtUtils = jwtUtils;
            _appSettings = appSettings.Value;
            this.notificationService = notificationService;
            this.logger = logger;
        }


        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = db.Users.SingleOrDefault(x => x.Username == model.Username);

            // validate
            if (user == null || !BCryptNet.Verify(model.Password, user.PasswordHash))
            {
                var ex = new AppException("Username or password is incorrect");
                logger.LogError(ex, "Error Authenticating");
                throw ex;
            }


            // authentication successful so generate jwt token
            var jwtToken = _jwtUtils.GenerateJwtToken(user);
            logger.LogInformation($"{user.Username} logged in at {DateTime.UtcNow}");
            return new AuthenticateResponse(user, jwtToken);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return db.Users;
        }

        public User GetUserById(int id) 
        {
            var user = db.Users.Find(id);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }

        public async Task<bool> RegisterUser(RegisterRequest model)
        {
            var userExist = await db.Users.FirstOrDefaultAsync(x=>x.Username.Equals(model.Username));
            if(userExist != null) return false;
            var user = new User
            {
                Username= model.Username,
                PasswordHash = BCryptNet.HashPassword(model.Password),
                CreatedUtc = System.DateTime.UtcNow,
                Role = Enums.UserRole.User
            };
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task ForgotPasswordAsync(string email)
        {
            var user = await db.Users.FirstOrDefaultAsync(x=>x.Username.Equals(email));
            if(user == null) return;
            logger.LogInformation($"{user.Username} requested new password at {DateTime.UtcNow}");
            user.PasswordResetToken = BCryptNet.HashPassword(Guid.NewGuid().ToString(), BCryptNet.GenerateSalt()).Replace("/","");
            user.PasswordResetTokenExpiration = DateTime.UtcNow.AddHours(1);
            db.Update(user);
            await db.SaveChangesAsync();
            await notificationService.SendForgotPasswordMail(user);
        }

        public async Task<bool> ChangePassword(string token, string password)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.PasswordResetToken.Equals(token));
            if (user == null) return false;
            if(user.PasswordResetToken != null && user.PasswordResetTokenExpiration > DateTime.UtcNow)
            {
                user.PasswordHash = BCryptNet.HashPassword(password);
                db.Update(user);
                await db.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}