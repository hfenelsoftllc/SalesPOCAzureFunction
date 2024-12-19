using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SalesWebApp.Models;
using System.Text;

namespace SalesWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger;
        static readonly HttpClient client = new HttpClient();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Index( SalesRequest salesRequest)
        {
            salesRequest.Id = Guid.NewGuid().ToString();

            using (var content = new StringContent(JsonConvert.SerializeObject(salesRequest), Encoding.UTF8, "application/json")) 
            {
                HttpResponseMessage response = await client.PostAsync("http://localhost:7219/api/OnSalesUploadWriteToQueue", content);
                string returnVlue = response.Content.ReadAsStringAsync().Result;

            }
            return RedirectToAction(nameof(Index));
        }
    }
}
