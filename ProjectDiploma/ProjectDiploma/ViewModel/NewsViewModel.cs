using SharedLogic.Mapper;
using System;
using System.Collections.Generic;

namespace ProjectDiploma.ViewModel
{
    public class NewsViewModel : IMappable
    {        
        public int Id { get; set; }

        public string Header { get; set; }

        public string Annotation { get; set; }

        public DateTime Date { get; set; }

        public string Text { get; set; }

        public int? SectionId { get; set; }

        public HashSet<TagViewModel> Tags { get; set; }
    }
}
