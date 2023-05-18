using System.Collections.Generic;

namespace LE.UserService.Models.Requests
{
    public class CommentRequest
    {
        public string Text { get; set; }

        public string Correctcmt { get; set; }
        public List<FileOfCommentRequest> Audiocmts { get; set; }
        public List<FileOfCommentRequest> Imagecmts { get; set; }
    }

    public class FileOfCommentRequest
    {
        public string Type { get; set; }
        public string Url { get; set; }
    }
}
