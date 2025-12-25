using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentCar_BE.Models
{
    [Table("TrMaintenance")]
    public class Maintenance
    {
        [Key]
        [Column("maintenance_id")]
        [MaxLength(36)]
        public string MaintenanceId { get; set; }

        [Required]
        [Column("maintenance_date")]
        public DateTime MaintenanceDate { get; set; }

        [Required]
        [Column("decription")]
        [MaxLength(4000)]
        public string Description { get; set; }

        [Required]
        [Column("cost")]
        public decimal Cost { get; set; }

        [Required]
        [Column("car_id")]
        [MaxLength(36)]
        public string CarId { get; set; }

        [Required]
        [Column("employee_id")]
        [MaxLength(36)]
        public string EmployeeId { get; set; }

        public Car Car { get; set; }
        public Employee Employee { get; set; }
    }
}
