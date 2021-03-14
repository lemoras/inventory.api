using Newtonsoft.Json;

namespace InventoryWebApi.DTO
{
    public class CreateForm
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("price")]
        public int Price { get; set; }

        [JsonProperty("discount")]
        public int Discount { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}
