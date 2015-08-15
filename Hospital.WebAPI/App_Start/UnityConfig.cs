using System;
using Hospital.Data.Contexts;
using Hospital.Data.Entities;
using Hospital.Services;
using Hospital.Services.Interfaces;
using Hospital.Web.Common.UnityEx;
using Microsoft.Practices.Unity;
using Repository.Pattern.DataContext;
using Repository.Pattern.Ef6;
using Repository.Pattern.Repositories;
using Repository.Pattern.UnitOfWork;

namespace Hospital.WebAPI
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static readonly Lazy<IUnityContainer> Container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return Container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");

            container
                .RegisterType<IDataContextAsync, HospitalContext>(new PerRequestLifetimeManager())
                .RegisterType<IUnitOfWorkAsync, UnitOfWork>(new PerRequestLifetimeManager());

            RegisterRepositories(container);
            RegisterServices(container);
        }

        private static void RegisterServices(IUnityContainer container)
        {
            container.RegisterType<IPatientService, PatientService>();
        }

        private static void RegisterRepositories(IUnityContainer container)
        {
            container.RegisterType<IRepositoryAsync<Patient>, Repository<Patient>>()
                .RegisterType<IRepositoryAsync<PatientInfo>, Repository<PatientInfo>>()
                .RegisterType<IRepositoryAsync<PatientWeight>, Repository<PatientWeight>>();
        }
    }
}
