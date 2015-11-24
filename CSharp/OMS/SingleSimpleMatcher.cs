using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace ETS.OMS
{
    /// <summary>
    /// Naive implementation. Not thread safe.
    /// </summary>
    public class SingleSimpleMatcher : IMatcher
    {
        private class ForwardDecimal : IComparer<decimal>
        {
            public int Compare(decimal x, decimal y)
            {
                return y.CompareTo(x);
            }
        }

        private class ReverseDecimal : IComparer<decimal>
        {
            public int Compare(decimal x, decimal y)
            {
                return x.CompareTo(y);
            }
        }

        // Buy ordered high to low.
        private SortedDictionary<decimal, List<IOrder>> _buyPriceOrders = new SortedDictionary<decimal, List<IOrder>>(new ForwardDecimal());
        // Sell ordered low to high.
        private SortedDictionary<decimal, List<IOrder>> _sellPriceOrders = new SortedDictionary<decimal, List<IOrder>>(new ReverseDecimal());

        private delegate bool CompareOrder(decimal a, decimal b);

        private CompareOrder _sellCompare;
        private CompareOrder _buyCompare;

        public SingleSimpleMatcher()
        {
            _sellCompare = (a, b) => a >= b;
            _buyCompare = (a, b) => a <= b;
        }

        public List<ITradeReport> AddOrder(IOrder order)
        {
            Debug.Assert(order != null);

            List<ITradeReport> trades = null;

            SortedDictionary<decimal, List<IOrder>> book;
            SortedDictionary<decimal, List<IOrder>> oppositeBook;
            CompareOrder orderCompare;
            if (order.Side == Side.Buy)
            {
                oppositeBook = _sellPriceOrders;
                book = _buyPriceOrders;
                orderCompare = _buyCompare;
            }
            else                                                                  
            {
                oppositeBook = _buyPriceOrders;
                book = _sellPriceOrders;
                orderCompare = _sellCompare;
            }

            Debug.Assert(order.Volume > 0, "Order of no volume not supported");
            ulong remainingVolume = order.Volume;

            bool matchOrder = false;
            do
            {
                // Find a matching or better price and an order with volume.
                var goodPrices = oppositeBook.Keys.ToList().Where(key => orderCompare(key, order.Price) && oppositeBook[key].Count() > 0);
                matchOrder = goodPrices.Count() > 0;
                if (matchOrder)
                {
                    decimal bestPrice = goodPrices.First();
                    if (trades == null) trades = new List<ITradeReport>();

                    ulong tradedVolume;
                    trades.Add(TradeTopOrder(bestPrice, oppositeBook, order.Id, ref remainingVolume, out tradedVolume));                    
                }
            } while (remainingVolume > 0 && matchOrder);

            order.Volume = remainingVolume;

            // New order not traded out, add to side.
            if (remainingVolume > 0)
            {
                List<IOrder> orders;
                if (!book.ContainsKey(order.Price))
                {
                    orders = new List<IOrder>();
                    book.Add(order.Price, orders);
                }
                else
                {
                    orders = book[order.Price];
                }

                orders.Add(order);
            }

            return trades;
        }

        private static ITradeReport TradeTopOrder(
            decimal price,
            SortedDictionary<decimal, List<IOrder>> book,
            ulong matchingOrderId,
            ref ulong volume,
            out ulong tradedVolume)
        {
            Debug.Assert(book.ContainsKey(price), "book does not contain price.");

            // Always trade against the top order.
            var orders = book[price];

            Debug.Assert(orders.Count > 0, "book at price does not contain orders.");
            IOrder matchedOrder = orders[0];

            // Top order traded out.
            if (matchedOrder.Volume <= volume)
            {
                tradedVolume = matchedOrder.Volume;
                matchedOrder.Volume = 0;
                orders.RemoveAt(0);
            }
            else // Top order partially traded, new order remains.
            {
                tradedVolume = volume;
                matchedOrder.Volume -= volume;
            }

            ITradeReport trade = new TradeReport
            {
                Price = matchedOrder.Price,
                OrderId1 = matchingOrderId,
                OrderId2 = matchedOrder.Id,
                Volume = tradedVolume
            };

            volume -= tradedVolume;

            return trade;
        }

        //private void RemoveOrder()
    }
}
