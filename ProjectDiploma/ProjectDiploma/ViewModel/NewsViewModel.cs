using AutoMapper;
using DataStore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDiploma.ViewModel
{
    public class NewsViewModel
    {
        private readonly static IMapper _toDtoMapper;
        //private readonly static IMapper _fromDtoMapper;

        static NewsViewModel()
        {
            var toDtoConfig = new MapperConfiguration(cfg =>
                cfg.CreateMap<News, NewsViewModel>()
                    .ForMember(
                        dest => dest.Tags,
                        opt => opt.MapFrom(src => src.Tags.Select(obj => TagViewModel.FromDbObject(obj.Tags))))
                        );
            _toDtoMapper = toDtoConfig.CreateMapper();
        }
        public int Id { get; set; }

        public string Header { get; set; }

        public string Annotation { get; set; }

        public DateTime Date { get; set; }

        public string Text { get; set; }

        public int? SectionId { get; set; }


        public HashSet<TagViewModel> Tags { get; set; }

        public static NewsViewModel FromDbObject(News @news)
        {
            return _toDtoMapper.Map<NewsViewModel>(@news);
        }
    }
}
