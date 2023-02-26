using System;
using System.Collections.Generic;

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
        public Guid? Sender { get; set; }
        public Guid? Receiver { get; set; }
        public bool? Ismute { get; set; }
        public bool? Isblock { get; set; }

        public virtual User ReceiverNavigation { get; set; }
        public virtual User SenderNavigation { get; set; }
        public virtual ICollection<Arrangement> Arrangements { get; set; }
    }
}
