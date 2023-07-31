using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Mgm.VI.Data.Dto;
using Mgm.VI.Common;

namespace Mgm.VI.Data.AutoMapper
{
    public class Bootstrapper
    {
        private Mapper _mapper;
        public bool MapperIsSet {get; private set;}
        public ErrorMessages Errors { get; private set; }

        #region Private Constructor & Methods
        private void Initialize()
        {
            Errors = new ErrorMessages();
            MapperIsSet = false;
        }
        private Bootstrapper(bool suppressExceptions = true)
        {
            Initialize();
        }
        private static Mapper ConfigureBootstrapper(bool suppressExceptions = true)
        {
            return ConfigureBootstrapper(cfg => cfg.CreateMap<OrderDto, OrderDto>(), suppressExceptions);
        }
        private static Mapper ConfigureBootstrapper(Action<IMapperConfigurationExpression> configExpression, bool suppressExceptions = true)
        {
            MapperConfiguration config = null;
            try
            {
                config = new MapperConfiguration(configExpression);
            }
            catch
            {
                config = null;
                if (!suppressExceptions) throw;
            }
            return ConfigureBootstrapper(config);
        }
        private static Mapper ConfigureBootstrapper(MapperConfiguration config, bool suppressExceptions = true)
        {
            Mapper mapper = null;
            try
            {
                mapper = new Mapper(config);
            }
            catch
            {
                mapper = null;
                if (!suppressExceptions) throw;
            }
            return mapper;
        }
        #endregion Private Constructor & Methods

        #region Instantiators
        public static Bootstrapper New(bool suppressExceptions = true)
        {
            return new Bootstrapper(suppressExceptions);
        }
        public static Bootstrapper New(Action<IMapperConfigurationExpression> configExpression, bool suppressExceptions = true)
        {
            Bootstrapper instance = new Bootstrapper(suppressExceptions);

            try
            {
                var mapper = ConfigureBootstrapper(configExpression, suppressExceptions);
                if (mapper != null)
                    instance.SetMapper(mapper);
                else
                    instance = null;
            }
            catch (Exception e)
            {
                instance.Errors.Add("Mgm.VI.Data.AutoMapper::New", e.Message, e);
            }
            return instance;
        }
        public static Bootstrapper New(MapperConfiguration config, bool suppressExceptions = true)
        {
            Bootstrapper instance = new Bootstrapper();
            try
            {
                var mapper = ConfigureBootstrapper(config, suppressExceptions);
                if (mapper != null)
                    instance.SetMapper(mapper);
                else
                    instance = null;
            }
            catch (Exception e)
            {
                instance.Errors.Add("Mgm.VI.Data.AutoMapper::New", e.Message, e);
            }
            return instance;
        }
        #endregion Instantiators

        #region SetMapper
        public static IMapper CreateDtoMaps()
        {
            Action<IMapperConfigurationExpression> MapperConfigureAction = cfg =>
            {
                cfg.AddProfile(new DtoMappingProfile());
            };

            var config = new MapperConfiguration(MapperConfigureAction);
            var mapper = config.CreateMapper();
            return mapper;
        }
        public void SetMapper (Mapper mapper, bool suppressExceptions = true)
        {
            this._mapper = mapper;
            MapperIsSet = true;
        }
        public void SetMapper (Action<IMapperConfigurationExpression> configExpression, bool suppressExceptions = true)
        { 
            SetMapper(ConfigureBootstrapper(configExpression, suppressExceptions));
        }
        public void SetMapper(MapperConfiguration config, bool suppressExceptions = true)
        {
            SetMapper(ConfigureBootstrapper(config, suppressExceptions));
        }
        #endregion SetMapper
    }
}
