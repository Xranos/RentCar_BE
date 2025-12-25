using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentCar_BE.Models
{
    [Table("TrRental")]
    public class Rental
    {
        [Key]
        [Column("rental_id")]
        [MaxLength(36)]
        public string RentalId { get; set; }

        [Required]
        [Column("rental_date")]
        public DateTime RentalDate { get; set; }

        [Required]
        [Column("return_date")]
        public DateTime ReturnDate { get; set; }

        [Required]
        [Column("total_price")]
        public decimal TotalPrice { get; set; }

        [Required]
        [Column("payment_status")]
        public bool PaymentStatus { get; set; }

        [Required]
        [Column("customer_id")]
        [MaxLength(36)]
        public string CustomerId { get; set; }

        [Required]
        [Column("car_id")]
        [MaxLength(36)]
        public string CarId { get; set; }

        public Customer Customer { get; set; }
        public Car Car { get; set; }

        public ICollection<Payment> Payments { get; set; }
    }
}
