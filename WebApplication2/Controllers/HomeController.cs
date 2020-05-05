using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    class Person
    {
        public string Name { get; set; }
        public List<Pet> Pets { get; set; } = new List<Pet>();
    }
    class Pet
    {
        public string PetName { get; set; }
        public Person Master { get; set; }
    }
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {


            _logger.LogInformation("from method: {@Method} - AccountId: {@AccountId}", nameof(Index), "123");

            Person p1 = new Person { Name = "person1" };

            Pet pet1 = new Pet { PetName = "Pet1", Master = p1 };
            Pet pet2 = new Pet { PetName = "Pet2", Master = p1 };

            p1.Pets.Add(pet1);
            p1.Pets.Add(pet2);

            Person p2 = new Person { Name = "person2" };


            _logger.LogInformation("{@Pet1}", pet1);

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
