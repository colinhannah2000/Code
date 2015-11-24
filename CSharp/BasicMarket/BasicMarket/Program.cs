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


            // builder.RegisterType<Configuration.Configuration>().As<IConfiguration>();
            builder.Register(c => new Configuration.Configuration(ConfigurationManager.AppSettings["Markets"])).As<IConfiguration>();

            IContainer container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var configuration = scope.Resolve<IConfiguration>();
                //configuration.Load(marketsFileLocation);
            }
        }
    }
}
