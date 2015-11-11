using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Builder;
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
            builder.RegisterType<Configuration.Configuration>().As<IConfiguration>();

            IContainer container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var configuration = scope.Resolve<IConfiguration>();
                configuration.Load(ConfigurationManager.AppSettings["Markets"]);
            }
        }
    }
}
