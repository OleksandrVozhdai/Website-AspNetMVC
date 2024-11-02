using LAB2_OOKP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

public class UserAccountController : Controller
{
    private readonly ApplicationDbContext _context;

    public UserAccountController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        ViewBag.UserName = HttpContext.Session.GetString("UserName");

        if (string.IsNullOrEmpty(ViewBag.UserName))
        {
            return RedirectToAction("Login", "Authorisation");
        }

        return View(new ChangePasswordViewModel());
    }

    [HttpPost]
    public IActionResult ChangePassword(ChangePasswordViewModel model)
    {
        ViewBag.UserName = HttpContext.Session.GetString("UserName");

        if (string.IsNullOrEmpty(ViewBag.UserName))
        {
            ModelState.AddModelError("", "You are not logged in.");
            return View(model);
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        string userName = ViewBag.UserName as string;
        System.Diagnostics.Debug.WriteLine($"UserName in session: {ViewBag.UserName}");

 
        var currentUser = _context.UsersTable.FirstOrDefault(u => u.Name == userName);

        if (currentUser == null)
        {
            ModelState.AddModelError("", "User not found.");
            return View(model);
        }

        if (currentUser.Password != model.OldPassword)
        {
            ModelState.AddModelError("", "The current password is incorrect.");
            return View(model);
        }

        currentUser.Password = model.NewPassword;
        _context.SaveChanges();

        TempData["Message"] = "Password successfully changed!";
        return RedirectToAction("Index", "Home");
    }
}
