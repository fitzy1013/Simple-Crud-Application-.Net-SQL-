using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace WebApplication1.Pages.Client
{
    public class IndexModel : PageModel
    {
        public List<EmployeeInfo> listEmployees = new List<EmployeeInfo>();

        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=localhost;Initial Catalog=mystore;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";


                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM employees";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                EmployeeInfo employeeInfo = new EmployeeInfo();
                                employeeInfo.id = "" + reader.GetInt32(0);
                                employeeInfo.firstName = reader.GetString(1);
                                employeeInfo.lastName = reader.GetString(2);
                                employeeInfo.email = reader.GetString(3);
                                employeeInfo.phone = reader.GetString(4);
                                employeeInfo.created_at = reader.GetDateTime(5).ToString();

                                listEmployees.Add(employeeInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    public class EmployeeInfo
    {
        public String id;
        public String firstName;
        public String lastName;
        public String email;
        public String phone;
        public String created_at;
    }
}
