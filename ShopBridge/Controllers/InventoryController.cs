using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopBridge.Dll;
using ShopBridge.Model;
using System.Net;

namespace ShopBridge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryRepository _dbcontext;

        public InventoryController(IInventoryRepository dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<CommonResult>> GetAllItems()
        {
            var result = await _dbcontext.GetAllItems().ConfigureAwait(false);
            return Ok(result);

        }
        [HttpGet]
        [Route("Inventory/{searchText}")]
        public async Task<ActionResult> GetAllAsync(string searchText = null, int pageSize = 50, int page = 1)
        {
            try
            {
                if (pageSize <= 0)
                    pageSize = 50;
                if (page < 1)
                    page = 1;

                var item = await _dbcontext.GetAllItemAsync(searchText);

                var count = item.Count;

                var paginatedItems = item
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();

                var paginatedList = new PaginatedList<ShopBridge.Dll.Inventory>(paginatedItems, count, page, pageSize);
                return Ok(paginatedList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        [HttpGet]
        [Route("{Id}")]
        public async Task<ActionResult<CommonResult>> GetItemById(int Id)
        {
            if (Id != 0)
            {
                var result = await _dbcontext.GetItemById(Id).ConfigureAwait(false);
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("addItem")]
        public async Task<ActionResult<CommonResult>> AddItem([FromBody] ShopBridge.Dll.Inventory request)
        {

            if (ModelState.IsValid)
            {
                if (request.Price != 0)
                {
                    var item = await _dbcontext.AddItem(request).ConfigureAwait(false);
                    return Ok(item);
                }
                else
                {
                    return BadRequest();
                }

            }
            else
            {
                return BadRequest();
            }
        }


        [HttpPut]
        [Route("updateItem")]
        public async Task<ActionResult<CommonResult>> UpdateItem([FromBody] ShopBridge.Dll.Inventory request)
        {
            if (ModelState.IsValid)
            {
                var result = await _dbcontext.UpdateItem(request).ConfigureAwait(false);

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpDelete]
        [Route("removeItem")]
        public async Task<ActionResult<CommonResult>> RemoveItem(int Id)
        {
            if (Id != 0)
            {
                var result = await _dbcontext.RemoveItem(Id).ConfigureAwait(false);
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
