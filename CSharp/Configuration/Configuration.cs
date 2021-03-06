﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ETS.Configuration
{
    public class Configuration : IConfiguration
    {
        private class Market : IMarket
        {
            public string Symbol { get; internal set; }
            public string Name { get; internal set; }
            public string Industry { get; internal set; }
            public ulong Capitalisation { get; internal set; }
            public decimal Weight { get; internal set; }
        }

        public Configuration(string marketsFileLocation)
        {
            Load(marketsFileLocation);
        }

        public Dictionary<int, IMarket> Markets { get; private set; }

        public void Load(string marketsFile)
        {
            LoadMarkets(marketsFile);
        }

        private void LoadMarkets(string marketsFile)
        {
            StreamReader marketsReader = new StreamReader(marketsFile);
            string allMarkets = marketsReader.ReadToEnd();

            var markets = allMarkets.Split(new char[] { '\n' });

            var marketIndexes = Enumerable.Range(0, markets.Length);

            int marketId = 0;
            Markets = new Dictionary<int, IMarket>();
            allMarkets.Split(new char[] { '\n' }).ToList().ForEach(
                market =>
                {
                    var split = market.Split(new char[] { '\t' }).ToArray();
                    Markets.Add(marketId++, new Market()
                    {
                        Symbol = split[0],
                        Name = split[1],
                        Industry = split[2],
                        Capitalisation = Convert.ToUInt64(split[3].Replace(",", "")),
                        Weight = Convert.ToDecimal(split[4])
                    });
                });
        }
    }
}
