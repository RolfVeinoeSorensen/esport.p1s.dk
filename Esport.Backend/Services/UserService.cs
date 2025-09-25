using BCryptNet = BCrypt.Net.BCrypt;
using Microsoft.Extensions.Options;
using Esport.Backend.Authorization;
using Esport.Backend.Entities;
using Esport.Backend.Helpers;
using Esport.Backend.Models.Users;
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
            var user = db.AuthUsers.Include(ur=>ur.Roles).SingleOrDefault(x => x.Username == model.Username);

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

        public IEnumerable<AuthUser> GetAllUsers()
        {
            return db.AuthUsers;
        }

        public AuthUser GetUserById(int id) 
        {
            var user = db.AuthUsers.Include(r=>r.Roles).FirstOrDefault(u=>u.Id.Equals(id));
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }

        public async Task<bool> RegisterUser(RegisterRequest model)
        {
            var userExist = await db.AuthUsers.FirstOrDefaultAsync(x=>x.Username.Equals(model.Username));
            var pendingRole = await db.AuthRoles.FirstOrDefaultAsync(x=>x.Role.Equals(Enums.UserRole.Pending));
            if(userExist != null || pendingRole == null) return false;
            var user = new AuthUser
            {
                Username = model.Username,
                PasswordHash = BCryptNet.HashPassword(model.Password),
                FirstName = model.Firstname,
                LastName = model.Lastname,
                CreatedUtc = DateTime.UtcNow,
                Roles = [pendingRole],
                PasswordResetToken = BCryptNet.HashPassword(Guid.NewGuid().ToString(), BCryptNet.GenerateSalt()).Replace("/", ""),
                PasswordResetTokenExpiration = DateTime.UtcNow.AddDays(1)
            };
            logger.LogInformation($"{user.Username} registered as new user at {DateTime.UtcNow}");
            await db.AuthUsers.AddAsync(user);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task ForgotPasswordAsync(string email)
        {
            var user = await db.AuthUsers.FirstOrDefaultAsync(x=>x.Username.Equals(email));
            if(user == null) return;
            logger.LogInformation($"{user.Username} requested new password at {DateTime.UtcNow}");
            user.PasswordResetToken = BCryptNet.HashPassword(Guid.NewGuid().ToString(), BCryptNet.GenerateSalt()).Replace("/","");
            user.PasswordResetTokenExpiration = DateTime.UtcNow.AddDays(1);
            db.Update(user);
            await db.SaveChangesAsync();
            await notificationService.SendForgotPasswordMail(user);
        }

        public async Task<bool> ChangePassword(string token, string password)
        {
            var user = await db.AuthUsers.FirstOrDefaultAsync(x => x.PasswordResetToken.Equals(token));
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