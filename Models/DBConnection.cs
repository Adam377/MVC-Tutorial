using AspNetCoreGeneratedDocument;
using Microsoft.Data.SqlClient;

namespace MVC_Tutorial.Models
{
    public static class DBConnection
    {
        #region Members
        private static readonly string _connectionString = "Data Source=(localdb)\\mvclocaldb;Initial Catalog=ExpensesDB";
        #endregion

        #region Constructor
        #endregion

        #region Methods
        public static List<Expense> MakeSelectQuery(string sqlQuery)
        {
            SqlConnection con = new SqlConnection(_connectionString);
            con.Open();
            SqlCommand sc = new SqlCommand(sqlQuery, con);

            List<Expense> allExpenses = new List<Expense>();

            using (SqlDataReader reader = sc.ExecuteReader())
            {
                while (reader.Read())
                {
                    allExpenses.Add(new Expense
                    {
                        Id = int.Parse(reader["ExpenseID"].ToString()),
                        Value = decimal.Parse(reader["ExpenseValue"].ToString()),
                        Description = reader["ExpenseDescription"].ToString()
                    });
                }
            }

            con.Close();

            return allExpenses;
        }

        public static void MakeDeleteInsertUpdateQuery(string sqlQuery)
        {
            SqlConnection con = new SqlConnection(_connectionString);
            con.Open();
            SqlCommand sc = new SqlCommand(sqlQuery, con);
            sc.ExecuteNonQuery();
            con.Close();
        }
        #endregion
    }
}
