using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Models.Dtos.TagDtos;
#pragma warning disable CA1031 // Do not catch general exception types
#pragma warning disable S1075 // URIs should not be hardcoded

namespace TodoListApp.WebApp.Controllers
{
    public class TagController : Controller
    {
        private readonly HttpClient httpClient;

        public TagController(IHttpClientFactory httpClientFactory)
        {
            this.httpClient = httpClientFactory.CreateClient();
            this.httpClient.BaseAddress = new Uri("https://localhost:7125/");
        }

        public async Task<IActionResult> Index()
        {
            var token = this.HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return this.RedirectToAction("SignIn", "Account");
            }

            this.httpClient.DefaultRequestHeaders.Authorization = new("Bearer", token);
            var response = await this.httpClient.GetAsync("api/Tag/getAllTagsAsync");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var tags = JsonSerializer.Deserialize<List<TagGetDto>>(json, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                });
                return this.View(tags ?? []);
            }
            else
            {
                this.TempData["ErrorMessage"] = "Failed to load tags.";
                return this.View(new List<TagGetDto>());
            }
        }

        public IActionResult Create(long taskId, long toDoListId)
        {
            this.ViewBag.TaskId = taskId;
            this.ViewBag.TodoListId = toDoListId;
            return this.View(new TagCreateDto { Name = string.Empty });
        }

        [HttpPost]
        public async Task<IActionResult> Create(TagCreateDto model, long taskId, long toDoListId)
        {
            var token = this.HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return this.RedirectToAction("SignIn", "Account");
            }

            if (!this.ModelState.IsValid)
            {
                this.ViewBag.TaskId = taskId;
                this.ViewBag.TodoListId = toDoListId;
                return this.View(model);
            }

            this.httpClient.DefaultRequestHeaders.Authorization = new("Bearer", token);
            var response = await this.httpClient.PostAsync($"api/tag/createTag?tagName={Uri.EscapeDataString(model.Name)}&taskId={taskId}", null);
            if (response.IsSuccessStatusCode)
            {
                this.TempData["SuccessMessage"] = "Tag created successfully.";
                return this.RedirectToAction("Index", "TaskItem", new { toDoListId });
            }
            else
            {
                this.ModelState.AddModelError("", $"Tag already exists");
                this.ViewBag.TaskId = taskId;
                this.ViewBag.TodoListId = toDoListId;
                return this.View(model);
            }
        }

        public async Task<IActionResult> AddTagToTask(long taskId, long tagId, long toDoListId)
        {
            var token = this.HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return this.RedirectToAction("SignIn", "Account");
            }

            try
            {
                this.httpClient.DefaultRequestHeaders.Authorization = new("Bearer", token);
                var response = await this.httpClient.PostAsync($"api/Tag/addTagToTask?taskId={taskId}&tagId={tagId}", null);
                if (response.IsSuccessStatusCode)
                {
                    this.TempData["SuccessMessage"] = "Tag added to task successfully.";
                }
                else
                {
                    this.TempData["ErrorMessage"] = "Failed to add tag to task.";
                }
                return this.RedirectToAction("Index", "TaskItem", new { toDoListId });
            }
            catch (Exception ex)
            {
                this.TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return this.RedirectToAction("Index", "TaskItem", new { toDoListId });
            }
        }

        public async Task<IActionResult> RemoveTagFromTask(long taskId, long tagId, long toDoListId)
        {
            var token = this.HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return this.RedirectToAction("SignIn", "Account");
            }

            try
            {
                this.httpClient.DefaultRequestHeaders.Authorization = new("Bearer", token);
                var response = await this.httpClient.DeleteAsync($"api/Tag/removeTagFromTask?taskId={taskId}&tagId={tagId}");
                if (response.IsSuccessStatusCode)
                {
                    this.TempData["SuccessMessage"] = "Tag removed from task successfully.";
                }
                else
                {
                    this.TempData["ErrorMessage"] = "Failed to remove tag from task.";
                }
                return this.RedirectToAction("Index", "TaskItem", new { toDoListId });
            }
            catch (Exception ex)
            {
                this.TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return this.RedirectToAction("Index", "TaskItem", new { toDoListId });
            }
        }
    }
}
