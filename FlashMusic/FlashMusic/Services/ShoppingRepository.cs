using FlashMusic.Database;
using FlashMusic.Dtos;
using FlashMusic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using EntityState = Microsoft.EntityFrameworkCore.EntityState;

namespace FlashMusic.Services
{
    public class ShoppingRepository : IShoppingRepository
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ShoppingRepository(AppDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this._context = context;
            this._config = configuration;
            this._httpContextAccessor = httpContextAccessor;
        }

        public IEnumerable<Product> GetProductsByCategoryId(int CategoryId)
        {
            var res = _context.Product.Where(t => t.CategoryId == CategoryId);
            return res.ToList();
        }

        public IEnumerable<Product> GetProductsByCategoryIdWithPageSize(int CategoryId, int PageSize)
        {
            var res = _context.Product.Where(t => t.CategoryId == CategoryId).Take(PageSize);
            return res.ToList();
        }

        public IEnumerable<Product> GetProductsWithPagination(int CategoryId, int PageIndex, int PageSize)
        {
            var res = _context.Product.Where(t => t.CategoryId == CategoryId).Skip(PageIndex * PageSize).Take(PageSize);
            return res.ToList();
        }

        public Product GetProductByProductId(Guid ProductId)
        {
            return _context.Product.FirstOrDefault(t => t.ProductId == ProductId);
        }

        public Guid GetProductIdByName(string Name)
        {
            return _context.Product.FirstOrDefault(t => t.Name == Name).ProductId;
        }

        // Guid的获取，应该在controller封装
        public bool AddProductToCart(int UserId, Guid ProductId, int Num)
        {
            _context.Cart.Add(new Cart
            {
                UserId = UserId,
                ProductId = ProductId,
                Num = Num,
                State = 0
            });
            return _context.SaveChanges() > 0;
        }

        public IEnumerable<CartGetDto> GetCartByUserId(int UserId)
        {
            // inner join
            var res =
                from cart in _context.Cart
                join product in _context.Product
                    on cart.ProductId equals product.ProductId
                where cart.UserId == UserId
                select new CartGetDto
                {
                    Num = cart.Num,
                    State = cart.State,
                    CategoryId = product.CategoryId,
                    Price = product.Price,
                    Name = product.Name,
                    PicUrl = product.PicUrl
                };
            return res.ToList();
        }

        public IEnumerable<Cart> PayCartByUserId(int UserId)
        {
            var res = _context.Cart.Where(t => t.UserId == UserId);
            var ret = res.ToList();
            _context.Cart.RemoveRange(res);
            _context.SaveChanges();
            return ret;
        }

        public bool DelCartById(int UserId, Guid ProductId)
        {
            var res = _context.Cart.FirstOrDefault(t => t.UserId == UserId && t.ProductId == ProductId);
            if (res == null)
                return false;
            _context.Cart.Remove(res);
            return _context.SaveChanges() > 0;
        }

        public bool UpdateCart(Guid ProductId, int Num)
        {
            var res = _context.Cart.FirstOrDefault(t => t.ProductId == ProductId);
            if (res == null)
                return false;
            res.Num = Num;
            return _context.SaveChanges() > 0;
        }

        public bool AddHistory(IEnumerable<Cart> CartList)
        {
            DateTime curtime = DateTime.Now;
            List<History> histories = new List<History>();
            foreach(Cart cart in CartList)
            {
                History addhistory = new History
                {
                    UserId = cart.UserId,
                    ProductId = cart.ProductId,
                    Num = cart.Num,
                    PayTime = DateTime.Now
                };
                histories.Add(addhistory);
                // Detach的使用
                // _context.Entry<History>(addhistory).State = EntityState.Detached;
                // _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                // foreach (var entity in _context.ChangeTracker.Entries())
                // {
                //     entity.State = EntityState.Detached;
                // }
            }

            _context.History.AddRange(histories);
            return _context.SaveChanges() > 0;
        }

        public bool AddHistoryItem(Cart CartItem)
        {

            _context.History.Add(new History
            {
                UserId = CartItem.UserId,
                ProductId = CartItem.ProductId,
                Num = CartItem.Num,
                PayTime = DateTime.Now
            });
            return _context.SaveChanges() > 0;
        }

        public IEnumerable<HistoryGetDto> GetHistoryByUserId(int UserId)
        {
            var res =
                from history in _context.History
                join product in _context.Product
                    on history.ProductId equals product.ProductId
                where history.UserId == UserId
                select new HistoryGetDto
                {
                    PayTime = history.PayTime,
                    Num = history.Num,
                    CategoryId = product.CategoryId,
                    Price = product.Price,
                    Name = product.Name,
                    PicUrl = product.PicUrl
                };
            return res.ToList();
        }

        public IEnumerable<History> GetHistoryByUserIdWithTime(int UserId, DateTime PayTime)
        {
            var res = _context.History.Where(t => t.UserId == UserId && t.PayTime == PayTime);
            return res.ToList();
        }

    }
}
