using System;
using System.Collections.Generic;
using System.Collections;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Chatconf
    {
        public Chatconf()
        {
            Arrangements = new HashSet<Arrangement>();
        }

        public Guid Confid { get; set; }
        public BitArray Ismute { get; set; }
        public BitArray Isblock { get; set; }
        public Guid? Sender { get; set; }
        public Guid? Receiver { get; set; }

        public virtual User ReceiverNavigation { get; set; }
        public virtual User SenderNavigation { get; set; }
        public virtual ICollection<Arrangement> Arrangements { get; set; }
    }
}
