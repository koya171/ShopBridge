using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBridge.Dll
{
    public interface IInventoryRepository
    {
        Task<CommonResult> GetAllItems();
        Task<List<Inventory>> GetAllItemAsync(string searchText);
        Task<CommonResult> GetItemById(int Id);

        Task<CommonResult> AddItem(Inventory request);

        Task<CommonResult> UpdateItem(Inventory request);

        Task<CommonResult> RemoveItem(int Id);
    }
}
