using Esport.Backend.Authorization;
using Esport.Backend.Entities;
using Esport.Backend.Helpers;
using Esport.Backend.Models;
using Esport.Backend.Models.Users;
using Esport.Backend.Services.Message;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Esport.Backend.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> logger;
        private readonly DataContext db;
        private readonly IJwtUtils _jwtUtils;
        private readonly AppSettings _appSettings;
        private IMemoryCache mc;
        private readonly ICaptchaService cs;
        private readonly INotificationService notificationService;

        public UserService(
            ILogger<UserService> logger,
            DataContext context,
            IJwtUtils jwtUtils,
            IOptions<AppSettings> appSettings,
            IMemoryCache memoryCache,
            ICaptchaService captchaService,
            INotificationService notificationService)
        {
            db = context;
            mc = memoryCache;
            cs = captchaService;
            _jwtUtils = jwtUtils;
            _appSettings = appSettings.Value;
            this.notificationService = notificationService;
            this.logger = logger;
        }


        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = db.AuthUsers.Include(ur => ur.Roles).SingleOrDefault(x => x.Username == model.Username);

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

        public async Task<List<AuthUser>> GetAllUsers()
        {
            return await db.AuthUsers.Include(r => r.Roles).AsNoTracking().ToListAsync(); ;
        }

        public AuthUser GetUserById(int id)
        {
            var user = db.AuthUsers.Include(r => r.Roles).FirstOrDefault(u => u.Id.Equals(id));
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }

        public async Task<SubmitResponse> RegisterUser(RegisterRequest req)
        {
            var response = new SubmitResponse
            {
                Ok = true,
            };
            mc.TryGetValue(req.CaptchaId, out string? storedCaptchaCode);
            if (storedCaptchaCode == null || storedCaptchaCode != req.CaptchaCode)
            {
                response.Ok = false;
                response.Message = "CAPTCHA var ikke udfyldt korrekt";
                response.Captcha = cs.GenerateCaptcha();
            }
            else
            {
                var userExist = await db.AuthUsers.FirstOrDefaultAsync(x => x.Username.Equals(req.Username));
                var pendingRole = await db.AuthRoles.FirstOrDefaultAsync(x => x.Role.Equals(Enums.UserRole.Pending));
                if (userExist != null || pendingRole == null)
                {
                    response.Ok = false;
                    response.Message = "Kunne ikke oprette brugere. M�ske findes denne email allerede i vores system.";
                    response.Captcha = cs.RefreshCaptcha(req.CaptchaId);
                }
                else if (req.Password != req.PasswordRepeat)
                {
                    response.Ok = false;
                    response.Message = "Password var ikke ens. Der blev ikke oprettet en bruger.";
                    response.Captcha = cs.RefreshCaptcha(req.CaptchaId);
                }
                else
                {
                    var user = new AuthUser
                    {
                        Username = req.Username,
                        PasswordHash = BCryptNet.HashPassword(req.Password),
                        FirstName = req.Firstname,
                        LastName = req.Lastname,
                        CreatedUtc = DateTime.UtcNow,
                        Roles = [pendingRole],
                        PasswordResetToken = BCryptNet.HashPassword(Guid.NewGuid().ToString(), BCryptNet.GenerateSalt()).Replace("/", ""),
                        PasswordResetTokenExpiration = DateTime.UtcNow.AddDays(1),
                        ActivateAccountToken = BCryptNet.HashPassword(Guid.NewGuid().ToString(), BCryptNet.GenerateSalt()).Replace("/", ""),
                        ActivateAccountTokenExpiration = DateTime.UtcNow.AddDays(7),
                        IsActivated = false,
                    };
                    logger.LogInformation($"{user.Username} registered as new user at {DateTime.UtcNow}");
                    await notificationService.SendRegisterNewUserConfirmMail(user);
                    await db.AuthUsers.AddAsync(user);
                    await db.SaveChangesAsync();
                    response.Message = "Du har registreret en ny bruger. Vi sender snart en email hvor vi beder dig aktivere din bruger.";
                }
            }
            return response;
        }

        public async Task<SubmitResponse> ForgotPasswordAsync(ForgotPasswordRequest req)
        {
            var response = new SubmitResponse
            {
                Ok = true,
                Message = $"Hvis {req.Email} findes i vores system vil du snart modtage et link til at skifte dit password."
            };
            mc.TryGetValue(req.CaptchaId, out string? storedCaptchaCode);
            if (storedCaptchaCode == null || storedCaptchaCode != req.CaptchaCode)
            {
                response.Ok = false;
                response.Message = "CAPTCHA var ikke udfyldt korrekt";
                response.Captcha = cs.GenerateCaptcha();
            }
            else
            {
                var user = await db.AuthUsers.FirstOrDefaultAsync(x => x.Username.Equals(req.Email));
                if (user != null)
                {
                    logger.LogInformation($"{user.Username} requested new password at {DateTime.UtcNow}");
                    user.PasswordResetToken = BCryptNet.HashPassword(Guid.NewGuid().ToString(), BCryptNet.GenerateSalt()).Replace("/", "");
                    user.PasswordResetTokenExpiration = DateTime.UtcNow.AddDays(1);
                    await notificationService.SendForgotPasswordMail(user);
                    db.Update(user);
                    await db.SaveChangesAsync();
                }

            }
            return response;
        }

        public async Task<bool> ChangePassword(string token, string password)
        {
            var user = await db.AuthUsers.FirstOrDefaultAsync(x => x.PasswordResetToken.Equals(token));
            if (user == null) return false;
            if (user.PasswordResetToken != null && user.PasswordResetTokenExpiration > DateTime.UtcNow)
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

        public async Task<bool> ActivateUser(string token)
        {
            var user = await db.AuthUsers.FirstOrDefaultAsync(x => x.ActivateAccountToken.Equals(token));
            if (user == null) return false;
            if (user.PasswordResetToken != null && user.ActivateAccountTokenExpiration > DateTime.UtcNow)
            {
                user.IsActivated = true;
                db.Update(user);
                await db.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<SubmitResponse> UpdateUser(UpdateUserRequest model)
        {
            var response = new SubmitResponse
            {
                Ok = true,
            };
            var user = await db.AuthUsers.FirstOrDefaultAsync(u => u.Id == model.Id);
            if (user == null)
            {
                response.Ok = false;
                response.Message = "Bruger ikke fundet";
                return response;
            }

            // update user properties
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.AddressStreet = model.AddressStreet;
            user.AddressStreetNumber = model.AddressStreetNumber;
            user.AddressFloor = model.AddressFloor;
            user.AddressSide = model.AddressSide;
            user.AddressPostalCode = model.AddressPostalCode;
            user.AddressCity = model.AddressCity;
            user.Mobile = model.Mobile;
            user.ConsentShowImages = model.ConsentShowImages;
            user.CanBringLaptop = model.CanBringLaptop;
            user.CanBringStationaryPc = model.CanBringStationaryPc;
            user.CanBringPlaystation = model.CanBringPlaystation;

            db.AuthUsers.Update(user);
            await db.SaveChangesAsync();

            response.Message = "Bruger opdateret succesfuldt";
            return response;
        }

        public async Task<List<Team>> GetAllTeams()
        {
            return await db.Teams.Include(u => u.Members).AsNoTracking().ToListAsync();
        }

        public async Task<List<AuthUser>> GetAllTeamUsers(int teamId)
        {
            return await db.AuthUsers
                .Include(u => u.Teams)
                .Where(u => u.Teams.Any(t => t.Id == teamId))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<SubmitResponse> AddUserToTeam(UserToTeamRequest model)
        {
            var response = new SubmitResponse
            {
                Ok = true,
            };
            var user = await db.AuthUsers
                .Include(u => u.Teams)
                .FirstOrDefaultAsync(u => u.Id == model.UserId);
            var team = await db.Teams.FirstOrDefaultAsync(t => t.Id == model.TeamId);
            if (user == null || team == null)
            {
                response.Ok = false;
                response.Message = "Bruger eller hold ikke fundet";
                return response;
            }

            if (user.Teams.Any(t => t.Id == team.Id))
            {
                response.Ok = false;
                response.Message = "Bruger er allerede medlem af holdet";
                return response;
            }

            user.Teams.Add(team);
            db.AuthUsers.Update(user);
            await db.SaveChangesAsync();

            response.Message = "Bruger tilføjet til holdet succesfuldt";
            return response;
        }
        public async Task<SubmitResponse> RemoveUserFromTeam(UserToTeamRequest model)
        {
            var response = new SubmitResponse
            {
                Ok = true,
            };
            var user = await db.AuthUsers
                .Include(u => u.Teams)
                .FirstOrDefaultAsync(u => u.Id == model.UserId);
            var team = await db.Teams.FirstOrDefaultAsync(t => t.Id == model.TeamId);
            if (user == null || team == null)
            {
                response.Ok = false;
                response.Message = "Bruger eller hold ikke fundet";
                return response;
            }

            if (!user.Teams.Any(t => t.Id == team.Id))
            {
                response.Ok = false;
                response.Message = "Bruger er ikke medlem af holdet";
                return response;
            }

            user.Teams.Remove(team);
            db.AuthUsers.Update(user);
            await db.SaveChangesAsync();

            response.Message = "Bruger fjernet fra holdet succesfuldt";
            return response;
        }

        public async Task<SubmitResponse> CreateOrUpdateTeam(Team model)
        {
            var response = new SubmitResponse
            {
                Ok = true,
            };
            Team? team;
            if (model.Id > 0)
            {
                team = await db.Teams.FirstOrDefaultAsync(t => t.Id == model.Id);
                if (team == null)
                {
                    response.Ok = false;
                    response.Message = "Hold ikke fundet";
                    return response;
                }
                team.Name = model.Name;
                team.Description = model.Description;
                db.Teams.Update(team);
                response.Message = "Hold opdateret succesfuldt";
            }
            else
            {
                team = new Team
                {
                    Name = model.Name,
                    Description = model.Description,
                };
                await db.Teams.AddAsync(team);
                response.Message = "Hold oprettet succesfuldt";
            }
            await db.SaveChangesAsync();
            return response;
        }
    }
}