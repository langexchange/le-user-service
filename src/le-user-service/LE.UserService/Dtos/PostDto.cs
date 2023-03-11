using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LE.UserService.Dtos
{
    public class PostDto
    {
        public PostDto()
        {
            ImagePost = new List<FileOfPost>();
            AudioPost = new List<FileOfPost>();
            VideoPost = new List<FileOfPost>();
            UserInfo = new UserInfo();
        }
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public Guid LangId { get; set; }
        public string LangName { get; set; }
        public string Text { get; set; }
        public string Label { get; set; }
        public List<string> Labels => Label?.Split(Env.SplitChar).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
        public bool IsPublic { get; set; }
        public bool IsTurnOffComment { get; set; }
        public bool IsTurnOffCorrection { get; set; }
        public bool IsTurnOffShare { get; set; }
        public int NumOfInteract { get; set; }
        public bool IsUserInteracted { get; set; } = false;
        public int NumOfCmt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public UserInfo UserInfo { get; set; }
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
