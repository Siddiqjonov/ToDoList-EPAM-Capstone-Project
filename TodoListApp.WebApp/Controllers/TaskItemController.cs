#pragma warning disable S1075 // URIs should not be hardcoded
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Models.Dtos.TaskItemDtos;
using TodoListApp.WebApi.Models.FluentValidations.TaskItemFluentValidators;

namespace TodoListApp.WebApp.Controllers
{
    public class TaskItemController : Controller
    {
        private readonly HttpClient httpClient;

        public TaskItemController(IHttpClientFactory httpClientFactory)
        {
            this.httpClient = httpClientFactory.CreateClient();
            this.httpClient.BaseAddress = new Uri("https://localhost:7125/");
        }

        public IActionResult SelectList(long todoListId)
        {
            if (todoListId <= 0)
            {
                this.TempData["ErrorMessage"] = "Invalid Todo List ID.";
                return this.RedirectToAction("Index", "ToDoList");
            }

            this.HttpContext.Session.SetString("SelectedListId", todoListId.ToString());
            return this.RedirectToAction("Index");
        }

        public IActionResult SelectListToAddTask(long todoListId)
        {
            if (todoListId <= 0)
            {
                this.TempData["ErrorMessage"] = "Invalid Todo List ID.";
                return this.RedirectToAction("Index", "ToDoList");
            }

            this.HttpContext.Session.SetString("SelectedListId", todoListId.ToString());
            return this.RedirectToAction("Create", "TaskItem", new { toDoListId = todoListId });
        }

        public async Task<IActionResult> Index(long? tagId)
        {
            var token = this.HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return this.RedirectToAction("SignIn", "Account");
            }

            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            List<TaskItemGetDto> tasks = [];
            string apiUrl;

            if (tagId.HasValue)
            {
                apiUrl = $"api/taskItem/getTasksByTagId/{tagId}";
                this.ViewBag.TagId = tagId;
            }
            else
            {
                var listIdString = this.HttpContext.Session.GetString("SelectedListId");
                if (string.IsNullOrEmpty(listIdString))
                {
                    this.TempData["ErrorMessage"] = "ToDo list not selected.";
                    return this.RedirectToAction("Index", "ToDoList");
                }

                var todoListId = Convert.ToInt64(listIdString);
                this.ViewBag.TodoListId = todoListId;
                apiUrl = $"api/taskItem/getTasksByToDoListId/{todoListId}";
            }

            var response = await this.httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                tasks = JsonSerializer.Deserialize<List<TaskItemGetDto>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? [];
            }
            else
            {
                this.TempData["ErrorMessage"] = "Failed to load tasks.";
            }

            return this.View(tasks);
        }

        public IActionResult Create(long toDoListId)
        {
            var token = this.HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return this.RedirectToAction("SignIn", "Account");
            }

            if (toDoListId <= 0)
            {
                this.TempData["ErrorMessage"] = "Invalid Todo List ID.";
                return this.RedirectToAction("Index", "TodoList");
            }

            this.ViewBag.TodoListId = toDoListId;
            return this.View(new TaskItemCreateDto { TodoListId = toDoListId });
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskItemCreateDto model)
        {
            var token = this.HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return this.RedirectToAction("SignIn", "Account");
            }

            if (model.TodoListId <= 0)
            {
                this.ModelState.AddModelError("", "Invalid Todo List ID.");
                this.ViewBag.TodoListId = model.TodoListId;
                return this.View(model);
            }

            var validator = new TaskItemCreateDtoValidator();
            var result = validator.Validate(model);
            if (!result.IsValid)
            {
                this.ModelState.AddModelError("", string.Join(", ", result.Errors.Select(e => e.ErrorMessage)));
                this.ViewBag.TodoListId = model.TodoListId;
                return this.View(model);
            }

            this.httpClient.DefaultRequestHeaders.Authorization = new("Bearer", token);
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await this.httpClient.PostAsync("api/taskItem/addTask", content);

            if (response.IsSuccessStatusCode)
            {
                this.TempData["SuccessMessage"] = "Task created successfully.";
                return this.RedirectToAction("Index", new { toDoListId = model.TodoListId });
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            var errorMessage = JsonSerializer.Deserialize<Dictionary<string, string>>(errorContent)
                ?.TryGetValue("message", out var message) ?? false ? message : "Failed to create task.";

            this.ModelState.AddModelError("", errorMessage);
            this.ViewBag.TodoListId = model.TodoListId;
            return this.View(model);
        }

        public IActionResult Edit(long taskId, long toDoListId)
        {
            var token = this.HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return this.RedirectToAction("SignIn", "Account");
            }

            if (toDoListId <= 0)
            {
                this.TempData["ErrorMessage"] = "Invalid Todo List ID.";
                return this.RedirectToAction("Index", "TodoList");
            }

            var model = new TaskItemUpdateDto
            {
                TaskItemId = taskId,
                TodoListId = toDoListId,
            };

            this.ViewBag.TodoListId = toDoListId;
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TaskItemUpdateDto model)
        {
            var validator = new TaskItemUpdateDtoValidator();
            var result = validator.Validate(model);
            if (!result.IsValid)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
                this.ModelState.AddModelError("", errors);
                this.ViewBag.TodoListId = model.TodoListId;
                return this.View(model);
            }

            var token = this.HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return this.RedirectToAction("SignIn", "Account");
            }

            if (!this.ModelState.IsValid)
            {
                this.ViewBag.TodoListId = model.TodoListId;
                return this.View(model);
            }

            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await this.httpClient.PutAsync("api/taskItem/updateTask", content);

            if (response.IsSuccessStatusCode)
            {
                this.TempData["SuccessMessage"] = "Task updated successfully.";
                return this.RedirectToAction("Index", new { toDoListId = model.TodoListId });
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                this.ModelState.AddModelError("", $"Failed to update task: {error}");
                this.ViewBag.TodoListId = model.TodoListId;
                return this.View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(long taskId)
        {
            var token = this.HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return this.RedirectToAction("SignIn", "Account");
            }

            this.httpClient.DefaultRequestHeaders.Authorization = new("Bearer", token);
            var response = await this.httpClient.DeleteAsync($"api/taskItem/deleteTask/{taskId}");
            if (response.IsSuccessStatusCode)
            {
                this.TempData["SuccessMessage"] = "Task deleted successfully.";
            }
            else
            {
                this.TempData["ErrorMessage"] = "Failed to delete task.";
            }
            return this.RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(long taskId, long toDoListId)
        {
            var token = this.HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return this.RedirectToAction("SignIn", "Account");
            }

            this.httpClient.DefaultRequestHeaders.Authorization = new("Bearer", token);
            var response = await this.httpClient.GetAsync($"api/taskItem/getTaskDetails/{taskId}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var task = JsonSerializer.Deserialize<TaskItemGetDto>(json);
                if (task == null)
                {
                    return this.NotFound();
                }
                this.ViewBag.TodoListId = toDoListId;
                return this.View(task);
            }
            else
            {
                return this.NotFound();
            }
        }

        public async Task<IActionResult> UpdateStatus(long taskId, bool isCompleted, long toDoListId)
        {
            var token = this.HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return this.RedirectToAction("SignIn", "Account");
            }

            this.httpClient.DefaultRequestHeaders.Authorization = new("Bearer", token);
            var response = await this.httpClient.PatchAsync($"api/taskItem/updateTaskStatus/{taskId}?isCompleted={isCompleted}", null);
            if (response.IsSuccessStatusCode)
            {
                this.TempData["SuccessMessage"] = "Task status updated successfully.";
            }
            else
            {
                this.TempData["ErrorMessage"] = "Failed to update task status.";
            }
            return this.RedirectToAction("Index", new { toDoListId });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTaskStatus(long taskId, bool isCompleted)
        {
            var token = this.HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                this.TempData["ErrorMessage"] = "You must be logged in.";
                return this.RedirectToAction("Login", "Auth");
            }

            var request = new HttpRequestMessage(HttpMethod.Patch, $"api/taskItem/updateTaskStatus/{taskId}?isCompleted={isCompleted}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await this.httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                this.TempData["ErrorMessage"] = "Failed to update task status.";
            }

            return this.RedirectToAction("Index");
        }

        public async Task<IActionResult> Search(string title, DateTime? createdDate, DateTime? dueDate, long toDoListId)
        {
            var token = this.HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return this.RedirectToAction("SignIn", "Account");
            }

            this.httpClient.DefaultRequestHeaders.Authorization = new("Bearer", token);
            var query = $"api/taskItem/searchTasks?";
            if (!string.IsNullOrEmpty(title))
            {
                query += $"title={Uri.EscapeDataString(title)}&";
            }

            if (createdDate.HasValue)
            {
                query += $"createdDate={createdDate.Value:yyyy-MM-dd}&";
            }

            if (dueDate.HasValue)
            {
                query += $"dueDate={dueDate.Value:yyyy-MM-dd}";
            }

            var response = await this.httpClient.GetAsync(query);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var tasks = JsonSerializer.Deserialize<List<TaskItemGetDto>>(json, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                });
                this.ViewBag.TodoListId = toDoListId;
                this.ViewBag.SearchTitle = title;
                this.ViewBag.SearchCreatedDate = createdDate;
                this.ViewBag.SearchDueDate = dueDate;
                return this.View("Index", tasks ?? []);
            }
            else
            {
                this.TempData["ErrorMessage"] = "Failed to search tasks.";
                return this.View("Index", new List<TaskItemGetDto>());
            }
        }
    }
}
