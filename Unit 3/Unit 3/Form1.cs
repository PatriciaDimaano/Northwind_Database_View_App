using Microsoft.VisualBasic.ApplicationServices;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Unit_3
{
    public partial class Form1 : Form
    {
        private string srvr;
        private string db;
        private string uname;
        private string pword;
        public Form1(string server, string database, string user, string pass)
        {
            InitializeComponent();

            srvr = server;
            db = database;
            uname = user;
            pword = pass;

            SetUserRecordPermissions();
            Greeting.Text = "Hello, " + user + "!";
            Greeting.Visible = true;
        }

        private void SetUserRecordPermissions()
        {
            Data data = new Data(srvr, db, uname, pword);
            List<string> AccessibleRecords = new List<string>();

            try
            {
                using (SqlConnection connection = new SqlConnection(data.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("select is_member('CEORole')", connection))
                    {
                        if ((int)command.ExecuteScalar() == 1)
                        {
                            AccessibleRecords.Add("Employees");
                            AccessibleRecords.Add("Orders");
                            AccessibleRecords.Add("Customers");
                        }
                    }

                    using (SqlCommand command = new SqlCommand("select is_member('SalesRole')", connection))
                    {
                        if ((int)command.ExecuteScalar() == 1)
                        {
                            AccessibleRecords.Add("Orders");
                            AccessibleRecords.Add("Customers");
                        }
                    }

                    using (SqlCommand command = new SqlCommand("select is_member('HRRole')", connection))
                    {
                        if ((int)command.ExecuteScalar() == 1)
                        {
                            AccessibleRecords.Add("Employees");
                        }
                    }
                }
            }

            catch (SqlException ex)
            {
                MessageBox.Show("Please make sure that the user ID and password you entered are both correct.\n\n Error message: " 
                    + ex.Message, "Database Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoginForm login = new LoginForm();
                this.Hide();
                login.ShowDialog();
                this.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            foreach (string table in AccessibleRecords)
            {
                Selection.Items.Add(table);
            }

            Selection.SelectedIndex = 0;
        }

        private void LoadBtn_Click(object sender, EventArgs e)
        {
                Data data = new Data(srvr, db, uname, pword);
                DataSet dataset = new DataSet();

                using (SqlConnection connection = new SqlConnection(data.ConnectionString))
                {
                    connection.Open();

                    switch (Selection.SelectedItem.ToString())
                    {
                        case "Employees":
                            LoadEmployees(connection, data, dataset);
                            break;

                        case "Orders":
                            LoadOrders(connection, data, dataset);
                            break;

                        case "Customers":
                            LoadCustomers(connection, data, dataset);
                            break;
                    }
                }
                DataView.DataSource = dataset.Tables[0];
        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            this.Hide();
            login.ShowDialog();
            this.Close();
        }

        private void LoadEmployees(SqlConnection connection, Data data, DataSet dataset)
        {
            using (SqlCommand DataCount = new SqlCommand(data.EmployeeCount, connection))
            {
                object number = DataCount.ExecuteScalar();
                DataNumber.Text = "Number of Data: " + number.ToString();
                DataNumber.Visible = true;
            }
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(data.EmployeeList, connection))
            {
                dataAdapter.Fill(dataset);
            }
        }

        private void LoadOrders(SqlConnection connection, Data data, DataSet dataset)
        {
            using (SqlCommand DataCount = new SqlCommand(data.OrderCount, connection))
            {
                object number = DataCount.ExecuteScalar();
                DataNumber.Text = "Number of Data: " + number.ToString();
                DataNumber.Visible = true;
            }
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(data.OrderList, connection))
            {
                dataAdapter.Fill(dataset);
            }
        }

        private void LoadCustomers(SqlConnection connection, Data data, DataSet dataset)
        {
            using (SqlCommand DataCount = new SqlCommand(data.CustomerCount, connection))
            {
                object number = DataCount.ExecuteScalar();
                DataNumber.Text = "Number of Data: " + number.ToString();
                DataNumber.Visible = true;
            }
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(data.CustomerList, connection))
            {
                dataAdapter.Fill(dataset);
            }
        }
    }
}
