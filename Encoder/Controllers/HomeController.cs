using Encoder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Encoder.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private HtmlEncoder _htmlEncoder;//html içeriklerin encoder edilmesi için kullanılır.
        private UrlEncoder _urlEncoder;
        private JavaScriptEncoder _JavaScriptEncoder;

        public HomeController(ILogger<HomeController> logger, JavaScriptEncoder javaScriptEncoder, UrlEncoder urlEncoder, HtmlEncoder htmlEncoder)
        {
            _logger = logger;
            _JavaScriptEncoder = javaScriptEncoder;
            _urlEncoder = urlEncoder;
            _htmlEncoder = htmlEncoder;
        }

        public IActionResult Method()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Method(string name)
        {
            ViewBag.Encoder = _htmlEncoder.Encode(name);
            //hangisi encoder edilecekse o seçilir. html/js/url
            return RedirectToAction("Method");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
