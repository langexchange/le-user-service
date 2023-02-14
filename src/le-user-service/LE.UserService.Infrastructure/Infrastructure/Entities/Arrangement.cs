﻿using System;
using System.Collections.Generic;
using System.Collections;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Arrangement
    {
        public Guid Arrangeid { get; set; }
        public Guid Confid { get; set; }
        public DateTime? TimeStarted { get; set; }
        public DateTime? TimeEnded { get; set; }
        public BitArray IsAccepted { get; set; }

        public virtual Chatconf Conf { get; set; }
    }
}
