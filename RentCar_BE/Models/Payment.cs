using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentCar_BE.Models
{
    [Table("LtPayment")]
    public class Payment
    {
        [Key]
        [Column("payment_id")]
        [MaxLength(36)]
        public string PaymentId { get; set; }

        [Required]
        [Column("payment_date")]
        public DateTime PaymentDate { get; set; }

        [Required]
        [Column("amount")]
        public decimal Amount { get; set; }

        [Required]
        [Column("payment_method")]
        [MaxLength(100)]
        public string PaymentMethod { get; set; }

        [Required]
        [Column("rental_id")]
        [MaxLength(36)]
        public string RentalId { get; set; }

        public Rental Rental { get; set; }
    }
}
