using Newtonsoft.Json;

namespace Stripe
{
    public class StripeCardCreateOptions
    {
        [JsonProperty("card")]
        public StripeCreditCardOptions Card { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

    }
}
