using LE.UserService.Dtos;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Services
{
    public interface IPostService
    {
        Task CreatePost(PostDto postDto, CancellationToken cancellationToken = default);
    }
}
