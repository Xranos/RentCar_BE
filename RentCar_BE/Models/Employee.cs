using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentCar_BE.Models
{
    [Table("MsEmployee")]
    public class Employee
    {
        [Key]
        [Column("employee_id")]
        [MaxLength(36)]
        public string EmployeeId { get; set; }

        [Required]
        [Column("name")]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        [Column("position")]
        [MaxLength(4000)]
        public string Position { get; set; }

        [Required]
        [Column("email")]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [Column("phone_number")]
        [MaxLength(36)]
        public string PhoneNumber { get; set; }

        public ICollection<Maintenance> Maintenances { get; set; }
    }
}
