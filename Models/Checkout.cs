using System.ComponentModel.DataAnnotations;

namespace MOTM.Models
{
    public class Checkout
    {
        [Required]
        public string Date { get; set; }
        [Required]
        public string Time { get; set; }
    }
}
