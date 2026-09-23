using Microsoft.AspNetCore.Mvc;
using StudentManagment.Web.Models;
using System.Net.Http.Json;

namespace StudentManagment.Web.Controllers
{
    public class ProgrammeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProgrammeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("StudentAPI");

            var programmes = await client.GetFromJsonAsync<List<Programme>>(
                "api/Programmes");

            var departments = await client.GetFromJsonAsync<List<Department>>(
                "api/Departments");

            ViewBag.Departments =
                departments ?? new List<Department>();

            return View(
                programmes ?? new List<Programme>());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Programme programme)
        {
            var client = _httpClientFactory.CreateClient("StudentAPI");

            programme.CreatedBy = "Admin";

            var response = await client.PostAsJsonAsync(
                "api/Programmes",
                programme);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] =
                    "Programme created successfully.";
            }
            else
            {
                var message =
                    await response.Content.ReadAsStringAsync();

                TempData["Error"] = message.Trim('"');
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Update(Programme programme)
        {
            var client = _httpClientFactory.CreateClient("StudentAPI");
            programme.CreatedBy = "Admin";

            var response = await client.PutAsJsonAsync(
                "api/Programmes/" + programme.ProgrammeId,
                programme);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] =
                    "Programme updated successfully.";
            }
            else
            {
                var message =
                    await response.Content.ReadAsStringAsync();

                TempData["Error"] = message.Trim('"');
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("StudentAPI");

            var response = await client.DeleteAsync(
                "api/Programmes/" + id);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] =
                    "Programme deleted successfully.";
            }
            else
            {
                TempData["Error"] =
                    "Unable to delete programme.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}