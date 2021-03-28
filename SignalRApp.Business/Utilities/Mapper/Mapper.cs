using System.Collections.Generic;
using AutoMapper;


namespace SignalRApp.Business.Utilities.Mapper
{
    public class Mapper : IMapper
    {
        private readonly AutoMapper.IMapper _mapper;

        public Mapper()
        {
            var profiles = new List<Profile>
            {
                new Profiles()
            };

            var config = new MapperConfiguration(config =>
            {
                foreach (var profile in profiles)
                {
                    config.AddProfile(profile);
                }
            });

            _mapper = config.CreateMapper();
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return _mapper.Map<TSource, TDestination>(source);
        }
    }
}