using SharedLogic.Mapper;

namespace ProjectDiploma.ViewModel
{
    public class OrganizationViewModel : IMappable
    {
        public enum OrganizationType
        {
            Company,
            University
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string ContactInformation { get; set; }

        public OrganizationType Type { get; set; }
    }
}
