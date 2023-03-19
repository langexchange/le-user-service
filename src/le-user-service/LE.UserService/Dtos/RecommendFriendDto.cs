using Newtonsoft.Json;

namespace LE.UserService.Dtos
{
    public class RecommendFriendDto
    {
        public string[] NativeLangs { get; set; }
        public string[] TargetLangs { get; set; }
        public string CountryCode { get; set; }
    }
}
