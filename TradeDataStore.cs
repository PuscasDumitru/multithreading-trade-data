
using System.Collections.Concurrent;

namespace multithreading_trade_data
{
    public static class TradeDataStore
    {
        public const int NUMBER_OF_TRADES_FOR_PAIR = 100;

        public static Dictionary<string, TradePair> TradePairs = new Dictionary<string, TradePair>();
        public static ConcurrentQueue<TradeData> RecentTrades = new ConcurrentQueue<TradeData>();

        public static ConcurrentDictionary<string, ConcurrentQueue<TradeData>> TradeData = 
            new ConcurrentDictionary<string, ConcurrentQueue<TradeData>>();
    }
}
