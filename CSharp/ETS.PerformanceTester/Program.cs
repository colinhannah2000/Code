using Autofac;
using ETS.Configuration;
using System.Configuration;
using ETS.OMS;
using System;
using System.Collections.Generic;

namespace ETS.PerformanceTester
{
    class Program
    {
        static void Main(string[] args)
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.Register(c => new Configuration.Configuration(ConfigurationManager.AppSettings["Markets"])).As<IConfiguration>();

            builder.RegisterType<SingleSimpleMatcher>().As<IMatcher>();
            builder.RegisterType<OmsFactory>().As<IOmsFactory>().SingleInstance();

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

            var tests = new List<Tuple<decimal, Side, ulong>>
            {
                new Tuple<decimal, Side, ulong>(10,Side.Buy,100),
                new Tuple<decimal, Side, ulong>(10,Side.Buy,100),
                new Tuple<decimal, Side, ulong>(10,Side.Buy,100),
                new Tuple<decimal, Side, ulong>(10,Side.Buy,100),
                new Tuple<decimal, Side, ulong>(10,Side.Buy,100),
                new Tuple<decimal, Side, ulong>(10,Side.Buy,100),
            };
        }
    }
}
