using BookStoreWebApp.Data;
using BookStoreWebApp.Models;
using BookStoreWebApp.Service;
using Microsoft.AspNetCore.Identity;
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

        public AccountRepository(UserManager<AccountUser> User,SignInManager<AccountUser> signInManager,
            IUserService userService)
        {
            _user = User;
            _signInManager = signInManager;
            _userService = userService;
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
    }
}
