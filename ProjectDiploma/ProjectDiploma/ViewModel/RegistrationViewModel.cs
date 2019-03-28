using ProjectDiploma.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ProjectDiploma.ViewModel
{
    public class RegistrationViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [CheckRole]
        public string Role { get; set; }

        [Required]
        public OrganizationViewModel Organization {get; set;}
    }
}
