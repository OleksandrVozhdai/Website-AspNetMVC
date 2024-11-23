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

            var query = _context.UsersTwoTest.ToList();

            stopwatch.Start();
            Parallel.ForEach(query, user =>
            {
                var result = user.Email.ToUpper();
            });
            stopwatch.Stop();
            ViewData["ParallelForTime"] = stopwatch.ElapsedMilliseconds;


            stopwatch.Restart();

            int threadCount = 4;
            var threads = new Thread[threadCount];
            var partitionSize = query.Count / threadCount;

            var threadResults = new List<int>();

            for (int i = 0; i < threadCount; i++)
            {
                int threadIndex = i;
                threads[i] = new Thread(() =>
                {
       
                    var dataSubset = query.Skip(threadIndex * partitionSize).Take(partitionSize).ToList();
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
            ViewData["ThreadedTime"] = stopwatch.ElapsedMilliseconds;



            stopwatch.Restart();
            var tasks = new List<Task>();
            for (int i = 0; i < 4; i++)
            {
                int taskIndex = i; 
                tasks.Add(Task.Run(() =>
                {
                    var dataSubset = query.Skip(taskIndex * query.Count / 4).Take(query.Count / 4).ToList();
                    foreach (var user in dataSubset)
                    {
                        var result = user.Email.ToUpper();
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());
            stopwatch.Stop();
            ViewData["TPLTime"] = stopwatch.ElapsedMilliseconds;

            return View();
        }


    }
}
