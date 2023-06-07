using System.ComponentModel.DataAnnotations;

namespace MOTM.Models
{
    public class Service
    {
        [Key]
        public byte ID { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }
        [Required,StringLength(300)]
        public string Description { get; set; }
        [Required]
        public short Price { get; set; }
        [Required]
        public byte Duration { get; set; }
    }
}
