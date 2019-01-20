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
                        opt => opt.MapFrom(src => src.Tags.Select(obj => obj.Tag.ToType<TagViewModel>())));

                configuration.CreateMap<News, NewsViewModel>()
                    .ForMember(
                        dest => dest.Tags,
                        opt => opt.MapFrom(src => src.Tags.Select(obj => obj.Tag.ToType<TagViewModel>())));

                configuration.CreateMap<Tag, TagViewModel>();

                configuration.CreateMap<Project, ProjectViewModel>()
                    .ForMember(
                        dest => dest.Tags,
                        opt => opt.MapFrom(
                            src => src.Tags
                                .Select(obj => obj.Tag.ToType<TagViewModel>())))
                    .ForMember(
                        dest => dest.Sponsors,
                        opt => opt.MapFrom(
                            src => src.Sponsors
                            .Select(obj => obj.Company.ToType<OrganizationViewModel>())))
                    .ForMember(
                        dest => dest.Initializer,
                        opt => opt.MapFrom(
                            src => src.Initializer
                            .ToType<OrganizationViewModel>()));

                configuration.CreateMap<Company, OrganizationViewModel>()
                    .ForMember(
                        dest => dest.Type,
                        opt => opt.MapFrom(src => OrganizationViewModel.OrganizationType.Company));

                configuration.CreateMap<University, OrganizationViewModel>()
                    .ForMember(
                        dest => dest.Type,
                        opt => opt.MapFrom(src => OrganizationViewModel.OrganizationType.University));

            });
        }

        public static TTarget Create<TTarget>(object source)
        {
            return AutoMapper.Mapper.Instance.Map<TTarget>(source);
        }
    }
}
