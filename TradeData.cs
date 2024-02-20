using Newtonsoft.Json;

namespace multithreading_trade_data
{
    public class TradeData
    {
        [JsonProperty("e")]
        public string? EventType { get; set; }

        [JsonProperty("E")]
        public long EventTime { get; set; }

        [JsonProperty("s")]
        public string? Symbol { get; set; }

        [JsonProperty("p")]
        public string? Price { get; set; }

        [JsonProperty("q")]
        public string? Quantity { get; set; }

        [JsonProperty("t")]
        public string? TradeId { get; set; }

        [JsonProperty("m")]
        public bool IsBuyerMaker { get; set; }
    }
}
