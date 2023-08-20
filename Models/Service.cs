using System.ComponentModel.DataAnnotations;

namespace MOTM.Models
{
    public class Service
    {
        [Key]
        public byte ID { get; set; }
        [Required]
        public byte SortID { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }
        [Required,StringLength(300)]
        public string Description { get; set; }
        [Required, Range(0,300)]
        public short Price { get; set; }
        [Required, Range(0, 120)]
        public byte Duration { get; set; }
        [Required]
        public bool Active { get; set; }
    }
}
