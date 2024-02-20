
using Newtonsoft.Json;
using System.Collections.Concurrent;
using WebSocketSharp;

namespace multithreading_trade_data
{
    public class TradeStreamHandler
    {
        private string _tradePairSymbol;
        private string _url;

        public TradeStreamHandler(string tradePairSymbol)
        {
            _tradePairSymbol = tradePairSymbol;
            _url = "wss://stream.binance.com:9443/ws/" + tradePairSymbol.ToLower() + "@trade";
        }
        public void Start()
        {
            using (WebSocket ws = new WebSocket(_url))
            {
                ws.OnMessage += (sender, e) =>
                {
                    try
                    {
                        var tradeData = JsonConvert.DeserializeObject<TradeData>(e.Data);
                        var tradesQueue = TradeDataStore.TradeData.GetOrAdd(_tradePairSymbol, new ConcurrentQueue<TradeData>());

                        if (tradeData != null)
                        {
                            tradesQueue.Enqueue(tradeData);
                            TradeDataStore.RecentTrades.Enqueue(tradeData);
                        }
                    }
                    catch (JsonSerializationException jsonEx)
                    {
                        Console.WriteLine($"JSON parsing error: {jsonEx.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error while parsing trade data: {ex.Message}");
                    }
                };

                ws.OnError += (sender, e) =>
                {
                    Console.WriteLine($"Error in {_tradePairSymbol} stream: {e.Message}");
                };

                ws.Connect();

                while (ws.IsAlive)
                {
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
