using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Core.Errors;
using TodoListApp.WebApi.Models.Dtos.ToDoListDtos;
using TodoListApp.WebApi.Models.FluentValidations.ToDoListFluentValidators;
#pragma warning disable S1075 // URIs should not be hardcoded

namespace TodoListApp.WebApp.Controllers
{
    public class TodoListController : Controller
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<TodoListController> logger;

        public TodoListController(IHttpClientFactory httpClientFactory, ILogger<TodoListController> logger)
        {
            // Initialize HttpClient with base address
            this.httpClient = httpClientFactory.CreateClient();
            this.httpClient.BaseAddress = new Uri("https://localhost:7125/");
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var token = this.HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return this.RedirectToAction("SignIn", "Account");
            }

            token = token.Replace("\r", "").Replace("\n", "");
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await this.httpClient.GetAsync("api/toDoList/getAll");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var todoLists = JsonSerializer.Deserialize<List<ToDoListGetDto>>(json, options);
                return this.View(todoLists ?? []);
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                this.logger.LogError("Failed to load todo lists: {Error}", error);
                this.TempData["ErrorMessage"] = "Failed to load todo lists.";
                return this.View(new List<ToDoListGetDto>());
            }
        }

        public IActionResult Create()
        {
            return this.View(new TodoListCreateDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(TodoListCreateDto model)
        {
            var token = this.HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return this.RedirectToAction("SignIn", "Account");
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            token = token.Replace("\r", "").Replace("\n", "");
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await this.httpClient.PostAsync("api/toDoList/add", content);
            if (response.IsSuccessStatusCode)
            {
                this.logger.LogInformation("Todo list created successfully");
                this.TempData["SuccessMessage"] = "Todo list created successfully.";
                return this.RedirectToAction("Index");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                this.logger.LogError("Failed to create todo list: {Error}", error);
                this.ModelState.AddModelError("", "Failed to create todo list.");
                return this.View(model);
            }
        }

        public IActionResult Edit(long id)
        {
            var token = this.HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return this.RedirectToAction("SignIn", "Account");
            }

            if (id <= 0)
            {
                this.logger.LogWarning("Invalid TodoListId: {Id}", id);
                return this.NotFound();
            }

            var model = new TodoListUpdateDto()
            {
                TodoListId = id,
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(TodoListUpdateDto model)
        {
            var token = this.HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return this.RedirectToAction("SignIn", "Account");
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Extract the user ID (adjust claim type if needed: "Id", "sub", "userId")
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "Id")?.Value
                ?? throw new ForbiddenException("Access forbidden");

            model.UserId = long.Parse(userId);

            var validator = new TodoListUpdateDtoValidator();
            var result = validator.Validate(model);
            if (!result.IsValid)
            {
                this.ModelState.AddModelError("", string.Join(", ", result.Errors.Select(e => e.ErrorMessage)));
                this.ViewBag.TodoListId = model.TodoListId;
                return this.View("Edit", model);
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await this.httpClient.PutAsync("api/toDoList/update", content);
            if (response.IsSuccessStatusCode)
            {
                this.logger.LogInformation("Todo list {Id} updated successfully", model.TodoListId);
                this.TempData["SuccessMessage"] = "Todo list updated successfully.";
                return this.RedirectToAction("Index");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                this.logger.LogError("Failed to update todo list {Id}: {Error}", model.TodoListId, error);
                this.ModelState.AddModelError("", "Failed to update todo list.");
                return this.View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(long id)
        {
            var token = this.HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return this.RedirectToAction("SignIn", "Account");
            }

            if (id <= 0)
            {
                this.logger.LogWarning("Invalid TodoListId for deletion: {Id}", id);
                this.TempData["ErrorMessage"] = "Invalid Todo List ID.";
                return this.RedirectToAction("Index");
            }

            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await this.httpClient.DeleteAsync($"api/toDoList/delete/{id}");
            if (response.IsSuccessStatusCode)
            {
                this.logger.LogInformation("Todo list {Id} deleted successfully", id);
                this.TempData["SuccessMessage"] = "Todo list deleted successfully.";
            }
            else
            {
                // Log error and display user-friendly message
                var error = await response.Content.ReadAsStringAsync();
                this.logger.LogError("Failed to delete todo list {Id}: {Error}", id, error);
                this.TempData["ErrorMessage"] = "Failed to delete todo list.";
            }
            return this.RedirectToAction("Index");
        }
    }
}
