using Autofac;
using ETS.Configuration;
using System.Configuration;

namespace ETS.BasicMarket
{
    class Program
    {
        static void Main(string[] args)
        {
            ContainerBuilder builder = new ContainerBuilder();
            Configuration.Configuration.Build(builder, ConfigurationManager.AppSettings["Markets"]);

            IContainer container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var configuration = scope.Resolve<IConfiguration>();
                //configuration.Load(marketsFileLocation);
            }
        }
    }
}
