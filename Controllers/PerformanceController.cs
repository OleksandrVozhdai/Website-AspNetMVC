using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LAB2_OOKP.Models;

namespace LAB2_OOKP.Controllers
{
    public class PerformanceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PerformanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult ComparePerformance()
        {
            var stopwatch = new Stopwatch();

            var query = _context.UsersTwoTest;

            stopwatch.Start();
            Parallel.ForEach(query.ToList(), user =>
            {
                var result = user.Email.ToUpper(); 
            });
            stopwatch.Stop();
            ViewData["ParallelForTime"] = stopwatch.ElapsedMilliseconds;

            stopwatch.Restart();
            var threads = new Thread[4]; 
            for (int i = 0; i < 4; i++)
            {
                int threadIndex = i;
                threads[i] = new Thread(() =>
                {
                    var dataSubset = query.Skip(threadIndex * query.Count() / 4).Take(query.Count() / 4).ToList();
                    foreach (var user in dataSubset)
                    {
                        var result = user.Email.ToUpper();
                    }
                });
                threads[i].Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }
            stopwatch.Stop();
            ViewData["ThreadingTime"] = stopwatch.ElapsedMilliseconds;

            stopwatch.Restart();
            var tasks = new Task[4];
            for (int i = 0; i < 4; i++)
            {
                int taskIndex = i;
                tasks[i] = Task.Run(() =>
                {
                    var dataSubset = query.Skip(taskIndex * query.Count() / 4).Take(query.Count() / 4).ToList();
                    foreach (var user in dataSubset)
                    {
                        var result = user.Email.ToUpper();
                    }
                });
            }

            Task.WhenAll(tasks).Wait();
            stopwatch.Stop();
            ViewData["TPLTime"] = stopwatch.ElapsedMilliseconds;

            return View();
        }
    }
}
