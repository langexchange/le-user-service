using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Entities
{
    public partial class Correctmsg
    {
        public int Messid { get; set; }
        public string CorrectText { get; set; }

        public virtual Message Mess { get; set; }
    }
}
