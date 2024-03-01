using VideoSharingPlatform.Core.Entities.VideoAggregate;

namespace VideoSharingPlatform.Web.Dtos;

public class VideoResponse {
    public VideoResponse(Video video, int commentsCount, Reaction? reaction)
    {
        Video = video;
        CommentsCount = commentsCount;
        Reaction = reaction;

    }

    public Video Video { get; set; }

    public Reaction? Reaction { get; set; }


    public int CommentsCount { get; set; }
}