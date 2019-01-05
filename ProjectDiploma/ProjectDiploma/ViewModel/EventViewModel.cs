using AutoMapper;
using DataStore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDiploma.ViewModel
{
    public class EventViewModel
    {
        private readonly static IMapper _toDtoMapper;
        //private readonly static IMapper _fromDtoMapper;

        static EventViewModel()
        {
            var toDtoConfig = new MapperConfiguration(cfg => 
                cfg.CreateMap<Event, EventViewModel>()
                    .ForMember(
                        dest => dest.Tags, 
                        opt => opt.MapFrom(src => src.Tags.Select(obj => TagViewModel.FromDbObject(obj.Tags))))
                        );
            _toDtoMapper = toDtoConfig.CreateMapper();
        }

        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Adress { get; set; }

        public decimal? Cost { get; set; }

        public HashSet<TagViewModel> Tags { get; set; }

        public static EventViewModel FromDbObject(Event @event)
        {
            return _toDtoMapper.Map<EventViewModel>(@event);
        }

        //public static Event ToDbObject(EventViewModel @event)
        //{
        //    return _fromDtoMapper.Map<Event>(@event);
        //}
    }
}
