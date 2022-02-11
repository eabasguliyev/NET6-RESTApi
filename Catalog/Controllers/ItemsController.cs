using Catalog.DTOs;
using Catalog.Entities;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers{

    // GET /items
    [ApiController]
    [Route("[controller]")]
    public class ItemsController: ControllerBase
    {
        private readonly IItemsRepository repository;

        public ItemsController(IItemsRepository repository)
        {
            this.repository = repository;
        }

        // GET /items
        [HttpGet]
        public IEnumerable<ItemDto> GetItems(){
            var items = repository.GetItems().Select(item => item.AsDto());

            return items;
        }

        // GET /items/{id}
        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItem(Guid id){
            var item = repository.GetItem(id);

            if(item is null){
                return NotFound();
            }
            // return Ok(item);
            return item.AsDto();
        }

        // POST /items
        [HttpPost]
        public ActionResult<ItemDto> CreateItem(CreateItemDto itemDto){
            Item item = new(){
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            repository.CreateItem(item);

            return CreatedAtAction(nameof(GetItem), new{
                id = item.Id
            }, item.AsDto());
        }
    
        // PUT /items/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateItem(Guid id, UpdateItemDto itemDto){
            var item = repository.GetItem(id);

            if(item is null) return NotFound();

            var updatedItem = item with {
                Name = itemDto.Name,
                Price = itemDto.Price
            };

            repository.UpdateItem(updatedItem);

            return NoContent();
        }
    
        // DELETE /items/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteItem(Guid id){
            var item = repository.GetItem(id);

            if(item is null) return NotFound();

            repository.DeleteItem(id);

            return NoContent();
        }
    }
}