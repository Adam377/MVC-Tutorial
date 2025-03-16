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
            try
            {
                string connectionString = "Data Source=(localdb)\\mvclocaldb;Initial Catalog=ExpensesDB";

                string sqlQuery = $"SELECT * FROM dbo.Expenses WHERE ExpenseID = {id}";

                SqlConnection con = new SqlConnection(connectionString);

                con.Open();
                SqlCommand sc = new SqlCommand(sqlQuery, con);

                Expense expense = new Expense();

                using (SqlDataReader reader = sc.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        expense.Id = int.Parse(reader["ExpenseID"].ToString());
                        expense.Value = decimal.Parse(reader["ExpenseValue"].ToString());
                        expense.Description = reader["ExpenseDescription"].ToString();
                    }
                }

                con.Close();

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
            string connectionString = "Data Source=(localdb)\\mvclocaldb;Initial Catalog=ExpensesDB";

            string sqlQuery = $"DELETE FROM dbo.Expenses WHERE ExpenseID = {id}";

            SqlConnection con = new SqlConnection(connectionString);

            con.Open();
            SqlCommand sc = new SqlCommand(sqlQuery, con);
            sc.ExecuteNonQuery();
            con.Close();
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
                string connectionString = "Data Source=(localdb)\\mvclocaldb;Initial Catalog=ExpensesDB";

                string sqlQuery = $"UPDATE dbo.Expenses SET ExpenseValue = '{model.Value}', ExpenseDescription = '{model.Description}' WHERE ExpenseID = {model.Id}";

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
        else
        {
            try
            {
                string connectionString = "Data Source=(localdb)\\mvclocaldb;Initial Catalog=ExpensesDB";

                string sqlQuery = $"INSERT INTO dbo.Expenses (ExpenseValue, ExpenseDescription) VALUES ('{model.Value}', '{model.Description}')";

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
