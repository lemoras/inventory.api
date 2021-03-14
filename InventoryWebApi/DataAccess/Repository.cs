using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InventoryWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InventoryWebApi.DataAccess
{
    public class Repository: IRepository
    {
        private readonly InventoryContext _context;

        public Repository(InventoryContext context)
        {
            _context = context;
        }

        public async Task<List<InventoryItem>> GetInventory()
        {
            IQueryable<InventoryItem> items = _context.Inventory;
            // return await restaurants.OrderByDescending(o => o.Id).ToListAsync();
            return await items.OrderBy(o => o.Barcode).ToListAsync();
        }
        
        public async Task<List<InventoryItem>> GetInventorySortedPrice()
        {
            IQueryable<InventoryItem> items = _context.Inventory;
            // return await restaurants.OrderByDescending(o => o.Id).ToListAsync();
            return await items.OrderByDescending(o => o.Price).ToListAsync();
        }

        public async Task<InventoryItem> AddInventoryItem(InventoryItem inventoryItem)
        {
            await _context.Inventory.AddAsync(inventoryItem);
            await _context.SaveChangesAsync();

            return inventoryItem;
        }

        public async Task<List<InventoryItem>> GetInventoryItem(ItemQueryModel query)
        {
            IQueryable<InventoryItem> items = _context.Inventory;

            if (query.Barcode > 0)
            {
                items = items.Where(o => o.Barcode == query.Barcode);
            }

            if (query.Category != null)
            {
                items = items.Where(o => o.Category == query.Category);
            }

            if (query.Name != null)
            {
                items = items.Where(o => o.Name == query.Name);
            }

            if (query.Discount > 0)
            {
                items = items.Where(o => o.Discount >= query.Discount);
            }

            return await items.OrderBy(o => o.Barcode).ToListAsync();
        }

        public async Task<InventoryItem> UpdateInventoryItem(InventoryItem inventoryItem)
        {
            _context.Inventory.Update(inventoryItem);
            await _context.SaveChangesAsync();

            return inventoryItem;
        }

        public async Task<List<InventoryItem>> GetSorted()
        {
            IQueryable<InventoryItem> items = _context.Inventory;
            return await items.OrderByDescending(o => o.Price).ToListAsync();
        }

        public async Task<InventoryItem> DeleteInventoryItem(InventoryItem inventoryItem)
        {
            _context.Inventory.Remove(inventoryItem);
            await _context.SaveChangesAsync();

            return inventoryItem;
        }
    }
}
