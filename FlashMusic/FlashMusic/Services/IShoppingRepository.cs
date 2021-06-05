using FlashMusic.Dtos;
using FlashMusic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashMusic.Services
{
    public interface IShoppingRepository
    {
        // 获取商品列表
        public IEnumerable<Product> GetProductsByCategoryId(int CategoryId);

        public IEnumerable<Product> GetProductsByCategoryIdWithPageSize(int CategoryId, int PageSize);

        public IEnumerable<Product> GetProductsWithPagination(int CategoryId, int PageIndex, int PageSize);     // PageIndex从0开始

        public Product GetProductByProductId(Guid ProductId);

        public Guid GetProductIdByName(string Name);

        // 添加商品到购物车
        public bool AddProductToCart(int UserId, Guid ProductId, int Num);

        // 获取购物车列表
        public IEnumerable<CartGetDto> GetCartByUserId(int UserId);

        // 购物车结算 / 删除
        public IEnumerable<Cart> PayCartByUserId(int UserId);

        public bool DelCartById(int UserId, Guid ProductId);

        // 更新购物车 数量
        public bool UpdateCart(Guid ProductId, int Num);

        // 添加历史订单
        public bool AddHistory(IEnumerable<Cart> CartList);

        public bool AddHistoryItem(Cart CartItem);

        // 获取历史记录列表
        public IEnumerable<HistoryGetDto> GetHistoryByUserId(int UserId);

        public IEnumerable<History> GetHistoryByUserIdWithTime(int UserId, DateTime PayTime);
    }
}
