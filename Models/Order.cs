using System.ComponentModel.DataAnnotations;

namespace MOTM.Models
{
    public class Order
    {
        [Required, StringLength(450)]
        public string CustomerID { get; set; }
        [Required]
        public long Services { get; set; }
        [Required]
        public DateTime TimeSlot { get; set; }
        [Required]
        public DateTime OrderedTime { get; set; }
        [Required]
        public bool Fulfilled { get; set; }
    }
}
