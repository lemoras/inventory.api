using System;
namespace InventoryWebApi.DTO
{
    public class Query
    {
        public int barcode { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public int discount { get; set; }
    }
}
