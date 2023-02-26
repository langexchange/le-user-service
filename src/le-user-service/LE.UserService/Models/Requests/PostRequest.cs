using System;
using System.Collections.Generic;

namespace LE.UserService.Models.Requests
{
    public class PostRequest
    {
        public Guid LangId { get; set; }
        public string Text { get; set; }
        public string Label { get; set; }
        public bool IsTurnOffComment { get; set; } = false;
        public bool IsTurnOffCorrection { get; set; } = false;
        public bool IsTurnOffShare { get; set; } = false;
        public bool IsPublic { get; set; } = true;
        public List<FileOfPostRequest> ImagePost { get; set;}
        public List<FileOfPostRequest> AudioPost { get; set;}
        public List<FileOfPostRequest> VideoPost { get; set;}

    }
    public class FileOfPostRequest
    {
        public string Type { get; set; }
        public string Url { get; set; }
    }
}
