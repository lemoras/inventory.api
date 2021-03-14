using Microsoft.EntityFrameworkCore;
using InventoryWebApi.Models;

namespace InventoryWebApi.DataAccess
{
    public class InventoryContext: DbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> options)
            : base(options)
        { }

        public DbSet<InventoryItem> Inventory { get; set; }
    }
}
