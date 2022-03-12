using BookStoreWebApp.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BookStoreWebApp.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> CreateUserAsync(SignUpDTO UserModel);
        Task<SignInResult> PasswordSignInAsync(LoginDTO login);
        Task SignOut();
        Task<IdentityResult> ChangePasswordAsync(ChangePasswordDTO changePassword);
    }    
}