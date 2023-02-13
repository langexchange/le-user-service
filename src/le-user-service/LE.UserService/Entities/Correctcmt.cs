using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Entities
{
    public partial class Correctcmt
    {
        public int Commentid { get; set; }
        public string CorrectText { get; set; }

        public virtual Comment Comment { get; set; }
    }
}
