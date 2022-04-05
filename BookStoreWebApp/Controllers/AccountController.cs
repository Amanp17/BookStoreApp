using BookStoreWebApp.Models;
using BookStoreWebApp.Repository;
using BookStoreWebApp.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        [Route("signup")]
        public IActionResult SignUp(bool IsSuccess = false)
        {
            //Sending Success Message for Displaying the alert
            ViewBag.IsSuccess = IsSuccess;
            return View();
        }
        [Route("signup")]
        [HttpPost]
        public async Task<IActionResult> SignupPost(SignUpDTO signup)
        {
            //Checks For Model Validation
            if (ModelState.IsValid)
            {
                //Execute the Repository logic
                var result = await _accountRepository.CreateUserAsync(signup);
                if (!result.Succeeded)
                {
                    //Checks for the result error
                    foreach(var errormessage in result.Errors)
                    {
                        ModelState.AddModelError("", errormessage.Description);
                    }
                    return View(nameof(SignUp));
                }
                return RedirectToAction(nameof(SignUp), new { IsSuccess = true });
            }
            return View(nameof(SignUp));
        }

        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO login,string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.PasswordSignInAsync(login);
                if (result.Succeeded)
                {
                    //If there is an url and result is Succeccd then it will redirect to local URL
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        //If there Exists some local URL in the Query String then Redirect to that URL
                        return LocalRedirect(returnUrl);
                    }
                    //If LoggedIn Successfully Then Enter Into Home Page
                    return RedirectToAction("Index", "Home");
                }
                if (result.IsNotAllowed)
                {
                    ModelState.AddModelError("", "User Not Allowed");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Credintial");
                }
            }
            return View(nameof(login));
        }

        [Route("logout")]
        public async Task<IActionResult> SignOutAsync()
        {
            await _accountRepository.SignOut();
            return RedirectToAction(nameof(Login));
        }

        [Route("change-password")]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO changePassword)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.ChangePasswordAsync(changePassword);
                if (result.Succeeded)
                {
                    ModelState.Clear();
                    ViewBag.IsSuccess = true;
                    return View();
                }
                foreach(var errormessage in result.Errors)
                {
                    ModelState.AddModelError("", errormessage.Description);
                }
            }
            return View(changePassword);
        }
        [HttpGet("confirm-email")]
        public async Task<ActionResult> ConfirmEmail(string Uid, string token)
        {
            if(!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(Uid))
            {
                token = token.Replace(' ', '+');
                var result = await _accountRepository.ConfirmEmailAsync(Uid, token);
                if (result.Succeeded)
                {
                    ViewBag.IsSuceess = true;
                }
            }
            return View();
        }


    }
    
}

