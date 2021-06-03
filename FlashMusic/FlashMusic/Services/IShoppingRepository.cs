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

        public Product GetProductByProductId(Guid ProductId);

        // 添加商品到购物车/订单



        // 获取订单列表

        // 订单结算/支付功能，删除订单，添加到历史记录

        // 获取历史记录列表
    }
}
