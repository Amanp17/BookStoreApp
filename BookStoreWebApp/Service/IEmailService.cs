using BookStoreWebApp.Models;
using System.Threading.Tasks;

namespace BookStoreWebApp.Service
{
    public interface IEmailService
    {
        Task SendTestEmail(UserEmailOptions userEmailOptions);
        Task SendEmailConfirmation(UserEmailOptions userEmailOptions);
    }
}