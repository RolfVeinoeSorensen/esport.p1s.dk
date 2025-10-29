using System.ComponentModel.DataAnnotations;

namespace Esport.Backend.Models.Users
{
    public class UpdateUserRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string AddressStreet { get; set; }

        [Required]
        public int AddressStreetNumber { get; set; }

        public string? AddressFloor { get; set; }

        public string? AddressSide { get; set; }

        [Required]
        public string AddressPostalCode { get; set; }

        [Required]
        public string AddressCity { get; set; }

        public string? Mobile { get; set; }

        [Required]
        public bool ConsentShowImages { get; set; }

        [Required]
        public bool CanBringLaptop { get; set; }

        [Required]
        public bool CanBringStationaryPc { get; set; }

        [Required]
        public bool CanBringPlaystation { get; set; }
    }
}