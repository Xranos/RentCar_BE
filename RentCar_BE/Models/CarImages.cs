using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentCar_BE.Models
{
    [Table("MsCarImages")]
    public class CarImage
    {
        [Key]
        [Column("image_car_id")]
        [MaxLength(36)]
        public string ImageCarId { get; set; }

        [Required]
        [Column("car_id")]
        [MaxLength(36)]
        public string CarId { get; set; }

        [Required]
        [Column("image_link")]
        [MaxLength(2000)]
        public string ImageLink { get; set; }

        public Car Car { get; set; }
    }
}
