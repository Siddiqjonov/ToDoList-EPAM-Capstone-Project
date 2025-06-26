using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Models.Dtos.AppUserDtos;
using TodoListApp.WebApi.Models.Dtos.AuthDtos;
using TodoListApp.WebApi.Models.FluentValidations.UserFluentValidators;
#pragma warning disable S1075 // URIs should not be hardcoded
#pragma warning disable SYSLIB1045 // Convert to 'GeneratedRegexAttribute'.

namespace TodoListApp.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient httpClient;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            this.httpClient = httpClientFactory.CreateClient();
            this.httpClient.BaseAddress = new Uri("https://localhost:7125/");
        }

        public IActionResult SignIn()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(UserLogInDto model)
        {
            var logInValidator = new UserLogInDtoValidator();
            var result = logInValidator.Validate(model);

            if (!result.IsValid)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
                this.ModelState.AddModelError("", $"Invalid login attempt: {errors}");
                return this.View(model);
            }

            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await this.httpClient.PostAsync("api/auth/signIn", content);
            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                this.HttpContext.Session.SetString("Token", token);
                return this.RedirectToAction("Index", "TodoList");
            }
            else
            {
                string errorMessage = "Invalid login attempt.";
                this.ModelState.AddModelError("", errorMessage);
                return this.View(model);
            }
        }

        public IActionResult SignUp()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserCreateDto model)
        {
            var userValidator = new UserCreateDtoValidator();
            var result = userValidator.Validate(model);

            if (!result.IsValid)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
                this.ModelState.AddModelError("", $"Sign up failed: {errors}");
                return this.View(model);
            }

            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await this.httpClient.PostAsync("api/auth/signUp", content);
            if (response.IsSuccessStatusCode)
            {
                this.TempData["SuccessMessage"] = "Account created successfully. Please sign in.";
                return this.RedirectToAction("SignIn");
            }
            else
            {
                string errorMessage = "Invalid sing up attempt.";
                this.ModelState.AddModelError("", errorMessage + "\n" + response.Content.ToString());
                return this.View(model);
            }

        }

        public async Task<IActionResult> LogOut()
        {
            var token = this.HttpContext.Session.GetString("Token");
            if (!string.IsNullOrEmpty(token))
            {
                _ = await this.httpClient.DeleteAsync($"api/auth/logOut?token={token}");
                this.HttpContext.Session.Remove("Token");
            }
            return this.RedirectToAction("SignIn");
        }

        public IActionResult ForgotPassword()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (!Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@gmail\.com$"))
            {
                this.ModelState.AddModelError("", "Gmail Only");
            }

            if (string.IsNullOrEmpty(email))
            {
                this.ModelState.AddModelError("", "Please enter a valid email address.");
                return this.View();
            }

            var response = await this.httpClient.PostAsync($"api/auth/request-password-reset?email={Uri.EscapeDataString(email)}", null);
            if (response.IsSuccessStatusCode)
            {
                this.TempData["SuccessMessage"] = "If you are the user of the app, a reset code has been sent.";
                return this.RedirectToAction("VerifyPasswordReset", new { email });
            }
            else
            {
                this.ModelState.AddModelError("", "Failed to send reset email. Please try again.");
                return this.View();
            }
        }

        public IActionResult VerifyPasswordReset(string email)
        {
            return this.View(new VerifyResetRequest()
            {
                Email = email,
            });
        }

        [HttpPost]
        public async Task<IActionResult> VerifyPasswordReset(VerifyResetRequest model)
        {
            if (!Regex.IsMatch(model.Email, @"^[a-zA-Z0-9._%+-]+@gmail\.com$"))
            {
                this.ModelState.AddModelError("", "Gmail Only");
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await this.httpClient.PostAsync("api/auth/verify-password-reset", content);
            if (response.IsSuccessStatusCode)
            {
                this.TempData["SuccessMessage"] = "Password reset successfully. Please sign in.";
                return this.RedirectToAction("SignIn");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                this.ModelState.AddModelError("", $"Password reset failed: {error}");
                return this.View(model);
            }
        }
    }
}
