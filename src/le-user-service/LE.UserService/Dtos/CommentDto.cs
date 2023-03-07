using System;
using System.Collections.Generic;

namespace LE.UserService.Dtos
{
    public class CommentDto
    {
        public CommentDto()
        {
            Audiocmts = new List<FileOfComment>();
            Imagecmts = new List<FileOfComment>();
        }
        public Guid CommentId { get; set; }
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public string Text { get; set; }
        public string Correctcmt { get; set; }
        public int NumOfInteract { get; set; }
        public List<FileOfComment> Audiocmts { get; set; }
        public List<FileOfComment> Imagecmts { get; set; }

    }

    public class FileOfComment
    {
        public string Type { get; set; }
        public string Url { get; set; }
    }
}
