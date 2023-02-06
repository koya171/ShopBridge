using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBridge.Dll
{
    public class InventoryRepository : IInventoryRepository
    {
        public async Task<CommonResult> GetAllItems()
        {
            CommonResult oCommonResult = new CommonResult
            {
                Count = 0,
                Message = string.Empty,
                Status = StatusCode.NoResult,
                Result = null
            };
            try
            {
                using (var inventoryDataContext = new InventoryDataContext())
                {
                    var item = await inventoryDataContext.Inventorys.ToListAsync();
                    oCommonResult = new CommonResult
                    {
                        Count = item.Any() ? item.Count(): 0,
                        Message = "Get All Inventory",
                        Status = StatusCode.Sucess,
                        Result = item ?? new List<Inventory>()
                    };
                }
            }
            catch(Exception ex)
            {
                oCommonResult = new CommonResult
                {
                    Count = 0,
                    Message = "server Error ",
                    Status = StatusCode.Fail,
                    Result = false
                };
            }
            return oCommonResult;
           
        }

        public async Task<CommonResult> GetItemById(int Id)
        {
            CommonResult oCommonResult = new CommonResult
            {
                Count = 0,
                Message = string.Empty,
                Status = StatusCode.NoResult,
                Result = null
            };
            try
            {
                using (var inventoryDataContext = new InventoryDataContext())
                {
                   var item=  await inventoryDataContext.Inventorys.FirstOrDefaultAsync(x => x.Id == Id);
                    if (item != null)
                    {
                        return oCommonResult = new CommonResult
                        {
                            Count = 1,
                            Message = "By Id",
                            Status = StatusCode.Sucess,
                            Result = item
                        };
                    }
                  
                }
            }
            catch(Exception ex)
            {
                return oCommonResult = new CommonResult
                {
                    Count = 0,
                    Message = "server Error",
                    Status = StatusCode.Fail,
                    Result = null
                };
            }
            return oCommonResult;


        }

        public async Task<CommonResult> AddItem(Inventory item)
        {

            CommonResult oCommonResult = new CommonResult
            {
                Count = 0,
                Message = string.Empty,
                Status = StatusCode.NoResult,
                Result = null
            };
            try
            {
                using (var inventoryDataContext = new InventoryDataContext())
                {
                    await inventoryDataContext.Inventorys.AddAsync(item);
                    await inventoryDataContext.SaveChangesAsync();
                    return oCommonResult = new CommonResult
                    {
                        Count = 1,
                        Message = "Added successfully",
                        Status = StatusCode.Sucess,
                        Result = item.Id
                    };
                }
            }
            catch (Exception ex)
            {
                oCommonResult = new CommonResult
                {
                    Count = 0,
                    Message = ex.Message + "" + ex.StackTrace,
                    Status = StatusCode.Fail,
                    Result = null
                };
            }
            return oCommonResult;
        }

        public async Task<CommonResult> UpdateItem(Inventory item)
        {
            CommonResult oCommonResult = new CommonResult
            {
                Count = 0,
                Message = string.Empty,
                Status = StatusCode.NoResult,
                Result = null
            };
            try
            {
                using (var inventoryDataContext = new InventoryDataContext())
                {
                    var itemToBeUpdated = await inventoryDataContext.Inventorys.FirstOrDefaultAsync(x => x.Id == item.Id);
                    if (itemToBeUpdated != null)
                    {
                        itemToBeUpdated.Price = item.Price;
                        itemToBeUpdated.Name = item.Name;
                        itemToBeUpdated.Description = item.Description;
                        await inventoryDataContext.SaveChangesAsync();
                        return oCommonResult = new CommonResult
                        {
                            Count = 0,
                            Message = "Updated successfully",
                            Status = StatusCode.NoResult,
                            Result = null
                        };
                    }
                }
            }
            catch(Exception ex)
            {
                oCommonResult = new CommonResult
                {
                    Count = 0,
                    Message = ex.Message + "" + ex.StackTrace,
                    Status = StatusCode.Fail,
                    Result = null
                };
            }
            return oCommonResult;
        }

        public async Task<CommonResult> RemoveItem(int Id)
        {
            CommonResult oCommonResult = new CommonResult
            {
                Count = 0,
                Message = string.Empty,
                Status = StatusCode.NoResult,
                Result = null
            };
            try
            {
                using (var inventoryDataContext = new InventoryDataContext())
                {
                    var itemToBeDeleted = await inventoryDataContext.Inventorys.FirstOrDefaultAsync(x => x.Id == Id);
                    if (itemToBeDeleted != null)
                    {
                        inventoryDataContext.Inventorys.Remove(itemToBeDeleted);
                        await inventoryDataContext.SaveChangesAsync();
                        return oCommonResult = new CommonResult
                        {
                            Count = 0,
                            Message = "Deleted successfully ",
                            Status = StatusCode.NoResult,
                            Result = null
                        };
                    }

                   
                }

            }
            catch(Exception ex)
            {
                oCommonResult = new CommonResult
                {
                    Count = 0,
                    Message = ex.Message + "" + ex.StackTrace,
                    Status = StatusCode.Fail,
                    Result = null
                };
            }
            return oCommonResult;
        }

        public async Task<List<Inventory>> GetAllItemAsync(string searchText)
        {
            using (var inventoryDataContext = new InventoryDataContext())
            {
                var query = inventoryDataContext.Inventorys
                    .AsNoTracking();

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    searchText = searchText.ToLower();
                    query = query.Where(u => u.Name.ToLower().Contains(searchText));
                }

                query = query.OrderBy(u => u.Name);

                var Items = await query.ToListAsync();

                return new List<Inventory>(Items);
            }
        }
    }
}
