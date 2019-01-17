using SharedLogic.Mapper;
using System;
using System.Collections.Generic;

namespace ProjectDiploma.ViewModel
{
    public class EventViewModel: IMappable
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Adress { get; set; }

        public decimal? Cost { get; set; }

        public HashSet<TagViewModel> Tags { get; set; }
    }
}
