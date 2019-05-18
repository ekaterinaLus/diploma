using System.Collections.Generic;

namespace ProjectDiploma.ViewModel
{
    public class UserViewModel
    {
        public string Email { get; set; }
        public string Role { get; set; }
        public OrganizationViewModel Organization { get; set; }
    }
}
