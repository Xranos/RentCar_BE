using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentCar_BE.Models
{
    [Table("MsCustomer")]
    public class Customer
    {
        [Key]
        [Column("customer_id")]
        [MaxLength(36)]
        public string CustomerId { get; set; }

        [Required]
        [Column("name")]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        [Column("email")]
        [MaxLength(100)]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required]
        [Column("phone_number")]
        [MaxLength(50)]
        public string PhoneNumber { get; set; }

        [Required]
        [Column("address")]
        [MaxLength(500)]
        public string Address { get; set; }

        [Required]
        [Column("driver_license_number")]
        [MaxLength(100)]
        public string DriverLicenseNumber { get; set; }

        [Required]
        [Column("password")]
        [MinLength(5, ErrorMessage = "Password must be at least 5 characters")]
        [MaxLength(100)]
        public string Password { get; set; }

        public ICollection<Rental> Rentals { get; set; }
    }
}
