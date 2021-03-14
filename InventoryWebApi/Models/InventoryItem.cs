using System.ComponentModel.DataAnnotations;

namespace InventoryWebApi.Models
{
    public class InventoryItem
    {
        /// <summary>
        /// barcode of the item
        /// </summary>
        [Key]
        public int Barcode { get; set; }
        /// <summary>
        /// Name of the item
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Category to which item belongs
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// Price of the item
        /// </summary>
        public int Price { get; set; }
        /// <summary>
        /// Discount on the item
        /// </summary>
        public int Discount { get; set; }
        /// <summary>
        /// Quantity of the items available in store
        /// </summary>
        public int Quantity { get; set; }
    }
}
