using System.ComponentModel.DataAnnotations;

namespace MOTM.Models
{
    public class Registration
    {
        [Required, StringLength(30, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long", MinimumLength = 10)]
        [DataType(DataType.Password), Display(Name = "Password")]
        public string Password { get; set; }
        [DataType(DataType.Password), Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }
        [Required, StringLength(40), EmailAddress]
        public string EMail { get; set; }
        [Required, StringLength(30)]
        public string FirstName { get; set; }
        [Required, StringLength(30)]
        public string LastName { get; set; }
        [Required, Phone]
        public string PhoneNumber { get; set; }
        [Required, StringLength(30)]
        public string FirstAddressLine { get; set; }
        [StringLength(30)]
        public string? SecondAddressLine { get; set; }
        [StringLength(30)]
        public string? ThirdAddressLine { get; set; }
        [Required, StringLength(30)]
        public string PostTown { get; set; }
        [Required, StringLength(8)]
        public string PostCode { get; set; }
    }
}
