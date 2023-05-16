using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Statisticallearningprocess
    {
        public Guid Id { get; set; }
        public Guid Userid { get; set; }
        public string Packageids { get; set; }
        public int? Percent { get; set; }
        public int? Totalvocabs { get; set; }
        public int? Currentvocabs { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual User User { get; set; }
    }
}
