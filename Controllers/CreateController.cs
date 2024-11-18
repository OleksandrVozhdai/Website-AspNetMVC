using LAB2_OOKP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

public class CreateController : Controller
{
    private readonly ApplicationDbContext _context;
    public CreateController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [HttpPost]
    public IActionResult Index(string Name, string Surname, string Email, string Phone_Number, string Password, string ConfirmPassword, int Age)
    {
        // Перевірки
        if (_context.UsersTable.Any(u => u.Email == Email))
        {
            ModelState.AddModelError("", "This email is already registered.");
            return View();
        }

        if (Age < 18)
        {
            ModelState.AddModelError("", "Registration is allowed only for users 18 years old and above.");
            return View();
        }

        if (Password != ConfirmPassword)
        {
            ModelState.AddModelError("", "Passwords do not match.");
            return View();
        }

        char[] phumbers = Phone_Number.ToCharArray();
        if (phumbers.Length < 11 || phumbers[0] != '+' || !phumbers.Skip(1).All(char.IsDigit))
        {
            ModelState.AddModelError("", "Enter correct phone number");
            return View();
        }

        var newUser = new User
        {
            Name = Name,
            Surname = Surname,
            Email = Email,
            Age = Age,
            PhoneNumber = Phone_Number,
            Password = Password
        };

        _context.UsersTable.Add(newUser);
        _context.SaveChanges();

        // Генерація звіту
        GenerateRegistrationReport(Name);

        return RedirectToAction("Index", "Home");
    }

    private void GenerateRegistrationReport(string name)
    {
        string reportPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "reports", "registration_report.txt");
        Directory.CreateDirectory(Path.GetDirectoryName(reportPath));

        using (StreamWriter writer = new StreamWriter(reportPath, true))
        {
            writer.WriteLine($"Користувач {name} зареєструвався {DateTime.Now.ToString("dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture)}");
        }
    }


}
