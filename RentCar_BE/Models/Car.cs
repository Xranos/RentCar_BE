using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentCar_BE.Models
{
    [Table("MsCar")]
    public class Car
    {
        [Key]
        [Column("car_id")]
        [MaxLength(36)]
        public string CarId { get; set; }

        [Required]
        [Column("name")]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        [Column("model")]
        [MaxLength(100)]
        public string Model { get; set; }

        [Required]
        [Column("year")]
        public int Year { get; set; }

        [Required]
        [Column("license_plate")]
        [MaxLength(50)]
        public string LicensePlate { get; set; }

        [Required]
        [Column("price_per_day")]
        public decimal PricePerDay { get; set; }

        [Required]
        [Column("status")]
        public bool Status { get; set; }

        [Required]
        [Column("transmission")]
        [MaxLength(100)]
        public string Transmission { get; set; }

        [Required]
        [Column("number_of_car_seats")]
        public int NumberOfCarSeats { get; set; }

        public ICollection<Rental> Rentals { get; set; }
        public ICollection<CarImage> CarImages { get; set; }
        public ICollection<Maintenance> Maintenances { get; set; }
    }
}
