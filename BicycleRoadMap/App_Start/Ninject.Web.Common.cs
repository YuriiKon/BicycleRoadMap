[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(BicycleRoadMap.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(BicycleRoadMap.App_Start.NinjectWebCommon), "Stop")]

namespace BicycleRoadMap.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using global::Ninject;
    using global::Ninject.Web.Common;
    using global::Ninject.Modules;
    using global::Ninject.Web.Common.WebHost;
    using System.Web.Http;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        private static void RegisterServices(IKernel kernel)
        {
            GlobalConfiguration.Configuration.DependencyResolver = new Ninject.NinjectHttpDependencyResolver(kernel);
            System.Web.Mvc.DependencyResolver.SetResolver(new Ninject.NinjectDependencyResolver(kernel));
        }        
    }
}