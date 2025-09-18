using Esport.Backend.Enums;
using System.Text.Json.Serialization;

#nullable disable

namespace Esport.Backend.Entities
{
    public partial class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public UserRole Role { get; set; }
        public DateTime CreatedUtc { get; set; }
        public string PasswordResetToken { get; set; }
        public DateTime PasswordResetTokenExpiration { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }
    }
}
