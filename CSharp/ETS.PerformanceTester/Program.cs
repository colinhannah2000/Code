using Autofac;
using ETS.Configuration;
using System.Configuration;
using ETS.OMS;

namespace ETS.PerformanceTester
{
    class Program
    {
        static void Main(string[] args)
        {
            ContainerBuilder builder = new ContainerBuilder();
            Configuration.Configuration.Build(builder, ConfigurationManager.AppSettings["Markets"]);

            builder.RegisterType<SingleSimpleMatcher>().As<IMatcher>();
            builder.RegisterType<OmsFactory>().As<IOmsFactory>();

            IContainer container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var configuration = scope.Resolve<IConfiguration>();
                var orders = scope.Resolve<IOmsFactory>();
                var matcher = scope.Resolve<IMatcher>();

                TestMatcher(orders, matcher);
            }
        }

        private static void TestMatcher(IOmsFactory orders, IMatcher matcher)
        {
            var trades1 = matcher.AddOrder(orders.CreateOrder(10,Side.Buy,100));
            var trades2 = matcher.AddOrder(orders.CreateOrder(10, Side.Sell, 100));
        }
    }
}
