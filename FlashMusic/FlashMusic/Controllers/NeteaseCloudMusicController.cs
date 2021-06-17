using FlashMusic.Dtos;
using Microsoft.AspNetCore.Mvc;
using NeteaseCloudMusicApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

/**
 * 网易云Api实现
 * 工作原理：跨站请求伪造（CSRF），伪造请求头
 * 共148个 Api
 */
namespace FlashMusic.Controllers
{
    [ApiController]
    public class NeteaseCloudMusicController : ControllerBase
    {
		// 初始化 Cloud MusicApi
        static CloudMusicApi api = new CloudMusicApi();
		// 个人信息
		static string account = "13076320849";
		static string password = "xx88561368";
		static long uid = 545406676;
		static string nickname = "Flash_ing";
		static string birthday = "953086895804";


		public NeteaseCloudMusicController()
        {
			// 每次调用api都会使用该方法
            // LoginCellphone();
        }

		/// <summary>
		/// 初始化时登录
		/// </summary>
		/// <returns></returns>
        static async Task LoginCellphone()
        {
			/******************** 登录 ********************/
			// 循环执行直到登录成功
			while (true)
			{
				var queries = new Dictionary<string, object>();
				bool isPhone = Regex.Match(account, "^[0-9]+$").Success;
				queries[isPhone ? "phone" : "email"] = account;
				queries["password"] = password;

				if (!CloudMusicApi.IsSuccess(await api.RequestAsync(isPhone ? CloudMusicApiProviders.LoginCellphone : CloudMusicApiProviders.Login, queries, false)))
					Console.WriteLine("登录失败，账号或密码错误");
				else
					break;
			}
			Console.WriteLine("登录成功");
			Console.WriteLine();

			/******************** 登录 ********************/

			/******************** 获取账号信息 ********************/

			var json = await api.RequestAsync(CloudMusicApiProviders.LoginStatus);

			uid = (long)json["profile"]["userId"];
			// Console.WriteLine($"账号ID： {uid}");
			// Console.WriteLine($"账号昵称： {json["profile"]["nickname"]}");

			/******************** 获取账号信息 ********************/
		}

		/// <summary>
		/// 初始化昵称，失败
		/// </summary>
        [HttpPost("activate/init/profile")]
        public async Task<IActionResult> ActivateInitProfile()
        {
			var json = await api.RequestAsync(CloudMusicApiProviders.ActivateInitProfile, new Dictionary<string, object> { ["nickname"] = nickname });
			return Ok(json);
        }

		/// <summary>
		/// 获取专辑内容
		/// </summary>
		[HttpGet("album")]
		public async Task<IActionResult> Album([FromQuery] string id)
        {
			var json = await api.RequestAsync(CloudMusicApiProviders.Album, new Dictionary<string, object> { ["id"] = id });
			return Ok(json);
        }

		/// <summary>
		/// 专辑动态信息
		/// </summary>
		[HttpPost("album/detail/dynamic")]
		public async Task<IActionResult> AlbumDetailDynamic([FromBody] AlbumDetailDynamicDto albumDetailDynamicDto)
        {
			var json = await api.RequestAsync(CloudMusicApiProviders.AlbumDetailDynamic, new Dictionary<string, object> { ["id"] = albumDetailDynamicDto.id });
			return Ok(json);
		}

		/// <summary>
		/// 最新专辑
		/// </summary>
		[HttpPost("album/newest")]
		public async Task<IActionResult> AlbumNewest()
        {
			var json = await api.RequestAsync(CloudMusicApiProviders.AlbumNewest);
			return Ok(json);
        }

		/// <summary>
		/// 收藏/取消收藏专辑
		/// </summary>
		[HttpPost("album/sub")]
		public async Task<IActionResult> AlbumSub([FromBody] AlbumSubDto albumSubDto)
        {
			var json = await api.RequestAsync(CloudMusicApiProviders.AlbumSub, new Dictionary<string, object>
			{
				["t"] = albumSubDto.t, ["id"] = albumSubDto.id
			});
			return Ok(json);
        }

		/// <summary>
		/// 获取已收藏专辑列表
		/// </summary>
		[HttpPost("album/sublist")]
		public async Task<IActionResult> AlbumSublist()
        {
			var json = await api.RequestAsync(CloudMusicApiProviders.AlbumSublist);
			return Ok(json);
        }
		
		/// <summary>
		/// 获取歌手单曲
		/// </summary>
		[HttpGet("artists")]
		public async Task<IActionResult> Artists([FromQuery] string id)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.Artists, new Dictionary<string, object> { ["id"] = id });
			return Ok(json);
		}

		/// <summary>
		/// 获取歌手专辑
		/// </summary>
		[HttpGet("artist/album")]
		public async Task<IActionResult> ArtistAlbum([FromQuery] string id)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.ArtistAlbum, new Dictionary<string, object> { ["id"] = id });
			return Ok(json);
		}

		/// <summary>
		/// 获取歌手描述
		/// </summary>
		[HttpGet("artist/desc")]
		public async Task<IActionResult> ArtistsDesc([FromQuery] string id)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.ArtistDesc, new Dictionary<string, object> { ["id"] = id });
			return Ok(json);
		}

		/// <summary>
		/// 歌手分类列表  修改
		/// </summary>
		[HttpGet("artist/list")]
		public async Task<IActionResult> ArtistList([FromQuery] string offset,
													[FromQuery] string initial,
													[FromQuery] string area,
													[FromQuery] string type)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.ArtistList, new Dictionary<string, object> { 
				["offset"] = offset,
				["initial"] = initial,
				["area"] = area,
				["type"] = type
			});
			return Ok(json);
		}

		/// <summary>
		/// 获取歌手mv
		/// </summary>
		[HttpGet("artist/mv")]
		public async Task<IActionResult> ArtistMv([FromQuery] string id)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.ArtistMv, new Dictionary<string, object> { ["id"] = id });
			return Ok(json);
		}

		/// <summary>
		/// 收藏/取消收藏歌手
		/// </summary>
		[HttpPost("artist/sub")]
		public async Task<IActionResult> ArtistSub([FromBody] NeteaseCloudDto2 neteaseCloudDto)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.ArtistSub, new Dictionary<string, object>
			{
				["id"] = neteaseCloudDto.id,
				["t"] = neteaseCloudDto.t
			}); ;
			return Ok(json);
		}

		/// <summary>
		/// 收藏的歌手列表
		/// </summary>
		[HttpPost("artist/sublist")]
		public async Task<IActionResult> ArtistSublist()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.ArtistSublist);
			return Ok(json);
		}

		/// <summary>
		/// 歌手热门50首歌曲
		/// </summary>
		[HttpPost("artist/top/song")]
		public async Task<IActionResult> ArtistTopSong([FromBody] NeteaseCloudDto neteaseCloudDto)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.ArtistTopSong, new Dictionary<string, object> { ["id"] = neteaseCloudDto.id });
			return Ok(json);
		}

		/// <summary>
		/// 轮播图
		/// </summary>
		[HttpGet("banner")]
		public async Task<IActionResult> Banner()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.Banner);
			return Ok(json);
		}

		/// <summary>
		/// batch批量请求接口
		/// </summary>
		[HttpPost("batch")]
		public async Task<IActionResult> Batch()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.Batch);
			return Ok(json);
		}

		/// <summary>
		/// 发送验证码
		/// </summary>
		[HttpPost("captcha/sent")]
		public async Task<IActionResult> CaptchaSent([FromBody] CaptchaSentDto captchaSentDto)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.CaptchaSent, new Dictionary<string, object> { ["phone"] = captchaSentDto.phone });
			return Ok(json);
		}

		/// <summary>
		/// 验证验证码
		/// </summary>
		[HttpPost("captcha/verify")]
		public async Task<IActionResult> CaptchaVerify([FromBody] CaptchaVerifyDto captchaVerifyDto)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.CaptchaVerify, new Dictionary<string, object> { 
				["phone"] = captchaVerifyDto.phone,
				["captcha"] = captchaVerifyDto.captcha
			});
			return Ok(json);
		}

		/// <summary>
		/// 检测手机号码是否已注册
		/// </summary>
		[HttpPost("cellphone/existence/check")]
		public async Task<IActionResult> CellphoneExistenceCheck([FromBody] CellphoneExistenceCheckDto cellphoneExistenceCheckDto)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.CellphoneExistenceCheck, new Dictionary<string, object> { ["phone"] = cellphoneExistenceCheckDto.phone });
			return Ok(json);
		}

		/// <summary>
		/// 音乐是否可用，失败
		/// </summary>
		[HttpPost("check/music")]
		public async Task<IActionResult> CheckMusic([FromBody] NeteaseCloudDto neteaseCloudDto)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.CheckMusic, new Dictionary<string, object> { ["id"] = neteaseCloudDto.id });
			return Ok(json);
		}

		/// <summary>
		/// 发送/删除评论
		/// </summary>
		[HttpGet("comment")]
		public async Task<IActionResult> Comment([FromQuery] string t,
												[FromQuery] string type,
												[FromQuery] string id,
												[FromQuery] string content,
												[FromQuery] string commentId)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.Comment, new Dictionary<string, object> { 
				["t"] = t,
				["type"] = type,
				["id"] = id,
				["content"] = content,
				["commentId"] = commentId
			});
			return Ok(json);
		}

		/// <summary>
		/// 专辑评论
		/// </summary>
		[HttpGet("comment/album")]
		public async Task<IActionResult> CommentAlbum([FromQuery] string id)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.CommentAlbum, new Dictionary<string, object> { ["id"] = id });
			return Ok(json);
		}

		/// <summary>
		/// 电台节目评论
		/// </summary>
		[HttpPost("comment/dj")]
		public async Task<IActionResult> CommentDj([FromBody] NeteaseCloudDto neteaseCloudDto)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.CommentDj, new Dictionary<string, object> { ["id"] = neteaseCloudDto.id });
			return Ok(json);
		}

		/// <summary>
		/// 获取动态评论
		/// </summary>
		[HttpPost("comment/event")]
		public async Task<IActionResult> CommentEvent([FromBody] CommentEventDto commentEventDto)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.CommentEvent, new Dictionary<string, object> { ["threadId"] = commentEventDto.threadId });
			return Ok(json);
		}

		/// <summary>
		/// 热门评论
		/// </summary>
		[HttpPost("comment/hot")]
		public async Task<IActionResult> CommentHot([FromBody] CommentHotDto commentHotDto)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.CommentHot, new Dictionary<string, object> { 
				["id"] = commentHotDto.id,
				["type"] = commentHotDto.type
			});
			return Ok(json);
		}

		/// <summary>
		/// 云村热评，官方下架，暂不能用
		/// </summary>
		[HttpPost("comment/hotwall/list")]
		public async Task<IActionResult> CommentHotwallList()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.CommentHotwallList);
			return Ok(json);
		}

		/// <summary>
		/// 给评论点赞
		/// </summary>
		[HttpGet("comment/like")]
		public async Task<IActionResult> CommentLike([FromQuery] string id,
													[FromQuery] string cid,
													[FromQuery] string t,
													[FromQuery] string type,
													[FromQuery] string threadId)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.CommentLike, new Dictionary<string, object>
			{
				["id"] = id,
				["cid"] = cid,
				["t"] = t,
				["type"] = type,
				["threadId"] = threadId
			});
			return Ok(json);
		}

		/// <summary>
		/// 歌曲评论
		/// </summary>
		[HttpPost("comment/music")]
		public async Task<IActionResult> CommentMusic([FromBody] NeteaseCloudDto neteaseCloudDto)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.CommentMusic, new Dictionary<string, object> { ["id"] = neteaseCloudDto.id });
			return Ok(json);
		}

		/// <summary>
		/// mv评论
		/// </summary>
		[HttpGet("comment/mv")]
		public async Task<IActionResult> CommentMv([FromQuery] string id)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.CommentMv, new Dictionary<string, object> { ["id"] = id });
			return Ok(json);
		}

		/// <summary>
		/// 歌单评论
		/// </summary>
		[HttpGet("comment/playlist")]
		public async Task<IActionResult> CommentPlaylist([FromQuery] string id)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.CommentPlaylist, new Dictionary<string, object> { ["id"] = id });
			return Ok(json);
		}

		/// <summary>
		/// 视频评论
		/// </summary>
		[HttpGet("comment/video")]
		public async Task<IActionResult> CommentVideo([FromQuery] string id)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.CommentVideo, new Dictionary<string, object> { ["id"] = id });
			return Ok(json);
		}

		/// <summary>
		/// 签到，失败
		/// </summary>
		[HttpPost("daily_signin")]
		public async Task<IActionResult> DailySignin()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.DailySignin);
			return Ok(json);
		}

		/// <summary>
		/// 我的数字专辑
		/// </summary>
		[HttpPost("digitalAlbum/purchased")]
		public async Task<IActionResult> DigitalAlbumPurchased()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.DigitalAlbumPurchased);
			return Ok(json);
		}

		/// <summary>
		/// 电台banner
		/// </summary>
		[HttpPost("dj/banner")]
		public async Task<IActionResult> DjBanner()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.DjBanner);
			return Ok(json);
		}

		/// <summary>
		/// 电台 - 非热门类型
		/// </summary>
		[HttpPost("dj/category/excludehot")]
		public async Task<IActionResult> DjCategoryExcludehot()
		{
			await LoginCellphone();
			var json = await api.RequestAsync(CloudMusicApiProviders.DjCategoryExcludehot);
			return Ok(json);
		}

		/// <summary>
		/// 电台 - 推荐类型
		/// </summary>
		[HttpPost("dj/category/recommend")]
		public async Task<IActionResult> DjCategoryRecommend()
		{
			await LoginCellphone();
			var json = await api.RequestAsync(CloudMusicApiProviders.DjCategoryRecommend);
			return Ok(json);
		}

		/// <summary>
		/// 电台 - 分类
		/// </summary>
		[HttpPost("dj/catelist")]
		public async Task<IActionResult> DjCatelist()
		{
			await LoginCellphone();
			var json = await api.RequestAsync(CloudMusicApiProviders.DjCatelist);
			return Ok(json);
		}

		/// <summary>
		/// 电台 - 详情
		/// </summary>
		[HttpPost("dj/detail")]
		public async Task<IActionResult> DjDetail([FromBody] DjDetailDto djDetailDto)
		{
			await LoginCellphone();
			var json = await api.RequestAsync(CloudMusicApiProviders.DjDetail, new Dictionary<string, object> { ["rid"] = djDetailDto.rid });
			return Ok(json);
		}

		/// <summary>
		/// 热门电台
		/// </summary>
		[HttpPost("dj/hot")]
		public async Task<IActionResult> DjHot()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.DjHot);
			return Ok(json);
		}

		/// <summary>
		/// 电台 - 付费精选
		/// </summary>
		[HttpPost("dj/paygift")]
		public async Task<IActionResult> DjPaygift()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.DjPaygift);
			return Ok(json);
		}

		/// <summary>
		/// 电台 - 节目
		/// </summary>
		[HttpPost("dj/program")]
		public async Task<IActionResult> DjProgram([FromBody] DjProgramDto djProgramDto)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.DjProgram, new Dictionary<string, object> { ["rid"] = djProgramDto.rid });
			return Ok(json);
		}

		/// <summary>
		/// 电台 - 节目详情
		/// </summary>
		[HttpPost("dj/program/detail")]
		public async Task<IActionResult> DjProgramDetail([FromBody] NeteaseCloudDto neteaseCloudDto)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.DjProgramDetail, new Dictionary<string, object> { ["id"] = neteaseCloudDto.id });
			return Ok(json);
		}

		/// <summary>
		/// 电台 - 节目详情
		/// </summary>
		[HttpPost("dj/program/toplist")]
		public async Task<IActionResult> DjProgramToplist()
		{
			await LoginCellphone();
			var json = await api.RequestAsync(CloudMusicApiProviders.DjProgramToplist);
			return Ok(json);
		}

		/// <summary>
		/// 电台 - 24小时节目榜，失败
		/// </summary>
		[HttpPost("dj/program/toplist/hours")]
		public async Task<IActionResult> DjProgramToplistHours()
		{
			await LoginCellphone();
			var json = await api.RequestAsync(CloudMusicApiProviders.DjProgramToplistHours);
			return Ok(json);
		}

		/// <summary>
		/// 电台 - 类别热门电台
		/// </summary>
		[HttpPost("dj/radio/hot")]
		public async Task<IActionResult> DjRadioHot([FromBody] DjRadioHotDto djRadioHotDto)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.DjRadioHot, new Dictionary<string, object> { ["cateId"] = djRadioHotDto.cateId });
			return Ok(json);
		}

		/// <summary>
		/// 电台 - 推荐
		/// </summary>
		[HttpPost("dj/recommend")]
		public async Task<IActionResult> DjRecommend()
		{
			await LoginCellphone();
			var json = await api.RequestAsync(CloudMusicApiProviders.DjRecommend);
			return Ok(json);
		}

		/// <summary>
		/// 电台 - 分类推荐
		/// </summary>
		[HttpPost("dj/recommend/type")]
		public async Task<IActionResult> DjRecommendType([FromBody] DjRecommendTypeDto djRecommendTypeDto)
		{
			await LoginCellphone();
			var json = await api.RequestAsync(CloudMusicApiProviders.DjRecommendType, new Dictionary<string, object> { ["type"] = djRecommendTypeDto.type });
			return Ok(json);
		}

		/// <summary>
		/// 电台 - 订阅
		/// </summary>
		[HttpPost("dj/sub")]
		public async Task<IActionResult> DjSub([FromBody] DjSubDto djSubDto)
		{
			await LoginCellphone();
			var json = await api.RequestAsync(CloudMusicApiProviders.DjSub, new Dictionary<string, object> { ["rid"] = djSubDto.rid });
			return Ok(json);
		}

		/// <summary>
		/// 电台的订阅列表
		/// </summary>
		[HttpPost("dj/sublist")]
		public async Task<IActionResult> DjSublist()
		{
			await LoginCellphone();
			var json = await api.RequestAsync(CloudMusicApiProviders.DjSublist);
			return Ok(json);
		}

		/// <summary>
		/// 电台 - 今日优选
		/// </summary>
		[HttpPost("dj/today/perfered")]
		public async Task<IActionResult> DjTodayPerfered()
		{
			await LoginCellphone();
			var json = await api.RequestAsync(CloudMusicApiProviders.DjTodayPerfered);
			return Ok(json);
		}

		/// <summary>
		/// 电台 - 新晋电台榜/热门电台榜，失败
		/// </summary>
		[HttpPost("dj/toplist")]
		public async Task<IActionResult> DjToplist()
		{
			await LoginCellphone();
			var json = await api.RequestAsync(CloudMusicApiProviders.DjToplist);
			return Ok(json);
		}

		/// <summary>
		/// 电台 - 24小时主播榜
		/// </summary>
		[HttpPost("dj/toplist/hours")]
		public async Task<IActionResult> DjToplistHours()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.DjToplistHours);
			return Ok(json);
		}

		/// <summary>
		/// 电台 - 主播新人榜
		/// </summary>
		[HttpPost("dj/toplist/newcomer")]
		public async Task<IActionResult> DjToplistNewcomer()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.DjToplistNewcomer);
			return Ok(json);
		}

		/// <summary>
		/// 电台 - 付费精品
		/// </summary>
		[HttpPost("dj/toplist/pay")]
		public async Task<IActionResult> DjToplistPay()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.DjToplistPay);
			return Ok(json);
		}

		/// <summary>
		/// 电台 - 最热主播榜
		/// </summary>
		[HttpPost("dj/toplist/popular")]
		public async Task<IActionResult> DjToplistPopular()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.DjToplistPopular);
			return Ok(json);
		}

		/// <summary>
		/// 获取动态消息
		/// </summary>
		[HttpPost("event")]
		public async Task<IActionResult> Event([FromBody] EventDto eventDto)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.Event, new Dictionary<string, object> { 
				["pagesize"] = eventDto.pagesize,
				["lasttime"] = eventDto.lasttime
			});
			return Ok(json);
		}

		/// <summary>
		/// 删除用户动态
		/// </summary>
		[HttpPost("event/del")]
		public async Task<IActionResult> EventDel([FromBody] EventDelDto eventDelDto)
		{
			await LoginCellphone();
			var json = await api.RequestAsync(CloudMusicApiProviders.EventDel, new Dictionary<string, object> { ["evId"] = eventDelDto.evId });
			return Ok(json);
		}

		/// <summary>
		/// 转发用户动态
		/// </summary>
		[HttpPost("event/forward")]
		public async Task<IActionResult> EventForward([FromBody] EventForwardDto eventForwardDto)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.EventForward, new Dictionary<string, object>
			{
				["uid"] = uid,
				["evId"] = eventForwardDto.evId,
				["forwards"] = eventForwardDto.forwards
			});
			return Ok(json);
		}

		/// <summary>
		/// 垃圾桶
		/// </summary>
		[HttpPost("fm_trash")]
		public async Task<IActionResult> FmTrash([FromBody] NeteaseCloudDto neteaseCloudDto)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.FmTrash, new Dictionary<string, object> { ["id"] = neteaseCloudDto.id });
			return Ok(json);
		}

		/// <summary>
		/// 关注/取消关注用户
		/// </summary>
		[HttpPost("follow")]
		public async Task<IActionResult> Follow([FromBody] FollowDto followDto)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.Follow, new Dictionary<string, object> { 
				["id"] = followDto.id,
				["t"] = followDto.t
			});
			return Ok(json);
		}

		/// <summary>
		/// 获取热门话题
		/// </summary>
		[HttpPost("hot/topic")]
		public async Task<IActionResult> HotTopic()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.HotTopic);
			return Ok(json);
		}

		/// <summary>
		/// 喜欢音乐
		/// </summary>
		[HttpPost("like")]
		public async Task<IActionResult> Like([FromBody] NeteaseCloudDto neteaseCloudDto)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.Like, new Dictionary<string, object> { ["id"] = neteaseCloudDto.id });
			return Ok(json);
		}

		/// <summary>
		/// 喜欢音乐列表
		/// </summary>
		[HttpPost("likelist")]
		public async Task<IActionResult> Likelist()
		{
			await LoginCellphone();
			var json = await api.RequestAsync(CloudMusicApiProviders.Likelist, new Dictionary<string, object> { ["uid"] = uid });
			return Ok(json);
		}

		// Login相关的删除

		/// <summary>
		/// 歌词
		/// </summary>
		[HttpGet("lyric")]
		public async Task<IActionResult> Lyric([FromQuery] string id)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.Lyric, new Dictionary<string, object> { ["id"] = id });
			return Ok(json);
		}

		/// <summary>
		///	通知 - 评论
		/// </summary>
		[HttpPost("msg/comments")]
		public async Task<IActionResult> MsgComments()
		{
			await LoginCellphone();
			var json = await api.RequestAsync(CloudMusicApiProviders.MsgComments, new Dictionary<string, object> { ["uid"] = uid });
			return Ok(json);
		}

		/// <summary>
		/// 通知 - @我
		/// </summary>
		[HttpPost("msg/forwards")]
		public async Task<IActionResult> MsgForwards()
		{
			await LoginCellphone();
			var json = await api.RequestAsync(CloudMusicApiProviders.MsgForwards);
			return Ok(json);
		}

		/// <summary>
		/// 通知 - 通知
		/// </summary>
		[HttpPost("msg/notices")]
		public async Task<IActionResult> MsgNotices()
		{
			await LoginCellphone();
			var json = await api.RequestAsync(CloudMusicApiProviders.MsgNotices);
			return Ok(json);
		}

		/// <summary>
		/// 通知 - 私信
		/// </summary>
		[HttpPost("msg/private")]
		public async Task<IActionResult> MsgPrivate()
		{
			await LoginCellphone();
			var json = await api.RequestAsync(CloudMusicApiProviders.MsgPrivate);
			return Ok(json);
		}

		/// <summary>
		/// 私信内容
		/// </summary>
		[HttpPost("msg/private/history")]
		public async Task<IActionResult> MsgPrivateHistory()
		{
			await LoginCellphone();
			var json = await api.RequestAsync(CloudMusicApiProviders.MsgPrivateHistory, new Dictionary<string, object> { ["uid"] = uid });
			return Ok(json);
		}

		/// <summary>
		/// 全部 mv
		/// </summary>
		[HttpGet("mv/all")]
		public async Task<IActionResult> MvAll([FromQuery] string area,
												[FromQuery] string type,
												[FromQuery] string order,
												[FromQuery] string offset)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.MvAll, new Dictionary<string, object> { 
				["area"] = area,
				["type"] = type,
				["order"] = order,
				["offset"] = offset
			});
			return Ok(json);
		}

		/// <summary>
		/// 获取 mv 数据
		/// </summary>
		[HttpGet("mv/detail")]
		public async Task<IActionResult> MvDetail([FromQuery] string mvid)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.MvDetail, new Dictionary<string, object> { ["mvid"] = mvid });
			return Ok(json);
		}

		/// <summary>
		/// 网易出品mv
		/// </summary>
		[HttpPost("mv/exclusive/rcmd")]
		public async Task<IActionResult> MvExclusiveRcmd()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.MvExclusiveRcmd);
			return Ok(json);
		}

		/// <summary>
		/// 最新 mv
		/// </summary>
		[HttpPost("mv/first")]
		public async Task<IActionResult> MvFirst()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.MvFirst);
			return Ok(json);
		}

		/// <summary>
		/// 收藏/取消收藏 MV
		/// </summary>
		[HttpPost("mv/sub")]
		public async Task<IActionResult> MvSub([FromBody] MvSubDto mvSubDto)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.MvSub, new Dictionary<string, object> { 
				["mvid"] = mvSubDto.mvid,
				["t"] = mvSubDto.t
			});
			return Ok(json);
		}

		/// <summary>
		/// 收藏的 MV 列表
		/// </summary>
		[HttpPost("mv/sublist")]
		public async Task<IActionResult> MvSublist()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.MvSublist);
			return Ok(json);
		}

		/// <summary>
		/// mv 地址
		/// </summary>
		[HttpGet("mv/url")]
		public async Task<IActionResult> MvUrl([FromQuery] string id)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.MvUrl, new Dictionary<string, object> { ["id"] = id });
			return Ok(json);
		}

		/// <summary>
		/// 推荐歌单
		/// </summary>
		[HttpGet("personalized")]
		public async Task<IActionResult> Personalized()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.Personalized);
			return Ok(json);
		}

		/// <summary>
		/// 推荐电台
		/// </summary>
		[HttpPost("personalized/djprogram")]
		public async Task<IActionResult> PersonalizedDjprogram()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.PersonalizedDjprogram);
			return Ok(json);
		}

		/// <summary>
		/// 推荐 mv
		/// </summary>
		[HttpPost("personalized/mv")]
		public async Task<IActionResult> PersonalizedMv()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.PersonalizedMv);
			return Ok(json);
		}

		/// <summary>
		/// 推荐新音乐
		/// </summary>
		[HttpGet("personalized/newsong")]
		public async Task<IActionResult> PersonalizedNewsong()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.PersonalizedNewsong);
			return Ok(json);
		}

		/// <summary>
		/// 独家放送
		/// </summary>
		[HttpPost("personalized/privatecontent")]
		public async Task<IActionResult> PersonalizedPrivatecontent()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.PersonalizedPrivatecontent);
			return Ok(json);
		}

		/// <summary>
		/// 私人 FM
		/// </summary>
		[HttpPost("personal_fm")]
		public async Task<IActionResult> PersonalFm()
		{
			await LoginCellphone();
			var json = await api.RequestAsync(CloudMusicApiProviders.PersonalFm);
			return Ok(json);
		}

		/// <summary>
		/// 歌单分类
		/// </summary>
		[HttpGet("playlist/catlist")]
		public async Task<IActionResult> PlaylistCatlist()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.PlaylistCatlist);
			return Ok(json);
		}

		/// <summary>
		/// 新建歌单
		/// </summary>
		[HttpPost("playlist/create")]
		public async Task<IActionResult> PlaylistCreate(PlaylistCreateDto playlistCreateDto)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.PlaylistCreate, new Dictionary<string, object> { ["name"] = playlistCreateDto.name });
			return Ok(json);
		}

		/// <summary>
		/// 删除歌单
		/// </summary>
		[HttpPost("playlist/delete")]
		public async Task<IActionResult> PlaylistDelete([FromBody] NeteaseCloudDto neteaseCloudDto)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.PlaylistDelete, new Dictionary<string, object> { ["id"] = neteaseCloudDto.id });
			return Ok(json);
		}

		/// <summary>
		/// 更新歌单描述
		/// </summary>
		[HttpPost("playlist/desc/update")]
		public async Task<IActionResult> PlaylistDescUpdate([FromBody] PlaylistDescUpdateDto playlistDescUpdateDto)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.PlaylistDescUpdate, new Dictionary<string, object> { 
				["id"] = playlistDescUpdateDto.id,
				["desc"] = playlistDescUpdateDto.desc
			});
			return Ok(json);
		}

		/// <summary>
		/// 获取歌单详情
		/// </summary>
		[HttpGet("playlist/detail")]
		public async Task<IActionResult> PlaylistDetail([FromQuery] string id)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.PlaylistDetail, new Dictionary<string, object> { ["id"] = id });
			return Ok(json);
		}

		/// <summary>
		/// 热门歌单分类
		/// </summary>
		[HttpGet("playlist/hot")]
		public async Task<IActionResult> PlaylistHot()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.PlaylistHot);
			return Ok(json);
		}

		/// <summary>
		/// 更新歌单名
		/// </summary>
		[HttpPost("playlist/name/update")]
		public async Task<IActionResult> PlaylistNameUpdate([FromBody] PlaylistNameUpdateDto playlistNameUpdateDto)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.PlaylistNameUpdate, new Dictionary<string, object>
			{ 
				["id"] = playlistNameUpdateDto.id,
				["name"] = playlistNameUpdateDto.name
			});
			return Ok(json);
		}

		/// <summary>
		/// 收藏/取消收藏歌单
		/// </summary>
		[HttpGet("playlist/subscribe")]
		public async Task<IActionResult> PlaylistSubscribe([FromQuery] string t, [FromQuery] string id)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.PlaylistSubscribe, new Dictionary<string, object>
			{ 
				["t"] = t,
				["id"] = id
			});
			return Ok(json);
		}

		/// <summary>
		/// 歌单收藏者
		/// </summary>
		[HttpGet("playlist/subscribers")]
		public async Task<IActionResult> PlaylistSubscribers([FromQuery] string id)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.PlaylistSubscribers, new Dictionary<string, object> { ["id"] = id });
			return Ok(json);
		}

		/// <summary>
		/// 更新歌单标签
		/// </summary>
		[HttpPost("playlist/tags/update")]
		public async Task<IActionResult> PlaylistTagsUpdate([FromBody] PlaylistTagsUpdateDto playlistTagsUpdateDto)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.PlaylistTagsUpdate, new Dictionary<string, object> { 
				["id"] = playlistTagsUpdateDto.id,
				["tags"] = playlistTagsUpdateDto.tags
			});
			return Ok(json);
		}

		/// <summary>
		/// 对歌单添加或删除歌曲
		/// </summary>
		[HttpPost("playlist/tracks")]
		public async Task<IActionResult> PlaylistTracks([FromBody] PlaylistTracksDto playlistTracksDto)
		{
			await LoginCellphone();
			var json = await api.RequestAsync(CloudMusicApiProviders.PlaylistTracks, new Dictionary<string, object> { 
				["op"] = playlistTracksDto.op,
				["pid"] = playlistTracksDto.pid
			});
			return Ok(json);
		}

		/// <summary>
		/// 更新歌单
		/// </summary>
		[HttpPost("playlist/update")]
		public async Task<IActionResult> PlaylistUpdate([FromBody] PlaylistUpdateDto playlistUpdateDto)
		{
			await LoginCellphone();
			var json = await api.RequestAsync(CloudMusicApiProviders.PlaylistUpdate, new Dictionary<string, object> { 
				["id"] = playlistUpdateDto.id,
				["name"] = playlistUpdateDto.name,
				["desc"] = playlistUpdateDto.desc,
				["tags"] = playlistUpdateDto.tags
			});
			return Ok(json);
		}

		/// <summary>
		/// 心动模式/智能播放
		/// </summary>
		[HttpPost("playmode/intelligence/list")]
		public async Task<IActionResult> PlaymodeIntelligenceList([FromBody] PlaymodeIntelligenceListDto playmodeIntelligenceListDto)
		{
			await LoginCellphone();
			var json = await api.RequestAsync(CloudMusicApiProviders.PlaymodeIntelligenceList, new Dictionary<string, object>
			{ 
				["id"] = playmodeIntelligenceListDto.id,
				["pid"] = playmodeIntelligenceListDto.pid
			});
			return Ok(json);
		}

		/// <summary>
		/// 推荐节目
		/// </summary>
		[HttpPost("program/recommend")]
		public async Task<IActionResult> ProgramRecommend()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.ProgramRecommend);
			return Ok(json);
		}

		/// <summary>
		/// 更换绑定手机
		/// </summary>
		[HttpPost("rebind")]
		public async Task<IActionResult> Rebind([FromBody] RebindDto rebindDto)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.Rebind, new Dictionary<string, object> { 
				["oldcaptcha"] = rebindDto.oldcaptcha,
				["captcha"] = rebindDto.captcha,
				["phone"] = rebindDto.phone,
				["ctcode"] = rebindDto.ctcode
			});
			return Ok(json);
		}

		/// <summary>
		/// 每日推荐歌单
		/// </summary>
		[HttpPost("recommend/resource")]
		public async Task<IActionResult> RecommendResource()
		{
			await LoginCellphone();
			var json = await api.RequestAsync(CloudMusicApiProviders.RecommendResource);
			return Ok(json);
		}

		/// <summary>
		/// 每日推荐歌曲
		/// </summary>
		[HttpPost("recommend/songs")]
		public async Task<IActionResult> RecommendSongs()
		{
			await LoginCellphone();
			var json = await api.RequestAsync(CloudMusicApiProviders.RecommendSongs);
			return Ok(json);
		}

		/// <summary>
		/// 相关视频
		/// </summary>
		[HttpGet("related/allvideo")]
		public async Task<IActionResult> RelatedAllvideo([FromQuery] string id)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.RelatedAllvideo, new Dictionary<string, object> { ["id"] = id });
			return Ok(json);
		}
		/// <summary>
		/// 相关歌单推荐
		/// </summary>
		[HttpGet("related/playlist")]
		public async Task<IActionResult> RelatedPlaylist([FromQuery] string id)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.RelatedPlaylist, new Dictionary<string, object> { ["id"] = id });
			return Ok(json);
		}

		/// <summary>
		/// 资源点赞( MV,电台,视频)
		/// </summary>
		[HttpGet("resource/like")]
		public async Task<IActionResult> ResourceLike([FromQuery] string type,
													[FromQuery] string t,
													[FromQuery] string id)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.ResourceLike, new Dictionary<string, object> { 
				["type"] = type,
				["t"] = t,
				["id"] = id
			});
			return Ok(json);
		}

		/// <summary>
		///	听歌打卡
		/// </summary>
		[HttpPost("scrobble")]
		public async Task<IActionResult> Scrobble([FromBody] ScrobbleDto scrobbleDto)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.Scrobble, new Dictionary<string, object> { 
				["id"] = scrobbleDto.id,
				["sourceId"] = scrobbleDto.sourceid
			});
			return Ok(json);
		}

		/// <summary>
		/// 搜索
		/// </summary>
		[HttpGet("search")]
		public async Task<IActionResult> Search([FromQuery] string keywords, [FromQuery] string type)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.Search, new Dictionary<string, object> { ["keywords"] = keywords, ["type"] = type });
			return Ok(json);
		}

		/// <summary>
		/// 默认搜索关键词
		/// </summary>
		[HttpPost("search/default")]
		public async Task<IActionResult> SearchDefault()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.SearchDefault);
			return Ok(json);
		}

		/// <summary>
		/// 热搜列表(简略)
		/// </summary>
		[HttpGet("search/hot")]
		public async Task<IActionResult> SearchHot()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.SearchHot);
			return Ok(json);
		}

		/// <summary>
		/// 热搜列表(详细)
		/// </summary>
		[HttpPost("search/hot/detail")]
		public async Task<IActionResult> SearchHotDetail()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.SearchHotDetail);
			return Ok(json);
		}

		/// <summary>
		/// 搜索多重匹配
		/// </summary>
		[HttpPost("search/multimatch")]
		public async Task<IActionResult> SearchMultimatch([FromBody] SearchMultimatchDto searchMultimatchDto)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.SearchMultimatch, new Dictionary<string, object> { ["keywords"] = searchMultimatchDto.keywords });
			return Ok(json);
		}

		/// <summary>
		/// 搜索建议，失败
		/// </summary>
		[HttpGet("search/suggest")]
		public async Task<IActionResult> SearchSuggest([FromQuery] string keywords)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.SearchSuggest, new Dictionary<string, object> { ["keywords"] = keywords });
			return Ok(json);
		}

		/// <summary>
		/// 发送私信(带歌单)
		/// </summary>
		[HttpPost("send/playlist")]
		public async Task<IActionResult> SendPlaylist([FromBody] SendPlaylistDto sendPlaylistDto)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.SendPlaylist, new Dictionary<string, object> { 	
				["user_ids"] = sendPlaylistDto.user_ids,
				["msg"] = sendPlaylistDto.msg
			});
			return Ok(json);
		}

		/// <summary>
		/// 发送私信
		/// </summary>
		[HttpPost("send/text")]
		public async Task<IActionResult> SendText([FromBody] SendTextDto sendTextDto)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.SendText, new Dictionary<string, object> { 
				["user_ids"] = sendTextDto.user_ids,
				["msg"] = sendTextDto.msg
			});
			return Ok(json);
		}

		/// <summary>
		/// 设置
		/// </summary>
		[HttpPost("setting")]
		public async Task<IActionResult> Setting()
		{
			await LoginCellphone();
			var json = await api.RequestAsync(CloudMusicApiProviders.Setting);
			return Ok(json);
		}

		/// <summary>
		/// 分享歌曲、歌单、mv、电台、电台节目到动态
		/// </summary>
		[HttpPost("share/resource")]
		public async Task<IActionResult> ShareResource([FromBody] NeteaseCloudDto neteaseCloudDto)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.ShareResource, new Dictionary<string, object> { ["id"] = neteaseCloudDto.id });
			return Ok(json);
		}

		/// <summary>
		/// 获取相似歌手
		/// </summary>
		[HttpGet("simi/artist")]
		public async Task<IActionResult> SimiArtist([FromQuery] string id)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.SimiArtist, new Dictionary<string, object> { ["id"] = id });
			return Ok(json);
		}

		/// <summary>
		/// 相似 mv
		/// </summary>
		[HttpGet("simi/mv")]
		public async Task<IActionResult> SimiMv([FromQuery] string mvid)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.SimiMv, new Dictionary<string, object> { ["mvid"] = mvid });
			return Ok(json);
		}

		/// <summary>
		/// 获取相似歌单
		/// </summary>
		[HttpGet("simi/playlist")]
		public async Task<IActionResult> SimiPlaylist([FromQuery] string id)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.SimiPlaylist, new Dictionary<string, object> { ["id"] = id });
			return Ok(json);
		}

		/// <summary>
		/// 获取相似音乐
		/// </summary>
		[HttpPost("simi/song")]
		public async Task<IActionResult> SimiSong([FromBody] NeteaseCloudDto neteaseCloudDto)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.SimiSong, new Dictionary<string, object> { ["id"] = neteaseCloudDto.id });
			return Ok(json);
		}

		/// <summary>
		/// 获取最近 5 个听了这首歌的用户
		/// </summary>
		[HttpPost("simi/user")]
		public async Task<IActionResult> SimiUser([FromBody] NeteaseCloudDto neteaseCloudDto)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.SimiUser, new Dictionary<string, object> { ["id"] = neteaseCloudDto.id });
			return Ok(json);
		}

		/// <summary>
		/// 获取歌曲详情
		/// </summary>
		[HttpGet("song/detail")]
		public async Task<IActionResult> SongDetail([FromQuery] string ids)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.SongDetail, new Dictionary<string, object> { ["ids"] = ids });
			return Ok(json);
		}

		/// <summary>
		/// 获取音乐 url
		/// </summary>
		[HttpPost("song/url")]
		public async Task<IActionResult> SongUrl([FromBody] SongUrlDto songUrlDto)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.SongUrl, new Dictionary<string, object> { ["id"] = songUrlDto.id });
			return Ok(json);
		}

		/// <summary>
		/// 所有榜单
		/// </summary>
		[HttpPost("toplist")]
		public async Task<IActionResult> Toplist()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.Toplist);
			return Ok(json);
		}

		/// <summary>
		/// 歌手榜
		/// </summary>
		[HttpPost("toplist/artist")]
		public async Task<IActionResult> ToplistArtist()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.ToplistArtist);
			return Ok(json);
		}

		/// <summary>
		/// 所有榜单内容摘要
		/// </summary>
		[HttpGet("toplist/detail")]
		public async Task<IActionResult> ToplistDetail()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.ToplistDetail);
			return Ok(json);
		}

		/// <summary>
		/// 新碟上架
		/// </summary>
		[HttpPost("top/album")]
		public async Task<IActionResult> TopAlbum()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.TopAlbum);
			return Ok(json);
		}

		/// <summary>
		/// 热门歌手
		/// </summary>
		[HttpGet("top/artists")]
		public async Task<IActionResult> TopArtists()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.TopArtists);
			return Ok(json);
		}

		/// <summary>
		/// 排行榜，deplicated
		/// </summary>
		[HttpPost("top/list")]
		public async Task<IActionResult> Top_List([FromBody] NeteaseCloudDto neteaseCloudDto)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.Top_List, new Dictionary<string, object> { ["id"] = neteaseCloudDto.id });
			return Ok(json);
		}

		/// <summary>
		/// mv 排行
		/// </summary>
		[HttpPost("top/mv")]
		public async Task<IActionResult> TopMv()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.TopMv);
			return Ok(json);
		}

		/// <summary>
		/// 歌单 ( 网友精选碟 )
		/// </summary>
		[HttpGet("top/playlist")]
		public async Task<IActionResult> TopPlaylist([FromQuery] string order,
													[FromQuery] string cat,
													[FromQuery] string offset)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.TopPlaylist, new Dictionary<string, object> { 
				["order"] = order,
				["cat"] = cat,
				["offset"] = offset
			});
			return Ok(json);
		}

		/// <summary>
		/// 获取精品歌单
		/// </summary>
		[HttpPost("top/playlist/highquality")]
		public async Task<IActionResult> TopPlaylistHighquality()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.TopPlaylistHighquality);
			return Ok(json);
		}

		/// <summary>
		/// 新歌速递
		/// </summary>
		[HttpPost("top/song")]
		public async Task<IActionResult> TopSong([FromBody] TopSongDto topSongDto)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.TopSong, new Dictionary<string, object> { ["type"] = topSongDto.type });
			return Ok(json);
		}

		/// <summary>
		/// 用户电台
		/// </summary>
		[HttpPost("user/audio")]
		public async Task<IActionResult> UserAudio()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.UserAudio, new Dictionary<string, object> { ["uid"] = uid });
			return Ok(json);
		}

		/// <summary>
		/// 云盘
		/// </summary>
		[HttpPost("user/cloud")]
		public async Task<IActionResult> UserCloud()
		{
			await LoginCellphone();
			var json = await api.RequestAsync(CloudMusicApiProviders.UserCloud);
			return Ok(json);
		}

		/// <summary>
		/// 云盘歌曲删除
		/// </summary>
		[HttpPost("user/cloud/del")]
		public async Task<IActionResult> UserCloudDel([FromBody] NeteaseCloudDto neteaseCloudDto)
		{
			await LoginCellphone();
			var json = await api.RequestAsync(CloudMusicApiProviders.UserCloudDel, new Dictionary<string, object> { ["id"] = neteaseCloudDto.id });
			return Ok(json);
		}

		/// <summary>
		/// 云盘数据详情
		/// </summary>
		[HttpPost("user/cloud/detail")]
		public async Task<IActionResult> UserCloudDetail([FromBody] NeteaseCloudDto neteaseCloudDto)
		{
			await LoginCellphone();
			var json = await api.RequestAsync(CloudMusicApiProviders.UserCloudDetail, new Dictionary<string, object> { ["id"] = neteaseCloudDto.id });
			return Ok(json);
		}

		/// <summary>
		/// 获取用户详情
		/// </summary>
		[HttpGet("user/detail")]
		public async Task<IActionResult> UserDetail([FromQuery] string uid)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.UserDetail, new Dictionary<string, object> { ["uid"] = NeteaseCloudMusicController.uid });
			return Ok(json);
		}

		/// <summary>
		/// 获取用户电台
		/// </summary>
		/// <returns></returns>
		[HttpPost("user/dj")]
		public async Task<IActionResult> UserDj()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.UserDj, new Dictionary<string, object> { ["uid"] = uid });
			return Ok(json);
		}

		/// <summary>
		/// 获取用户动态
		/// </summary>
		[HttpPost("user/event")]
		public async Task<IActionResult> UserEvent()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.UserEvent, new Dictionary<string, object> { ["uid"] = uid });
			return Ok(json);
		}

		/// <summary>
		/// 获取用户粉丝列表
		/// </summary>
		[HttpPost("user/followeds")]
		public async Task<IActionResult> UserFolloweds()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.UserFolloweds, new Dictionary<string, object> { ["uid"] = uid });
			return Ok(json);
		}

		/// <summary>
		/// 获取用户关注列表
		/// </summary>
		[HttpPost("user/follows")]
		public async Task<IActionResult> UserFollows()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.UserFollows, new Dictionary<string, object> { ["uid"] = uid });
			return Ok(json);
		}

		/// <summary>
		/// 获取用户歌单
		/// </summary>
		[HttpGet("user/playlist")]
		public async Task<IActionResult> UserPlaylist([FromQuery] string uid)
		{
			await LoginCellphone();
			var json = await api.RequestAsync(CloudMusicApiProviders.UserPlaylist, new Dictionary<string, object> { ["uid"] = NeteaseCloudMusicController.uid });
			return Ok(json);
		}

		/// <summary>
		/// 获取用户播放记录
		/// </summary>
		[HttpGet("user/record")]
		public async Task<IActionResult> UserRecord([FromQuery] string uid, [FromQuery] string type)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.UserRecord, new Dictionary<string, object> { ["uid"] = NeteaseCloudMusicController.uid, ["type"] = type });
			return Ok(json);
		}

		/// <summary>
		/// 获取用户信息 , 歌单，收藏，mv, dj 数量
		/// </summary>
		[HttpPost("user/subcount")]
		public async Task<IActionResult> UserSubcount()
		{
			await LoginCellphone();
			var json = await api.RequestAsync(CloudMusicApiProviders.UserSubcount);
			return Ok(json);
		}

		/// <summary>
		/// 更新用户信息
		/// </summary>
		/// <param name="userUpdateDto"></param>
		/// <returns></returns>
		[HttpPost("user/update")]
		public async Task<IActionResult> UserUpdate([FromBody] UserUpdateDto userUpdateDto)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.UserUpdate, new Dictionary<string, object> { 
				["gender"] = userUpdateDto.gender,
				["birthday"] = userUpdateDto.birthday,
				["nickname"] = userUpdateDto.nickname,
				["province"] = userUpdateDto.province,
				["city"] = userUpdateDto.city,
				["signature"] = userUpdateDto.signature
			});
			return Ok(json);
		}

		/// <summary>
		/// 视频详情
		/// </summary>
		[HttpGet("video/detail")]
		public async Task<IActionResult> VideoDetail([FromQuery] string id)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.VideoDetail, new Dictionary<string, object> { ["id"] = id });
			return Ok(json);
		}

		/// <summary>
		/// 获取视频标签下的视频，失败
		/// </summary>
		[HttpGet("video/group")]
		public async Task<IActionResult> VideoGroup([FromQuery] string id)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.VideoGroup, new Dictionary<string, object> { ["id"] = id });
			return Ok(json);
		}

		/// <summary>
		/// 获取视频标签列表
		/// </summary>
		[HttpGet("video/group/list")]
		public async Task<IActionResult> VideoGroupList()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.VideoGroupList);
			return Ok(json);
		}

		/// <summary>
		/// 收藏视频
		/// </summary>
		[HttpPost("video/sub")]
		public async Task<IActionResult> VideoSub([FromBody] VideoSubDto videoSubDto)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.VideoSub, new Dictionary<string, object> { 
				["id"] = videoSubDto.id,
				["t"] = videoSubDto.t
			});
			return Ok(json);
		}

		/// <summary>
		/// 获取视频播放地址，失败
		/// </summary>
		[HttpGet("video/url")]
		public async Task<IActionResult> VideoUrl([FromQuery] string id)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.VideoUrl, new Dictionary<string, object> { ["id"] = id });
			return Ok(json);
		}

		/// <summary>
		/// 获取视频分类列表
		/// </summary>
		[HttpGet("video/category/list")]
		public async Task<IActionResult> VideoCategoryList()
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.VideoCategoryList);
			return Ok(json);
		}

		/// <summary>
		/// 获取全部视频列表
		/// </summary>
		[HttpGet("video/timeline/all")]
		public async Task<IActionResult> VideoTimelineAll()
        {
			var json = await api.RequestAsync(CloudMusicApiProviders.VideoTimelineAll);
			return Ok(json);
        }

		/// <summary>
		/// 获取视频点赞转发评论数数据
		/// </summary>
		[HttpGet("video/detail/info")]
		public async Task<IActionResult> VideoDetailInfo([FromQuery] string vid)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.VideoTimelineAll, new Dictionary<string, object> { ["vid"] = vid });
			return Ok(json);
		}

		/// <summary>
		/// 获取 mv 点赞转发评论数数据
		/// </summary>
		[HttpGet("mv/detail/info")]
		public async Task<IActionResult> MvDetailInfo([FromQuery] string mvid)
		{
			var json = await api.RequestAsync(CloudMusicApiProviders.VideoTimelineAll, new Dictionary<string, object> { ["mvid"] = mvid });
			return Ok(json);
		}
	}
}
