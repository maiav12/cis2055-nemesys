using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Nemesys.Controllers
{

    public class HomeController : Controller
    {
        [HttpGet]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)]
        public IActionResult Index()
        {
            //Load data from the Model
            ViewBag.Title = "Nemesys - Home";
            ViewBag.Message = "Hello there, this is the time: " + DateTime.Now;
            return View();
        }

        public class ModelBindingTest
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }


        public IActionResult About(ModelBindingTest test)
        {
            //Load data from the Model
            ViewBag.Title = "Nemesys - About";
            ViewBag.Message = "Hello there, this is the about page. The model binder found - " + test.Name + " - " + test.Id;
            return View();
        }
    }
}
