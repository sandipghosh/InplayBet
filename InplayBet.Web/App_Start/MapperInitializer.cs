
namespace InplayBet.Web
{
    using AutoMapper;
    using InplayBet.Web.Utilities;
    using SimpleInjector;
    using System;
    using System.Collections.Generic;

    public class MapperInitializer
    {
        /// <summary>
        /// Registers the mapper.
        /// </summary>
        /// <param name="container">The container.</param>
        public static void RegisterMapper(Container container)
        {
            try
            {
                AutoMapper.Mapper.Initialize(mapper =>
                {
                    IEnumerable<Profile> profiles = container.GetAllInstances<Profile>();
                    foreach (Profile profile in profiles)
                    {
                        mapper.AddProfile(profile);
                    }
                });

                Mapper.AssertConfigurationIsValid();
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(container);
            }
        }
    }
}