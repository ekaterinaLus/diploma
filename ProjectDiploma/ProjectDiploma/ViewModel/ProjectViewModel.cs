using SharedLogic.Mapper;
using System;
using System.Collections.Generic;

namespace ProjectDiploma.ViewModel
{
    public class ProjectViewModel : IMappable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Risks { get; set; }

        public string Stage { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? FinishDate { get; set; }

        public decimal CostCurrent { get; set; }

        public decimal CostFull { get; set; }

        public DateTime Date { get; set; }

        public bool IsClosed { get; set; }

        public string FileName { get; set; }

        public OrganizationViewModel Initializer { get; set; }

        public HashSet<OrganizationViewModel> Sponsors { get; set; }

        public HashSet<TagViewModel> Tags { get; set; } 
    }
}
