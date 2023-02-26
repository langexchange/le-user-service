using System;
using System.Collections.Generic;

namespace LE.UserService.Models.Responses
{
    public class PostRespond
    {
        public Guid PostId { get; set; }
        public Guid LangId { get; set; }
        public string LangName { get; set; }
        public string Text { get; set; }
        public string Label { get; set; }
        public bool IsTurnOffComment { get; set; }
        public bool IsTurnOffCorrection { get; set; }
        public bool IsTurnOffShare { get; set; }
        public List<FileOfPostRespond> ImagePost { get; set; }
        public List<FileOfPostRespond> AudioPost { get; set; }
        public List<FileOfPostRespond> VideoPost { get; set; }
    }

    public class FileOfPostRespond
    {
        public string Type { get; set; }
        public string Url { get; set; }
    }
}
