using ProjectDiploma.Logic.Mapper;

namespace SharedLogic.Mapper
{
    public static class MapExtensions
    {
        public static T ToType<T>(this IMappable @this)
            where T: class
        {
            return ObjectMapper.Create<T>(@this);
        }
    }
}
