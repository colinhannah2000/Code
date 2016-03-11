using System.Configuration;
using System.Linq;
using Autofac;
using ETS.Configuration;
using ETS.OMS;

namespace ETS.BasicMarket
{
    class Program
    {
        static void Main(string[] args)
        {
            ContainerBuilder builder = new ContainerBuilder();

            // builder.RegisterType<Configuration.Configuration>().As<IConfiguration>();
            builder.Register(c => new Configuration.Configuration(ConfigurationManager.AppSettings["Markets"]))
                .As<IConfiguration>().SingleInstance();

            builder.RegisterType<SingleSimpleMatcher>().As<IMatcher>();
            builder.RegisterType<OmsFactory>().As<IOmsFactory>().SingleInstance();
            builder.RegisterType<OrderMatcherMap>().As<IOrderMatcherMap>().SingleInstance();

            IContainer container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                // Create a matcher for each market.
                var configuration = scope.Resolve<IConfiguration>();
                var orderMatcherMap = scope.Resolve<IOrderMatcherMap>();
                configuration.Markets.Keys.ToList().ForEach(m => orderMatcherMap.Build(m, container.Resolve<IMatcher>()));
                //configuration.Load(marketsFileLocation);
            }
        }
    }
}
