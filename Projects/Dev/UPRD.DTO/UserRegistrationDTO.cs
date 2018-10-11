using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPRD.DTO
{
    public class UserRegistrationDTO
    {
        public string Id { get; set; }

        [Display(Name = "Shipper")]
        public string Shipper { get; set; }

        [Required]
        [Display(Name = "Shipper DUNS")]
        public string ShipperDuns { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "User Type")]
        public string UserType { get; set; }
        [Display(Name = "Is Active")]
        public bool? IsActive { get; set; }
    }
}
