using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Unit_3
{
    internal class Data
    {
        public String ConnectionString { get; private set; }

        public String EmployeeList = @"Select * from Employees";
        public String EmployeeCount = @"Select Count(EmployeeID) from Employees";
        public String OrderList = @"Select * from Orders";
        public String OrderCount = @"Select Count(OrderID) from Orders";
        public String CustomerList = @"Select * from Customers";
        public String CustomerCount = @"Select Count(CustomerID) from Customers";
        public Data(string server, string database, string user, string pass) {
            this.ConnectionString = $"Data source = {server}; " +
                $"Initial catalog = {database}; " +
                $"User ID = {user}; " +
                $"Password = {pass}";
        }

        public bool TestConnection()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (SqlException ex)
                {
                    return false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
}
