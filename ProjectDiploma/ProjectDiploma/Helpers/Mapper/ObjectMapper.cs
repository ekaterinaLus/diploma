using DataStore.Entities;
using ProjectDiploma.ViewModel;
using SharedLogic.Mapper;
using System.Linq;

namespace ProjectDiploma.Logic.Mapper
{
    public class ObjectMapper
    {
        static ObjectMapper()
        {
            AutoMapper.Mapper.Initialize(configuration =>
            {
                configuration.CreateMap<Event, EventViewModel>()
                    .ForMember(
                        dest => dest.Tags,
                        opt => opt.MapFrom(src => src.Tags.Select(obj => obj.Tags.ToType<TagViewModel>())));

                configuration.CreateMap<News, NewsViewModel>()
                    .ForMember(
                        dest => dest.Tags,
                        opt => opt.MapFrom(src => src.Tags.Select(obj => obj.Tags.ToType<TagViewModel>())));

                configuration.CreateMap<Tag, TagViewModel>();

            });
        }

        public static TTarget Create<TTarget>(object source)
        {
            return AutoMapper.Mapper.Instance.Map<TTarget>(source);
        }
    }
}
