using System.Threading.Tasks;
using InventoryWebApi.DataAccess;
using InventoryWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using InventoryWebApi.DTO;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace InventoryWebApi.Controllers
{
    [ApiController]
    [Route("[controller]/item")]
    public class Inventory : ControllerBase
    {
        private readonly IRepository _repository;

        public Inventory(IRepository repository)
        {
            this._repository = repository;
        }

        [HttpGet("query")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Get([FromQuery]Query query)
        {
            return Ok(await _repository.GetInventoryItem(new ItemQueryModel{
                Barcode = query.barcode,
                Name = query.name,
                Category = query.category,
                Discount = query.discount
            }));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Get()
        {
            return Ok(await _repository.GetInventory());
        }

        [HttpGet("sort")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetSortedPrice()
        {
            return Ok(await _repository.GetInventorySortedPrice());
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Create(CreateForm form)
        {
             var result = await _repository.AddInventoryItem(new InventoryItem{
                 Name = form.Name,
                 Category = form.Category,
                 Price = form.Price,
                 Quantity = form.Quantity,
                 Discount = form.Discount
             });

            return CreatedAtAction(nameof(Get), new Query { barcode = result.Barcode }, result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Edit(int barcode, [FromBody] CreateForm form)
        {
            var inventory =  await _repository.GetInventoryItem(new ItemQueryModel{
                Barcode = barcode
            });

            if (inventory.Any())
            {
                inventory.ForEach(async x => {
                    x.Name = form.Name;
                    x.Category = form.Category;
                    x.Price = form.Price;
                    x.Quantity = form.Quantity;
                    x.Discount = form.Discount;
                    await _repository.UpdateInventoryItem(x);
                });
                
                return NoContent();
            }                
            
            return NotFound();
        }

        [HttpDelete("{barcode}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(int barcode)
        {
            var inventory =  await _repository.GetInventoryItem(new ItemQueryModel {
                Barcode = barcode
            });

            if (inventory.Any())
            {
                inventory.ForEach(x=> _repository.DeleteInventoryItem(x));
            }

            return NoContent();
        }
    }
}
