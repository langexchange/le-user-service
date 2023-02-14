using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Vocabgoal
    {
        public int Goalid { get; set; }
        public int? Amount { get; set; }
        public string Type { get; set; }
    }
}
