using System.ComponentModel.DataAnnotations;

namespace MOTM.Models
{
    public class SignIn
    {
        [Required, EmailAddress]
        public string EMail { get;set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
