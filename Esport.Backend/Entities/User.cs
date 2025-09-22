#nullable disable

using Esport.Backend.Enums;

namespace Esport.Backend.Entities
{
    public partial class User
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public UserRole Role { get; set; }

        public DateTime CreatedUtc { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordResetToken { get; set; }

        public DateTime PasswordResetTokenExpiration { get; set; }

        public string AddressStreet { get; set; }

        public int? AddressStreetNumber { get; set; }

        public string AddressFloor { get; set; }

        public string AddressSide { get; set; }

        public string AddressPostalCode { get; set; }

        public string AddressCity { get; set; }

        public string Mobile { get; set; }

        public bool ConsentShowImages { get; set; }

        public virtual ICollection<User> InverseParent { get; set; } = new List<User>();

        public virtual User Parent { get; set; }
    }
}
