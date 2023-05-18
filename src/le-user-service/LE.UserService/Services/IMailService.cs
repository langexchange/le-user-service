using LE.UserService.Models.Requests;
using System.Threading.Tasks;

namespace LE.UserService.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
