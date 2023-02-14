using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Hobby
    {
        public Hobby()
        {
            Userhobbies = new HashSet<Userhobby>();
        }

        public Guid Hobbyid { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Userhobby> Userhobbies { get; set; }
    }
}
