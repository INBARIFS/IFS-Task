using IFSTask.Models;
using IFSTask.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace IFSTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string EncodedValue = "Ymx1ZWIxdWVCbHVl";                //hard coding this value on the server so it cant be tempared with on the frontend
        private readonly string TfaValue = "blue1sF0rev3r";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            HttpContext.Session.SetString("loggedIn", "false");

            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            if (Encoding.UTF8.GetString(Convert.FromBase64String(EncodedValue)) == loginViewModel.Password)
            {
                HttpContext.Session.SetString("loggedIn", "true");

                return Json(StatusCode(200));
            }
            else
            {
                return Json(StatusCode(401));
            }
        }

        public IActionResult Tfa()
        {
            if (HttpContext.Session.GetString("loggedIn") == "false")
            {
                return View("Index");
            }
            return View();
        }

        [HttpPost]
        public IActionResult ValidateTfa(LoginViewModel loginViewModel)
        {
            if (HttpContext.Session.GetString("loggedIn") == "false")
            {
                return View("Index");
            }
            if (TfaValue == loginViewModel.Password)
            {
                return Json(StatusCode(200));
            }
            else
            {
                return Json(StatusCode(401));
            }
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
