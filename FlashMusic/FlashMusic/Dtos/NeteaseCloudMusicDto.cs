using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashMusic.Dtos
{
    public class AlbumDto
    {
        public long id { get; set; }
    }

    public class AlbumDetailDynamicDto
    {
        public long id { get; set; }
    }

    public class AlbumSubDto
    {
        public int t { get; set; }
        public long id { get; set; }
    }

    public class ArtistsDto
    {
        public long id { get; set; }
    }

    public class ArtistAlbumDto
    {
        public long id { get; set; }
    }

    public class CaptchaSentDto
    {
        public long phone { get; set; } 
    }

    public class CaptchaVerifyDto
    {
        public long phone { get; set; }
        public long captcha { get; set; }
    }

    public class CellphoneExistenceCheckDto
    {
        public long phone { get; set; }
    }

    public class CommentDto
    {
        public int t { get; set; }
        public int type { get; set; }
        public long id { get; set; }
        public string content { get; set; }
        public int commentId { get; set; }
    }

    public class CommentEventDto
    {
        public string threadId { get; set; }
    }

    public class CommentHotDto
    {
        public long id { get; set; }
        public int type { get; set; }
    }

    public class CommentLikeDto
    {
        public long id { get; set; }
        public long cid { get; set; }
        public int t { get; set; }
        public int type { get; set; }
        public long threadId { get; set; }
    }

    public class CommentVideoDto
    {
        public string id { get; set; }
    }

    public class DjDetailDto
    {
        public long rid { get; set; }
    }

    public class DjProgramDto
    {
        public long rid { get; set; }
    }

    public class DjRecommendTypeDto
    {
        public int type { get; set; }
    }

    public class DjSubDto
    {
        public long rid { get; set; }
    }

    public class DjRadioHotDto
    {
        public long cateId { get; set; }
    }

    public class EventDto
    {
        public long pagesize { get; set; }
        public long lasttime { get; set; }
    }

    public class EventDelDto
    {
        public long evId { get; set; }
    }

    public class EventForwardDto
    {
        public long uid { get; set; }
        public long evId { get; set; }
        public string forwards { get; set; }
    }

    public class FollowDto
    {
        public long id { get; set; }
        public int t { get; set; }
    }

    public class MvDetailDto
    {
        public long mvid { get; set; }
    }

    public class MvSubDto
    {
        public long mvid { get; set; }
        public int t { get; set; }
    }

    public class PlaylistCreateDto
    {
        public string name { get; set; }
    }

    public class PlaylistDescUpdateDto
    {
        public long id { get; set; }
        public string desc { get; set; }
    }

    public class PlaylistNameUpdateDto
    {
        public long id { get; set; }
        public string name { get; set; }
    }

    public class PlaylistSubscribeDto
    {
        public int t { get; set; }
        public long id { get; set; }
    }

    public class PlaylistTagsUpdateDto
    {
        public long id { get; set; }
        public string tags { get; set; }
    }

    public class PlaylistTracksDto
    {
        public string op { get; set; }
        public long pid { get; set; }
    }

    public class PlaylistUpdateDto
    {
        public long id { get; set; }
        public string name { get; set; }
        public string desc { get; set; }
        public string tags { get; set; }
    }

    public class PlaymodeIntelligenceListDto
    {
        public long id { get; set; }
        public long pid { get; set; }
    }

    public class RebindDto
    {
        public long oldcaptcha { get; set; }
        public long captcha { get; set; }
        public long phone { get; set; }
        public long ctcode { get; set; }
    }
    public class ResourceLikeDto
    {
        public int type { get; set; }
        public int t { get; set; }
        public long id { get; set; }
    }

    public class RelatedAllvideoDto
    {
        public string id { get; set; }
    }

    public class ScrobbleDto
    {
        public long id { get; set; }
        public long sourceid { get; set; }
    }

    public class SearchDto
    {
        public string keywords { get; set; }
    }

    public class SearchMultimatchDto
    {
        public string keywords { get; set; }
    }

    public class SearchSuggestDto
    {
        public string keywords { get; set; }
    }

    public class SendPlaylistDto
    {
        public string user_ids { get; set; }
        public string msg { get; set; }
    }

    public class SendTextDto
    {
        public string user_ids { get; set; }
        public string msg { get; set; }
    }

    public class SimiMvDto
    {
        public string mvid { get; set; }
    }

    public class SongDetailDto
    {
        public string ids { get; set; }
    }

    public class SongUrlDto
    {
        public string id { get; set; }
    }

    public class TopSongDto
    {
        public int type { get; set; }
    }

    public class UserUpdateDto
    {
        public int gender { get; set; }
        public string birthday { get; set; }
        public string nickname { get; set; }
        public long province { get; set; }
        public long city { get; set; }
        public string signature { get; set; }
    }

    public class VideoSubDto
    {
        public long id { get; set; }
        public int t { get; set; }
    }

    public class VideoDetailDto
    {
        public string id { get; set; }
    }

    public class VideoUrlDto
    {
        public string id { get; set; }
    }


    // 以下采用通用Dto
    public class NeteaseCloudDto
    {
        public long id { get; set; }
    }

    public class NeteaseCloudDto2
    {
        public long id { get; set; }
        public int t { get; set; }
    }
}
