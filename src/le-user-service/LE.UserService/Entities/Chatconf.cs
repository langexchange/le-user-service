using System;
using System.Collections.Generic;
using System.Collections;

#nullable disable

namespace LE.UserService.Entities
{
    public partial class Chatconf
    {
        public Chatconf()
        {
            Arrangements = new HashSet<Arrangement>();
        }

        public int Confid { get; set; }
        public BitArray Ismute { get; set; }
        public BitArray Isblock { get; set; }
        public int? Sender { get; set; }
        public int? Receiver { get; set; }

        public virtual User ReceiverNavigation { get; set; }
        public virtual User SenderNavigation { get; set; }
        public virtual ICollection<Arrangement> Arrangements { get; set; }
    }
}
