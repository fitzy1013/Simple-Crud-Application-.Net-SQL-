using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Pages.Client;
using System.Data.SqlClient;

namespace WebApplication1.Pages.Employees
{
    public class CreateModel : PageModel
    {
        public EmployeeInfo employeeInfo = new EmployeeInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost() 
        {
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

            // adding user to the database

            try
            {
                String connectionString = "Data Source=localhost;Initial Catalog=mystore;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    String sql = "INSERT INTO employees " +
                                 "(firstName, lastName, email, phone) VALUES " +
                                 "(@firstName, @lastName, @email, @phone);";

                    using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("firstName", employeeInfo.firstName);
                        sqlCommand.Parameters.AddWithValue("lastName", employeeInfo.lastName);
                        sqlCommand.Parameters.AddWithValue ("email", employeeInfo.email);
                        sqlCommand.Parameters.AddWithValue("phone", employeeInfo.phone);

                        sqlCommand.ExecuteNonQuery();
                    }


                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
                return;
            }

            employeeInfo.firstName = ""; employeeInfo.lastName = ""; employeeInfo.email = ""; employeeInfo.phone = "";
            successMessage = "New Employee Added Succesfully";

            Response.Redirect("/Employees/Index");
        }
    }
}
