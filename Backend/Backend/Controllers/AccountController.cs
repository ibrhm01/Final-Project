using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Helpers;
using Backend.Models;
using Backend.Services.Interfaces;
using Backend.ViewModels;
using Backend.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Controllers
{
    public class AccountController : Controller
    {

        public readonly UserManager<AppUser> _userManager;
        public readonly SignInManager<AppUser> _signInManager;
        public readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly IFileService _fileService;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IEmailService emailService, IFileService fileService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _fileService = fileService;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View();

            AppUser user = new();
            user.FullName = registerVM.FullName;
            user.UserName = registerVM.UserName;
            user.Email = registerVM.Email;

            IdentityResult result = await _userManager.CreateAsync(user, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(registerVM);
            }

            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            string link = Url.Action(nameof(ConfirmEmail), "Account", new { userId = user.Id, token }, Request.Scheme, Request.Host.ToString());

            //// create email message
            //var email = new MimeMessage();
            //email.From.Add(MailboxAddress.Parse("ibrahimma@code.edu.az"));
            //email.To.Add(MailboxAddress.Parse(user.Email));
            //email.Subject = "Verify Email";

            string body = string.Empty;
            //using (StreamReader reader = new StreamReader("wwwroot/Verify.html"))
            //{
            //    body = reader.ReadToEnd();
            //}
            string path = "wwwroot/Verify.html";
            body = _fileService.ReadFile(path, body);

            body = body.Replace("{{link}}", link);
            body = body.Replace("{{Fullname}}", user.FullName);

            string subject = "Verify Email";
            _emailService.Send(user.Email, subject, body);

            //email.Body = new TextPart(TextFormat.Html) { Text = body };

            //// send email
            //using var smtp = new SmtpClient();
            //smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            //smtp.Authenticate("ibrahimma@code.edu.az", "fcypfykldyfzhplq");
            //smtp.Send(email);
            //smtp.Disconnect(true);


            return RedirectToAction(nameof(VerifyEmail));
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null) return NotFound();

            AppUser user = await _userManager.FindByIdAsync(userId);

            if (user == null) return NotFound();

            await _userManager.ConfirmEmailAsync(user, token);

            await _signInManager.SignInAsync(user, false);

            return RedirectToAction(nameof(Login));
        }

        public IActionResult VerifyEmail()
        {
            return View();
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM forgotPassword)
        {
            if (!ModelState.IsValid) return NotFound();

            AppUser exsistUser = await _userManager.FindByEmailAsync(forgotPassword.Email);

            if (exsistUser == null)
            {
                ModelState.AddModelError("Email", "Email isn't found");
                return View();
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(exsistUser);

            string link = Url.Action(nameof(ResetPassword), "Account", new { userId = exsistUser.Id, token },
                Request.Scheme, Request.Host.ToString());


            //// create email message
            //var email = new MimeMessage();
            //email.From.Add(MailboxAddress.Parse("ibrahimma@code.edu.az"));
            //email.To.Add(MailboxAddress.Parse(exsistUser.Email));
            //email.Subject = "Verify reset password Email";

            string body = string.Empty;
            //using (StreamReader reader = new StreamReader("wwwroot/Verify.html"))
            //{
            //    body = reader.ReadToEnd();
            //}
            string path = "wwwroot/Verify.html";
            body = _fileService.ReadFile(path, body);

            body = body.Replace("{{link}}", link);
            body = body.Replace("{{Fullname}}", exsistUser.FullName);

            string subject = "Verify Email";
            _emailService.Send(exsistUser.Email, subject, body);

            //email.Body = new TextPart(TextFormat.Html) { Text = body };

            // send email
            //using var smtp = new SmtpClient();
            //smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            //smtp.Authenticate("ibrahimma@code.edu.az", "fcypfykldyfzhplq");
            //smtp.Send(email);
            //smtp.Disconnect(true);

            return RedirectToAction(nameof(VerifyEmail));

        }
        public async Task<IActionResult> ResetPassword(string userId, string token)
        {
            ResetPasswordVM resetPassword = new ResetPasswordVM()
            {
                UserId = userId,
                Token = token
            };
            return View(resetPassword);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPassword)
        {
            if (!ModelState.IsValid) return View();



            AppUser exsistUser = await _userManager.FindByIdAsync(resetPassword.UserId);


            bool chekPassword = await _userManager.VerifyUserTokenAsync(exsistUser, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", resetPassword.Token);

            if (!chekPassword) return Content("Error");


            if (exsistUser == null) return NotFound();

            if (await _userManager.CheckPasswordAsync(exsistUser, resetPassword.Password))
            {
                ModelState.AddModelError("", "This password is your last password");
                return View(resetPassword);
            }



            await _userManager.ResetPasswordAsync(exsistUser, resetPassword.Token, resetPassword.Password);

            await _userManager.UpdateSecurityStampAsync(exsistUser);

            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM, string? ReturnUrl)
        {
            if (!ModelState.IsValid) return View(loginVM);

            AppUser user = await _userManager.FindByEmailAsync(loginVM.UserNameOrEmail);

            if (user == null)
            {
                user = await _userManager.FindByNameAsync(loginVM.UserNameOrEmail);
                if (user == null)
                {
                    ModelState.AddModelError("", "UserNameorEmail or Password is invalid!");
                    return View(loginVM);
                }
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, true);

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Hesabınız bloklanıb");
                return View(loginVM);
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "UserNameorEmail or Password is invalid!");
                return View(loginVM);
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "This account hasn't been verified yet!");
                return View(loginVM);
            }


            await _signInManager.SignInAsync(user, true);


            if (ReturnUrl != null)
            {
                return Redirect(ReturnUrl);
            }

            //if (await _userManager.IsInRoleAsync(user, RoleEnums.Admin.ToString()))
            //{
            //    return RedirectToAction("Index", "Dashboard", new { Area = "AdminArea" });
            //}



            return RedirectToAction("index", "Home");
        }

        //public async Task<IActionResult> CreateRoles()
        //{
        //    foreach (var item in Enum.GetValues(typeof(RoleEnums)))
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole { Name = item.ToString() });
        //    }
        //    return Content("Roles added");
        //}
    }
}


