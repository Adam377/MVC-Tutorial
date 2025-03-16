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
        decimal totalExpenses = 0;
        List<Expense> allExpenses = DBConnection.MakeSelectQuery("SELECT * FROM dbo.Expenses");

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
            try
            {
                Expense expense = DBConnection.MakeSelectQuery($"SELECT * FROM dbo.Expenses WHERE ExpenseID = {id}")[0];

                return View(expense);
            }
            catch
            {
                return RedirectToAction("Error");
            }
        }

        return View();
    }

    public IActionResult DeleteExpense(int id)
    {
        try
        {
            DBConnection.MakeDeleteInsertUpdateQuery($"DELETE FROM dbo.Expenses WHERE ExpenseID = {id}");
        }
        catch
        {
            return RedirectToAction("Error");
        }

        return RedirectToAction("Expenses");
    }

    public IActionResult CreateEditExpenseForm(Expense model)
    {
        if (model.Id != 0) {
            try
            {
                DBConnection.MakeDeleteInsertUpdateQuery($"UPDATE dbo.Expenses SET ExpenseValue = '{model.Value}', ExpenseDescription = '{model.Description}' WHERE ExpenseID = {model.Id}");

                return RedirectToAction("Expenses");
            }
            catch
            {
                return RedirectToAction("Error");
            }
        }
        else
        {
            try
            {
                DBConnection.MakeDeleteInsertUpdateQuery($"INSERT INTO dbo.Expenses (ExpenseValue, ExpenseDescription) VALUES ('{model.Value}', '{model.Description}')");

                return RedirectToAction("Expenses");
            }
            catch
            {
                return RedirectToAction("Error");
            }
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
