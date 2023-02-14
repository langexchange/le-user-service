using System;
using System.Collections.Generic;
using System.Collections;

#nullable disable

namespace LE.UserService.Infrastructure.Entities
{
    public partial class Tutorreq
    {
        public int Requestid { get; set; }
        public int Ownerid { get; set; }
        public string Text { get; set; }
        public BitArray IsQualified { get; set; }

        public virtual User Owner { get; set; }
    }
}
