using LAB2_OOKP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiteMVC.Models;
using System.Diagnostics;

namespace LAB2_OOKP.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserEmail = HttpContext.Session.GetString("UserEmail");
            return View();
        }

        public ActionResult About()

        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        public IActionResult PopulateUsersTwoTest()
        {
            var random = new Random();
            const int totalRecords = 100000; 
            const int batchSize = 1000;

            for (int batch = 0; batch < totalRecords / batchSize; batch++)
            {
                var usersList = new List<UsersTwoTest>();

                for (int i = 0; i < batchSize; i++)
                {
                    var index = batch * batchSize + i;
                    usersList.Add(new UsersTwoTest
                    {
                        Name = $"Name{index}",
                        Surname = $"Surname{index}",
                        Email = $"user{index}@example.com",
                        PhoneNumber = $"+380{random.Next(100000000, 999999999)}",
                        Password = random.Next(1000, 9999).ToString(),
                        Age = random.Next(18, 60),
                        RoleId = Guid.NewGuid()
                    });
                }

                _context.UsersTwoTest.AddRange(usersList);
                _context.SaveChanges(); 
            }

            return Content($"The UsersTwoTest table has been successfully populated!");
        }


        public IActionResult CompareDeleteAndTruncate()
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            _context.Database.ExecuteSqlRaw("DELETE FROM dbo.UsersTwoTest");
            stopwatch.Stop();
            var deleteTime = stopwatch.ElapsedMilliseconds;

            stopwatch.Reset();

            stopwatch.Start();
            _context.Database.ExecuteSqlRaw("TRUNCATE TABLE dbo.UsersTwoTest");
            stopwatch.Stop();
            var truncateTime = stopwatch.ElapsedMilliseconds;

            return Content($"DELETE execution time: {deleteTime} ms. TRUNCATE execution time: {truncateTime} ms.");
        }

    }
}