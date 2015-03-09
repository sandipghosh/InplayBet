
namespace InplayBet.Web.MapperConfig
{
    using System;
    using AutoMapper;
    using InplayBet.Web.Data.Implementation;
    using InplayBet.Web.Data.Interface;
    using InplayBet.Web.Utilities;
    using SimpleInjector;
    using SimpleInjector.Advanced;
    using SimpleInjector.Packaging;

    public class DataServiceLocatorConfig : IPackage
    {
        /// <summary>
        /// Registers the set of services in the specified <paramref name="container" />.
        /// </summary>
        /// <param name="container">The container the set of services is registered into.</param>
        public void RegisterServices(Container container)
        {
            try
            {
                this.RegisterServiceLocator(container);
                this.AddMapperProfile(container);
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(container);
            }
        }

        /// <summary>
        /// Registers the service locator.
        /// </summary>
        /// <param name="container">The container.</param>
        private void RegisterServiceLocator(Container container)
        {
            try
            {
                container.Register<IUserDataRepository, UserDataRepository>();
                container.Register<IBookMakerDataRepository, BookMakerDataRepository>();
                container.Register<IBetDataRepository, BetDataRepository>();
                container.Register<IChallengeDataRepository, ChallengeDataRepository>();
                container.Register<ILegueDataRepository, LegueDataRepository>();
                container.Register<ITeamDataRepository, TeamDataRepository>();
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(container);
            }
        }

        /// <summary>
        /// Adds the mapper profile.
        /// </summary>
        /// <param name="container">The container.</param>
        private void AddMapperProfile(Container container)
        {
            try
            {
                container.AppendToCollection(typeof(Profile),
                    Lifestyle.Singleton.CreateRegistration(typeof(Profile),
                    typeof(EntityMapperConfig), container));
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(container);
            }
        }
    }
}