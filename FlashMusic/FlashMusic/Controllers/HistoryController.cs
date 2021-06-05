using AutoMapper;
using FlashMusic.Dtos;
using FlashMusic.Services;
using FlashMusic.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashMusic.Controllers
{
    [Route("history")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private IShoppingRepository _shoppingRepository;
        private IHttpContextAccessor _httpContextAccessor;
        private IMapper _mapper;

        public HistoryController(IShoppingRepository shoppingRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            this._shoppingRepository = shoppingRepository;
            this._httpContextAccessor = httpContextAccessor;
            this._mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetHistory()
        {
            var id = _httpContextAccessor.HttpContext.User.FindFirst("Id").Value;
            int userid = Int32.Parse(id);
            IEnumerable<HistoryGetDto> historylist = _shoppingRepository.GetHistoryByUserId(userid);
            return Ok(new { 
                data = historylist,
                msg = Message.GetHistorySuccess()
            });
        }


    }
}
