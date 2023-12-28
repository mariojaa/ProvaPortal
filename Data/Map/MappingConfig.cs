using AutoMapper;

namespace ProvaPortal.Data.Map
{
    public static class MappingConfig
    {
        public static IMapper Initialize()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            return config.CreateMapper();
        }
    }
}
