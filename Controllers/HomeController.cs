using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MVC_Tutorial.Models;

namespace MVC_Tutorial.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly MVCTutorialDbContext _context;

    public HomeController(ILogger<HomeController> logger, MVCTutorialDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Expenses()
    {
        string connectionString = "Data Source=(localdb)\\mvclocaldb;Initial Catalog=ExpensesDB";

        string sqlQuery = "SELECT * FROM dbo.Expenses";

        SqlConnection con = new SqlConnection(connectionString);

        con.Open();
        SqlCommand sc = new SqlCommand(sqlQuery, con);

        List<Expense> allExpenses = new List<Expense>();

        using (SqlDataReader reader = sc.ExecuteReader())
        {
            while (reader.Read())
            {
                allExpenses.Add(new Expense {
                    Id = int.Parse(reader["ExpenseID"].ToString()),
                    Value = decimal.Parse(reader["ExpenseValue"].ToString()),
                    Description = reader["ExpenseDescription"].ToString()
                });
            }
        }

        con.Close();

        decimal totalExpenses = 0;
        foreach(Expense e in allExpenses)
        {
            totalExpenses += e.Value;
        }

        ViewBag.Expenses = totalExpenses;

        return View(allExpenses);
    }

    public IActionResult CreateEditExpense(int? id)
    {
        if(id != null)
        {
            // editing -> load an expense by id
            var expenseInDb = _context.Expenses.SingleOrDefault(expense => expense.Id == id);
            return View(expenseInDb);
        }

        return View();
    }

    public IActionResult DeleteExpense(int id)
    {
        var expenseInDb = _context.Expenses.SingleOrDefault(expense => expense.Id == id);
        _context.Expenses.Remove(expenseInDb);
        _context.SaveChanges();

        return RedirectToAction("Expenses");
    }

    public IActionResult CreateEditExpenseForm(Expense model)
    {
        try
        {
            string connectionString = "Data Source=(localdb)\\mvclocaldb;Initial Catalog=ExpensesDB";

            string sqlQuery = "INSERT INTO dbo.Expenses (ExpenseValue, ExpenseDescription) VALUES (" + "'" + model.Value + "','" + model.Description + "'" + ")";

            SqlConnection con = new SqlConnection(connectionString);

            con.Open();
            SqlCommand sc = new SqlCommand(sqlQuery, con);
            sc.ExecuteNonQuery();
            con.Close();

            return RedirectToAction("Expenses");
        }
        catch
        {
            return RedirectToAction("Error");
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
