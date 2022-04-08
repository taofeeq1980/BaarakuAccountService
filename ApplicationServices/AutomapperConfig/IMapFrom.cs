using AutoMapper;

namespace ApplicationServices.AutomapperConfig
{
    public interface IMapFrom<T>
    {
        void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }
}
