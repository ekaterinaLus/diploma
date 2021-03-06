﻿using DataStore.Entities;
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

                configuration.CreateMap<TagViewModel, Tag>();

                configuration.CreateMap<OrganizationViewModel, Company>();

                configuration.CreateMap<OrganizationViewModel, University>();

                configuration.CreateMap<IOrganization, OrganizationViewModel>()
                    .ForMember(
                        dest => dest.Type,
                        opt => opt.MapFrom(
                            src => opt is Company ? OrganizationViewModel.OrganizationType.Company : OrganizationViewModel.OrganizationType.University)
                    );

                configuration.CreateMap<ProjectViewModel, Project>()
                    .ForMember(
                        dest => dest.Tags,
                        opt => opt.MapFrom(
                            src => src.Tags
                                .Select(tag => new ProjectsTags()
                                {
                                    ProjectId = src.Id,
                                    TagId = tag.Id
                                })))
                    .ForMember(
                        dest => dest.Initializer,
                        opt => opt.MapFrom(
                            src => src.Initializer.ToType<University>()))
                    .ForMember(
                        dest => dest.Sponsors, //Куда (у объекта приемника, того что справа)
                        opt => opt.MapFrom( //Каким образом преобразование
                            src => src.Sponsors //Поле объекта источника, того что слева
                                .Select(company => new ProjectsCompanies()
                                {
                                    CompanyId = company.Id,
                                    ProjectId = src.Id
                                })));

                configuration.CreateMap<ProjectViewHistory, ProjectViewHistoryViewModel>()
                    .ForMember(
                        dest => dest.ContactInformation,
                        opt => opt.MapFrom(
                            src => src.Company.ContactInformation))
                    .ForMember(
                        dest => dest.OrganizationName,
                        opt => opt.MapFrom(
                            src => src.Company.Name))
                    .ForMember(
                        dest => dest.ProjectName,
                        opt => opt.MapFrom(
                            src => src.Project.Name));
            });
        }

        public static TTarget Create<TTarget>(object source)
        {
            return AutoMapper.Mapper.Instance.Map<TTarget>(source);
        }
    }
}
