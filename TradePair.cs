
using Newtonsoft.Json;

namespace multithreading_trade_data
{
    public class TradePair
    {
        [JsonProperty("symbols")]
        public List<Symbol>? Symbols { get; set; }
    }

    public class Symbol
    {
        [JsonProperty("baseAsset")]
        public string? BaseAsset { get; set; }

        [JsonProperty("baseAssetPrecision")]
        public int BaseAssetPrecision { get; set; }

        [JsonProperty("quoteAsset")]
        public string? QuoteAsset { get; set; }

        [JsonProperty("quotePrecision")]
        public int QuotePrecision { get; set; }

    }
}
