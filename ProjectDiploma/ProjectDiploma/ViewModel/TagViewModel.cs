using AutoMapper;
using DataStore.Entities;

namespace ProjectDiploma.ViewModel
{
    public class TagViewModel
    {
        private readonly static IMapper _toDtoMapper;

        static TagViewModel()
        {
            var toDtoConfig = new MapperConfiguration(cfg => cfg.CreateMap<Tag, TagViewModel>());
            _toDtoMapper = toDtoConfig.CreateMapper();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public static TagViewModel FromDbObject(Tag tag)
        {
            return _toDtoMapper.Map<TagViewModel>(tag);
        }
    }
}
