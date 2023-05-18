using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Message
    {
        public Guid Messid { get; set; }
        public Guid? Sender { get; set; }
        public Guid? Receiver { get; set; }
        public string Text { get; set; }

        public virtual User ReceiverNavigation { get; set; }
        public virtual User SenderNavigation { get; set; }
        public virtual Audiomsg Audiomsg { get; set; }
        public virtual Correctmsg Correctmsg { get; set; }
        public virtual Imagemsg Imagemsg { get; set; }
    }
}
