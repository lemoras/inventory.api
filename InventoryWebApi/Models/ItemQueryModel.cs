using System;
namespace InventoryWebApi.Models
{
    public class ItemQueryModel
    {
        public int Barcode { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int Discount { get; set; }
    }
}
