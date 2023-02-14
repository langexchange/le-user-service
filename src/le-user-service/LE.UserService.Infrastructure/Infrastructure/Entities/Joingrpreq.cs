﻿using System;
using System.Collections.Generic;
using System.Collections;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Joingrpreq
    {
        public int Userid { get; set; }
        public int Groupid { get; set; }
        public BitArray IsRemoved { get; set; }

        public virtual Group Group { get; set; }
        public virtual User User { get; set; }
    }
}
