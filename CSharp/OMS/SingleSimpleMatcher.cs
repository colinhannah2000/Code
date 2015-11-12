using System;
using System.Collections.Generic;
using System.Linq;

namespace ETS.OMS
{
    /// <summary>
    /// Naive implementation. Not thread safe.
    /// </summary>
    public class SingleSimpleMatcher : IMatcher
    {
        private SortedDictionary<decimal, List<IOrder>> _buyPriceOrders = new SortedDictionary<decimal, List<IOrder>>();
        private SortedDictionary<decimal, List<IOrder>> _sellPriceOrders = new SortedDictionary<decimal, List<IOrder>>();

        public List<ITradeReport> AddOrder(IOrder order)
        {
            List<ITradeReport> trades = null;

            var oppositeBook = order.Side == Side.Buy ? _sellPriceOrders : _buyPriceOrders;

            ulong remainingVolume = order.Volume;

            // Find a match on the opposite side.
            if (oppositeBook.ContainsKey(order.Price))
            {                
                while (remainingVolume > 0 && oppositeBook.Keys.Count > 0)
                {
                    decimal bestPrice = oppositeBook.First().Key;
                    bool match = order.Side == Side.Buy ? bestPrice <= order.Price : bestPrice >= order.Price;
                    if (!match) break;

                    ulong tradedVolume;

                    if (trades == null) trades = new List<ITradeReport>();

                    trades.Add(TradeTopOrder(oppositeBook, remainingVolume, order.Id, out tradedVolume));
                    remainingVolume -= tradedVolume;
                }
            }

            // New order not traded out, add to side.
            if (remainingVolume > 0)
            {
                var book = order.Side == Side.Sell ? _sellPriceOrders : _buyPriceOrders;
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
            SortedDictionary<decimal, List<IOrder>> book, 
            ulong volume, 
            ulong matchingOrderId, 
            out ulong tradedVolume)
        {
            // Always trade against the top order.
            var orders = book.First().Value;
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

            return trade;
        }

        //private void RemoveOrder()
    }
}
