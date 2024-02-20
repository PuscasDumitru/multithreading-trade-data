
namespace multithreading_trade_data
{
    internal class TradeDataManager
    {

        public void OutputHeader()
        {
            Console.WriteLine($"{"Symbol",-6} | {"Price",-15} | {"Quantity",-10} | {"BaseAsset",-5} | {"BaseAssetPrecision",-5} | {"QuoteAsset",-5} " +
                $"| {"QuotePrecision",-5} | {"TradeTime",-18} | {"Type",-4} | {"ID",-15}");
                
            Console.WriteLine(new string('-', 150));
        }
        public void OutputTradeData()
        {
            OutputHeader();
            while (true)
            {
                TradeDataStore.RecentTrades.TryDequeue(out TradeData trade);

                if (trade != null)
                {
                    Console.ForegroundColor = trade.IsBuyerMaker ? ConsoleColor.Red : ConsoleColor.Green;

                    DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(trade.EventTime);
                    DateTime eventTimeToDateTime = dateTimeOffset.UtcDateTime;

                    Symbol tradePairInfo = TradeDataStore.TradePairs[trade.Symbol.ToUpper()].Symbols[0];
                    
                    Console.WriteLine($"{trade.Symbol} | {trade.Price} | {trade.Quantity} | {tradePairInfo.BaseAsset, -9} | {tradePairInfo.BaseAssetPrecision, -18} "
                         + $"| {tradePairInfo.QuoteAsset,-10} | {tradePairInfo.QuotePrecision,-15}" +
                        $"| {eventTimeToDateTime} | {(trade.IsBuyerMaker ? "Sell" : "Buy")} | {trade.TradeId}"); 
                    Console.ResetColor();
                }
            }
        }

        public void CleanOldTradeData()
        {
            while (true)
            {
                foreach (var pair in TradeDataStore.TradeData.Keys)
                {
                    if (TradeDataStore.TradeData.TryGetValue(pair, out var queue))
                    {
                        while (queue.Count > TradeDataStore.NUMBER_OF_TRADES_FOR_PAIR)
                        {
                            queue.TryDequeue(out _); 
                        }
                    }
                }

                Thread.Sleep(TimeSpan.FromMinutes(1));
            }
        }
    }
}
