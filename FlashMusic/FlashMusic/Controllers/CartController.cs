using AutoMapper;
using FlashMusic.Dtos;
using FlashMusic.Models;
using FlashMusic.Services;
using FlashMusic.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FlashMusic.Controllers
{
    [Route("cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private IShoppingRepository _shoppingRepository;
        private IHttpContextAccessor _httpContextAccessor;
        private IMapper _mapper;

        public CartController(IShoppingRepository shoppingRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            this._shoppingRepository = shoppingRepository;
            this._httpContextAccessor = httpContextAccessor;
            this._mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetCart()
        {
            var id = _httpContextAccessor.HttpContext.User.FindFirst("Id").Value;
            int userid = Int32.Parse(id);
            IEnumerable<CartGetDto> cartlist = _shoppingRepository.GetCartByUserId(userid);
            int cartnum = cartlist.Count();
            double totalprice = 0;
            foreach(CartGetDto cart in cartlist)
            {
                totalprice += cart.Num * cart.Price;
            }
            return Ok(new { 
                data = cartlist,
                msg = Message.GetCartSuccess(),
                cartnum = cartnum,
                totalprice = totalprice
            });
        }

        // 添加单个商品
        [HttpPost("cartadd")]
        [Authorize]
        public IActionResult AddCart([FromBody] CartAddDto cartAdd)
        {
            var id = _httpContextAccessor.HttpContext.User.FindFirst("Id").Value;
            int userid = Int32.Parse(id);
            Guid productid = _shoppingRepository.GetProductIdByName(cartAdd.Name);
            if(!_shoppingRepository.AddProductToCart(userid, productid, cartAdd.Num))
            {
                return Ok(Message.AddCartFail());
            }
            return Ok(Message.AddCartSuccess());
        }

        [HttpPost("cartdel")]
        [Authorize]
        public IActionResult DelCart([FromBody] CartDelDto cartDel)
        {
            var id = _httpContextAccessor.HttpContext.User.FindFirst("Id").Value;
            int userid = Int32.Parse(id);
            Guid productid = _shoppingRepository.GetProductIdByName(cartDel.Name);
            if(!_shoppingRepository.DelCartById(userid, productid))
            {
                return Ok(Message.DelCartFail());
            }
            return Ok(Message.DelCartSuccess());
        }

        [HttpPost("cartpay")]
        [Authorize]
        public IActionResult PayCart()
        {
            var id = _httpContextAccessor.HttpContext.User.FindFirst("Id").Value;
            int userid = Int32.Parse(id);
            IEnumerable<Cart> cartlist = _shoppingRepository.PayCartByUserId(userid);
            int judge = 1;
            foreach(Cart cartitem in cartlist)
            {
                if(!_shoppingRepository.AddHistoryItem(cartitem))
                {
                    judge = 0;
                }
            }
            if(judge == 1)
            {
                return Ok(Message.PayCartSuccess());

            }
            else
            {
                return Ok(Message.PayCartFail());
            }
        }

        // 测试AddRange使用
        [HttpPost("cartpayrange")]
        [Authorize]
        public IActionResult PayCartRange()
        {
            var id = _httpContextAccessor.HttpContext.User.FindFirst("Id").Value;
            int userid = Int32.Parse(id);
            IEnumerable<Cart> cartlist = _shoppingRepository.PayCartByUserId(userid);
            if(!_shoppingRepository.AddHistory(cartlist))
            {
                return Ok(Message.PayCartFail());
            }
            return Ok(Message.PayCartSuccess());
        }

        [HttpPost("cartupdate")]
        [Authorize]
        public IActionResult UpdateCart([FromBody] CartUpdateDto cartUpdate)
        {
            var id = _httpContextAccessor.HttpContext.User.FindFirst("Id").Value;
            int userid = Int32.Parse(id);
            Guid productid = _shoppingRepository.GetProductIdByName(cartUpdate.Name);
            if (!_shoppingRepository.UpdateCart(productid, cartUpdate.Num))
            {
                return Ok(Message.UpdateCartFail());
            }
            return Ok(Message.UpdateCartSuccess());
        }
    }
}
