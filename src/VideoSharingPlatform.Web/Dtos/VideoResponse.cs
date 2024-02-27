using VideoSharingPlatform.Core.Entities.VideoAggregate;

namespace VideoSharingPlatform.Web.Dtos;

public class VideoResponse {
    public VideoResponse(Video video, int commentsCount)
    {
        Video = video;
        CommentsCount = commentsCount;
    }

    public Video Video { get; set; }


    public int CommentsCount { get; set; }
}