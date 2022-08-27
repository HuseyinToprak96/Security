using Ganss.XSS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using XSS.Models;
using Xunit;

namespace XSS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Login(string returnUrl="www.google.com")//buraya return url yazar isek sayfa yönlendirmesinde gelen sayfaya göre haraket etmemizi sağlar.Mesela giriş yapmadan yapılamayacak bir işlemde bu şekilde yönlendirmede bulunur isek kullanıcı işlemine kaldığı yerden devam eder.
        {
            TempData["returnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        public IActionResult Login(string email,string password)
        {
           string returnUrl=TempData["returnUrl"].ToString();
            // if (true)//email ve password kontrolü yapılcak
            if (Url.IsLocalUrl(returnUrl))//eğer url bizim url imiz ise ona dön değilse indexe gitmesini sağlar.
            {
                return Redirect(returnUrl);
            }
            return Redirect("/");
        }

        public IActionResult CommentAdd()
        {
            HttpContext.Response.Cookies.Append("email", "htoprak@hotmail.com");
            HttpContext.Response.Cookies.Append("password", "1234");
            return View();
        }
        //[ValidateAntiForgeryToken]//core da formlar içinde default olarak token oluşur o token üzerinden karşılaştırma yapar.
        //farklı bir form üzerinden istek yapılmasını engellemek için kullanılır. Güvenlik için önemlidir. Özellikle bankacılıkta
       
        [IgnoreAntiforgeryToken]//bunu yazar isek proje seviyesinde belirlediğimiz ValidateAntiForgeryToken ı dikkate almamasını sağlarız.
        [HttpPost]
        public IActionResult CommentAdd(string name,string comment)
        {
            var sanitizer = new HtmlSanitizer();
            var html = comment;
            var sanitized = sanitizer.Sanitize(html, "https://www.example.com");//zararlı kodları temizler..
            ViewBag.Name = name;
            ViewBag.Comment = sanitized;
            return View();
        }

        public IActionResult CommentAdd2()
        {
            HttpContext.Response.Cookies.Append("email", "htoprak@hotmail.com");
            HttpContext.Response.Cookies.Append("password", "1234");
            if (System.IO.File.Exists("comments.txt"))
            {
                ViewBag.comments = System.IO.File.ReadAllLines("comments.txt");
            }
            return View();
        }
        [HttpPost]
        public IActionResult CommentAdd2(string name, string comment)
        {
            System.IO.File.AppendAllText("comments.txt", $"{name}---{comment}\n");
            return RedirectToAction("CommentAdd2");
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
