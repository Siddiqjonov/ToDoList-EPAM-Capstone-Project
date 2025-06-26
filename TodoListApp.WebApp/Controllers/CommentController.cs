using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Models.Dtos.CommentDtos;
#pragma warning disable S1075 // URIs should not be hardcoded

namespace TodoListApp.WebApp.Controllers
{
    public class CommentController : Controller
    {
        private readonly HttpClient httpClient;

        public CommentController(IHttpClientFactory httpClientFactory)
        {
            this.httpClient = httpClientFactory.CreateClient();
            this.httpClient.BaseAddress = new Uri("https://localhost:7125/");
        }

        public async Task<IActionResult> Index(long taskId)
        {
            var token = this.HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return this.RedirectToAction("SignIn", "Account");
            }

            this.httpClient.DefaultRequestHeaders.Authorization = new("Bearer", token);
            var response = await this.httpClient.GetAsync($"api/comment/getTaskComments/{taskId}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var comments = JsonSerializer.Deserialize<List<CommentGetDto>>(json, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                });
                this.ViewBag.TaskId = taskId;
                return this.View(comments ?? []);
            }
            else
            {
                this.TempData["ErrorMessage"] = "Failed to load comments.";
                return this.View(new List<CommentGetDto>());
            }
        }

        public IActionResult Create(long taskId)
        {
            return this.View(new CommentCreateDto { TaskItemId = taskId });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CommentCreateDto model)
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

            this.httpClient.DefaultRequestHeaders.Authorization = new("Bearer", token);
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await this.httpClient.PostAsync("api/comment/addComment", content);
            if (response.IsSuccessStatusCode)
            {
                this.TempData["SuccessMessage"] = "Comment added successfully.";
                return this.RedirectToAction("Index", "Comment", new { taskId = model.TaskItemId });
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                this.ModelState.AddModelError("", $"Failed to create comment: {error}");

                return this.View(model);
            }
        }

        public IActionResult Edit(long commentId, string content, long taskId)
        {
            var token = this.HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return this.RedirectToAction("SignIn", "Account");
            }

            var model = new CommentUpdateDto { CommentId = commentId, Content = content, TaskItemId = taskId };
            this.ViewBag.TaskId = taskId;
            return this.View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(CommentUpdateDto model)
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

            this.httpClient.DefaultRequestHeaders.Authorization = new("Bearer", token);
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await this.httpClient.PutAsync("api/comment/updateComment", content);
            if (response.IsSuccessStatusCode)
            {
                this.TempData["SuccessMessage"] = "Comment updated successfully.";
                return this.RedirectToAction("Index", new { taskId = model.TaskItemId });
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                this.ModelState.AddModelError("", $"Failed to update comment: {error}");
                return this.View(model);
            }
        }

        public async Task<IActionResult> Delete(long commentId, long taskId, long toDoListId)
        {
            var token = this.HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return this.RedirectToAction("SignIn", "Account");
            }

            this.httpClient.DefaultRequestHeaders.Authorization = new("Bearer", token);
            var response = await this.httpClient.DeleteAsync($"api/comment/deleteComment/{commentId}");
            if (response.IsSuccessStatusCode)
            {
                this.TempData["SuccessMessage"] = "Comment deleted successfully.";
            }
            else
            {
                this.TempData["ErrorMessage"] = "Failed to delete comment.";
            }
            return this.RedirectToAction("Index", new { taskId, toDoListId });
        }
    }
}
