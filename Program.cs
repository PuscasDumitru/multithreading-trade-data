
using Newtonsoft.Json;
using multithreading_trade_data;

class Program
{
    static void GetUserInput()
    {
        HttpClient client = new HttpClient();
        try
        {
            string tradePairsUrl = "https://api.binance.com/api/v3/exchangeInfo?symbol=";

            Console.WriteLine("Print the number of trading pairs you would like to see the trades for?");
            int tradePairsCount = int.Parse(Console.ReadLine());

            Console.WriteLine("Please print each of the trade pairs in the following format: usdt/btc");

            for (int tradePairIndex = 1; tradePairIndex <= tradePairsCount; tradePairIndex++)
            {
                Console.WriteLine($"Print the trade pair #{tradePairIndex}: ");
                string tradePairSymbol = Console.ReadLine().Replace("/", "").ToUpper();

                HttpResponseMessage response = client.GetAsync(tradePairsUrl + tradePairSymbol).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();
                string responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                TradePair tradePairResult = JsonConvert.DeserializeObject<TradePair>(responseBody);
                TradeDataStore.TradePairs.Add(tradePairSymbol, tradePairResult);
            }

        }
        catch (JsonSerializationException jsonEx)
        {
            Console.WriteLine($"JSON parsing error: {jsonEx.Message}");
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"\nAn exception occurred while trying to get a trade pair. \nMessage: {e.Message}");
        }
    }

    static void Main(string[] args)
    {
        GetUserInput();
        List<Thread> threads = new List<Thread>();
        foreach (string tradePairSymbol in TradeDataStore.TradePairs.Keys)
        {
            TradeStreamHandler handler = new TradeStreamHandler(tradePairSymbol);
            Thread thread = new Thread(new ThreadStart(handler.Start));

            threads.Add(thread);
            thread.Start();
        }

        TradeDataManager tradeDataManager = new TradeDataManager();

        Thread cleaningThread = new Thread(new ThreadStart(tradeDataManager.CleanOldTradeData));
        cleaningThread.Start();

        Thread displayThread = new Thread(new ThreadStart(tradeDataManager.OutputTradeData));
        displayThread.Start();
    }
}
