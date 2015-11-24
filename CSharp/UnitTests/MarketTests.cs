using Autofac;
using ETS.Configuration;
using System.Configuration;
using ETS.OMS;
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class MarketTests
    {
        public void Initialise(out IContainer container)
        {
            string location = @"C:\Users\colin\Code\Data\asx200Weights.txt";
            ContainerBuilder builder = new ContainerBuilder();

            builder.Register(c => new Configuration(location)).As<IConfiguration>();

            builder.RegisterType<SingleSimpleMatcher>().As<IMatcher>();
            builder.RegisterType<OmsFactory>().As<IOmsFactory>().SingleInstance();

            container = builder.Build();

            //using (var scope = container.BeginLifetimeScope())
            //{
            //    var configuration = scope.Resolve<IConfiguration>();
            //    orders = scope.Resolve<IOmsFactory>();
            //    matcher = scope.Resolve<IMatcher>();
            //}
        }

        [TestMethod]
        public void TestMatcher()
        {
            IContainer container;
            Initialise(out container);

            using (var scope = container.BeginLifetimeScope())
            {
                //var configuration = scope.Resolve<IConfiguration>();
                var orders = scope.Resolve<IOmsFactory>();
                var matcher = scope.Resolve<IMatcher>();

                var trades = matcher.AddOrder(orders.CreateOrder(10, Side.Buy, 100));
                Assert.IsTrue(trades == null);
                trades = matcher.AddOrder(orders.CreateOrder(10, Side.Sell, 100));
                Assert.IsTrue(trades.Count == 1 && trades[0].Price == 10 && trades[0].Volume == 100);
                
                // Trade volume over multiple orders.
                trades = matcher.AddOrder(orders.CreateOrder(10, Side.Buy, 50));
                Assert.IsTrue(trades == null);
                trades = matcher.AddOrder(orders.CreateOrder(10, Side.Buy, 100));
                Assert.IsTrue(trades == null);
                trades = matcher.AddOrder(orders.CreateOrder(9, Side.Sell, 100));
                Assert.IsTrue(trades.Count == 2 && trades[0].Price == 10 && trades[0].Volume == 50);
                Assert.IsTrue(trades[1].Price == 10 && trades[1].Volume == 50);
                trades = matcher.AddOrder(orders.CreateOrder(8, Side.Sell, 50));
                Assert.IsTrue(trades[0].Price == 10 && trades[0].Volume == 50);
                trades = matcher.AddOrder(orders.CreateOrder(8, Side.Sell, 50));
                Assert.IsTrue(trades == null);
                trades = matcher.AddOrder(orders.CreateOrder(8, Side.Buy, 50));
                Assert.IsTrue(trades[0].Price == 8 && trades[0].Volume == 50);

                // Don't trade over multiple but only top order.
                trades = matcher.AddOrder(orders.CreateOrder(8, Side.Buy, 100));
                Assert.IsTrue(trades == null);
                trades = matcher.AddOrder(orders.CreateOrder(8, Side.Buy, 20));
                Assert.IsTrue(trades == null);
                trades = matcher.AddOrder(orders.CreateOrder(7, Side.Sell, 20));
                Assert.IsTrue(trades.Count == 1 && trades[0].Price == 8 && trades[0].Volume == 20);
                trades = matcher.AddOrder(orders.CreateOrder(7, Side.Sell, 100));
                Assert.IsTrue(trades.Count == 2 && trades[0].Price == 8 && trades[0].Volume == 80);
                Assert.IsTrue(trades.Count == 2 && trades[1].Price == 8 && trades[1].Volume == 20);

                // Only trade on price overlap.
                trades = matcher.AddOrder(orders.CreateOrder(9, Side.Buy, 100));
                Assert.IsTrue(trades == null);
                trades = matcher.AddOrder(orders.CreateOrder(11, Side.Sell, 100));
                Assert.IsTrue(trades == null);
                trades = matcher.AddOrder(orders.CreateOrder(9.5m, Side.Buy, 100));
                Assert.IsTrue(trades == null);
                trades = matcher.AddOrder(orders.CreateOrder(10.5m, Side.Sell, 100));
                Assert.IsTrue(trades == null);
                trades = matcher.AddOrder(orders.CreateOrder(9.9m, Side.Buy, 100));
                Assert.IsTrue(trades == null);
                trades = matcher.AddOrder(orders.CreateOrder(10.1m, Side.Sell, 100));
                Assert.IsTrue(trades == null);
                trades = matcher.AddOrder(orders.CreateOrder(10, Side.Buy, 100));
                Assert.IsTrue(trades == null);
                // Tests maintenance of matcher order book price lists and trading best orders in order.
                trades = matcher.AddOrder(orders.CreateOrder(8, Side.Sell, 400)); // match all buys
                Assert.IsTrue(trades.Count == 4);
                Assert.IsTrue(trades[0].Price == 10 && trades[0].Volume == 100);
                Assert.IsTrue(trades[1].Price == 9.9m && trades[1].Volume == 100);
                Assert.IsTrue(trades[2].Price == 9.5m && trades[2].Volume == 100);
                Assert.IsTrue(trades[3].Price == 9 && trades[3].Volume == 100);
                trades = matcher.AddOrder(orders.CreateOrder(12, Side.Buy, 400)); // match all sells
                Assert.IsTrue(trades.Count == 3);
                Assert.IsTrue(trades[0].Price == 10.1m && trades[0].Volume == 100);
                Assert.IsTrue(trades[1].Price == 10.5m && trades[1].Volume == 100);
                Assert.IsTrue(trades[2].Price == 11 && trades[2].Volume == 100);
            }
        }
    }
}
