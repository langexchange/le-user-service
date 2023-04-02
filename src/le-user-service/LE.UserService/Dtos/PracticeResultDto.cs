using System;

namespace LE.UserService.Dtos
{
    public class PracticeResultDto
    {
        public Guid PackageId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int TotalVocabs { get; set; }
        public int CurrentNumOfVocab{ get; set; }
    }
}
