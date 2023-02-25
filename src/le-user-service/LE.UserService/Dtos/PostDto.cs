using System;
using System.Collections.Generic;

namespace LE.UserService.Dtos
{
    public class PostDto
    {
        public PostDto()
        {
            ImagePost = new List<FileOfPost>();
            AudioPost = new List<FileOfPost>();
            VideoPost = new List<FileOfPost>();
        }
        public Guid UserId { get; set; }
        public Guid LangId { get; set; }
        public string Text { get; set; }
        public string Label { get; set; }
        public List<FileOfPost> ImagePost { get; set; }
        public List<FileOfPost> AudioPost { get; set; }
        public List<FileOfPost> VideoPost { get; set; }
    }

    public class FileOfPost
    {
        public string Type { get; set; }
        public string Url { get; set; }
    }
}
