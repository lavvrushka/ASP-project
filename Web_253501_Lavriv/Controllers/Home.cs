using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253501_LAVRIV.Models;

namespace Web_253501_Lavriv.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["TitleText"] = "Лабораторная работа №2";

            var listItems = new List<ListDemo>
        {
            new ListDemo { Id = 1, Name = "Элемент 1" },
            new ListDemo { Id = 2, Name = "Элемент 2" },
            new ListDemo { Id = 3, Name = "Элемент 3" },
        };

            ViewBag.ListItems = new SelectList(listItems, "Id", "Name");

            return View();
        }
    }

}
 