using AutoMapper;
using FlashMusic.Dtos;
using FlashMusic.Models;
using FlashMusic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashMusic.Controllers
{
    [Route("product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IShoppingRepository _shoppingRepository;
        private IHttpContextAccessor _httpContextAccessor;
        private IMapper _mapper;

        public ProductController(IShoppingRepository shoppingRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            this._shoppingRepository = shoppingRepository;
            this._httpContextAccessor = httpContextAccessor;
            this._mapper = mapper;
        }

        [HttpGet("category")]
        public IActionResult GetProductsByCategoryId([FromQuery] int CategoryId)
        {
            IEnumerable<Product> res = _shoppingRepository.GetProductsByCategoryId(CategoryId);
            var resDto = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(res);
            return Ok(resDto);
        }

        [HttpGet("category/pagesize")]
        public IActionResult GetProductsByCategoryIdWithPageSize([FromQuery] int CategoryId,
            [FromQuery] int PageSize)
        {
            IEnumerable<Product> res = _shoppingRepository.GetProductsByCategoryIdWithPageSize(CategoryId, PageSize);
            var resDto = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(res);
            return Ok(resDto);
        }

        [HttpGet("category/pagination")]
        public IActionResult GetProductsWithPagination([FromQuery] int CategoryId,
            [FromQuery] int PageIndex, [FromQuery] int PageSize)
        {
            IEnumerable<Product> res = _shoppingRepository.GetProductsWithPagination(CategoryId, PageIndex, PageSize);
            var resDto = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(res);
            return Ok(resDto);
        }

        [HttpGet("{ProductId}")]
        public IActionResult GetProductByProductId([FromRoute] Guid ProductId)
        {
            Product res = _shoppingRepository.GetProductByProductId(ProductId);
            if (res == null)
                return NotFound();
            var resDto = _mapper.Map<Product, ProductDto>(res);
            return Ok(resDto);
        }
    }
}
