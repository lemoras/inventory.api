using System.Threading.Tasks;
using System.Collections.Generic;
using InventoryWebApi.Models;

namespace InventoryWebApi.DataAccess
{
    public interface IRepository
    {
        Task<List<InventoryItem>> GetInventory();
        Task<List<InventoryItem>> GetInventorySortedPrice();
        Task<InventoryItem> AddInventoryItem(InventoryItem inventoryItem);
        Task<List<InventoryItem>> GetInventoryItem(ItemQueryModel query);
        Task<List<InventoryItem>> GetSorted();
        Task<InventoryItem> UpdateInventoryItem(InventoryItem inventoryItem);
        Task<InventoryItem> DeleteInventoryItem(InventoryItem inventoryItem);
    }
}
