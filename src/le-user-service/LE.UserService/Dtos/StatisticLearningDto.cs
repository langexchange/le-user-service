using System;

namespace LE.UserService.Dtos
{
    public class StatisticLearningDto
    {
        public int Month { get; set; }
        public int? Percent { get; set; }
        public int? Totalvocabs { get; set; }
        public int? Currentvocabs { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
