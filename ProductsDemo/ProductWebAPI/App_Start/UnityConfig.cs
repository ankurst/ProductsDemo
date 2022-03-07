using Products.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace ProductWebAPI
{
    public static class UnityConfig
    {
        private static readonly IUnityContainer Container = new UnityContainer();

        private static void RegisterRepositories()
        {
            Container.RegisterType<IProductDataprovider, ProductDataProvider>();
        }

        public static void RegisterComponents()
        {
            RegisterRepositories();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(Container);
        }
    }
}