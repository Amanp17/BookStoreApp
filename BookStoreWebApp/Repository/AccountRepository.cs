using BookStoreWebApp.Data;
using BookStoreWebApp.Models;
using BookStoreWebApp.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreWebApp.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<AccountUser> _user;
        private readonly SignInManager<AccountUser> _signInManager;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public AccountRepository(UserManager<AccountUser> User,SignInManager<AccountUser> signInManager,
            IUserService userService,IEmailService emailService,IConfiguration configuration)
        {
            _user = User;
            _signInManager = signInManager;
            _userService = userService;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<IdentityResult> CreateUserAsync(SignUpDTO UserModel)
        {
            //Create an Identity User Containing UserName and Email
            AccountUser Users = new AccountUser()
            {
                FirstName = UserModel.FirstName,
                LastName = UserModel.LastName,
                UserName = UserModel.Email,
                Email = UserModel.Email
            };
            //CreateAsync Method will Create the user with its Password
            var result = await _user.CreateAsync(Users, UserModel.Password);
            if (result.Succeeded)
            {
                var token = await _user.GenerateEmailConfirmationTokenAsync(Users);
                if (!string.IsNullOrEmpty(token))
                {
                    await SendEmailConfirmation(Users, token);
                }
            }

            //return type will be IdentityResult
            return result;
        }
        public async Task<SignInResult> PasswordSignInAsync(LoginDTO login)
        {
            //Signinmanager Contains Method PasswordsSignInAsync Which will Attempt to sign in the UserName and Password Combination
            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, login.Rememberme, false);
            return result;
        }

        public async Task SignOut()
        {
            //SignOutAsync Method of SignInManager will Signs out the User from the Application
            await _signInManager.SignOutAsync();
        }
        public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordDTO changePassword)
        {
            //UserId will store the ID of the logged in Person using HttpContext
            var userId = _userService.GetUserId();
            //User
            var User = await _user.FindByIdAsync(userId);   //Find and return an User having userId
            //Changes Password of User having Current Password and New Password
            var result = await _user.ChangePasswordAsync(User, changePassword.CurrentPassword, changePassword.NewPassword);
            return result;
        }
        public async Task<IdentityResult> ConfirmEmailAsync(string Uid,string token)
        {
            var user = await _user.FindByIdAsync(Uid);
            var result = await _user.ConfirmEmailAsync(user, token);
            return result;
        }
        private async Task SendEmailConfirmation(AccountUser accountUser,string token)
        {
            string appDomain = _configuration.GetSection("Application:AppDomain").Value;
            string confirmationLink = _configuration.GetSection("Application:EmailConfirmation").Value;
            UserEmailOptions userEmailOptions = new UserEmailOptions
            {
                ToEmail = new List<string>() { accountUser.Email },
                PlaceHolder = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}",accountUser.FirstName+" "+accountUser.LastName),
                    new KeyValuePair<string, string>("{{Link}}",string.Format(appDomain + confirmationLink,accountUser.Id,token))
                }
            };

            await _emailService.SendEmailConfirmation(userEmailOptions);
        }
    }
}
