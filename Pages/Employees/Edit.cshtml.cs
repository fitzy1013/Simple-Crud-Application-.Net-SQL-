using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;
using WebApplication1.Pages.Client;

namespace WebApplication1.Pages.Employees
{
    public class EditModel : PageModel
    {
        public EmployeeInfo employeeInfo = new EmployeeInfo();
        public String errorMessage = "";
        public String successMessage = "";

        public void OnGet()
        {
            String id = Request.Query["id"];
            try
            {
                String connectionString = "Data Source=localhost;Initial Catalog=mystore;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    String sql = "SELECT * FROM employees WHERE id=@id";

                    using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                employeeInfo.id = "" + reader.GetInt32(0);
                                employeeInfo.firstName = reader.GetString(1);
                                employeeInfo.lastName = reader.GetString(2);
                                employeeInfo.email = reader.GetString(3);
                                employeeInfo.phone = reader.GetString(4);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
        }

        public void OnPost()
        {
            employeeInfo.id = Request.Query["id"];
            employeeInfo.firstName = Request.Form["firstName"];
            employeeInfo.lastName = Request.Form["lastName"];
            employeeInfo.phone = Request.Form["phone"];
            employeeInfo.email = Request.Form["email"];

            if (employeeInfo.firstName.Length == 0 || employeeInfo.lastName.Length == 0 ||
                employeeInfo.phone.Length == 0 || employeeInfo.email.Length == 0)
            {
                errorMessage = "All Fields are Required";
                return;
            }

            try
            {
                String connectionString = "Data Source=localhost;Initial Catalog=mystore;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    String sql = "UPDATE employees " +
                                 "SET firstName=@firstName, lastName=@lastName, email=@email, phone=@phone " +
                                 "WHERE id=@id";

                    using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("firstName", employeeInfo.firstName);
                        sqlCommand.Parameters.AddWithValue("lastName", employeeInfo.lastName);
                        sqlCommand.Parameters.AddWithValue("email", employeeInfo.email);
                        sqlCommand.Parameters.AddWithValue("phone", employeeInfo.phone);
                        sqlCommand.Parameters.AddWithValue("id", employeeInfo.id);

                        sqlCommand.ExecuteNonQuery();
                    }


                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Employees/Index");
        }
    }
}
