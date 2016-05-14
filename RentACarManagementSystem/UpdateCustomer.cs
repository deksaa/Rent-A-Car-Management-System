using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace RentACarManagementSystem
{
    public partial class UpdateCustomer : Form
    {
        static string connectionString = "server=.\\DESTAN;Database=RentACar;Integrated Security=True";
        SqlConnection connection = new SqlConnection(connectionString);
        SqlCommand command;
        string cust_id;
        public UpdateCustomer()
        {
            InitializeComponent();
        }

        private void makeAconnection()
        {
            try
            {
                connection.Open();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void fillTheDataGrid()
        {
            string query = "SELECT Cust_id,FirstName,SurName,Address,PhoneNumber,SSN FROM CUSTOMER;";
            try
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                dataGridView1.DataSource = dataSet.Tables[0];
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }

        }

        private void UpdateCustomer_Load(object sender, EventArgs e)
        {
            makeAconnection();
            fillTheDataGrid();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cust_id = dataGridView1.Rows[e.RowIndex].Cells["Cust_id"].Value.ToString();
            textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells["FirstName"].Value.ToString();
            textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells["SurName"].Value.ToString();
            textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells["Address"].Value.ToString();
            textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells["PhoneNumber"].Value.ToString();
            textBox5.Text = dataGridView1.Rows[e.RowIndex].Cells["SSN"].Value.ToString();
            
        }

        private void Update_Click(object sender, EventArgs e)
        {
            string query = "UPDATE CUSTOMER SET FirstName=@fn,SurName=@sn,Address=@ad,PhoneNumber=@pn," +
                            "SSN=@ssn WHERE Cust_id=@cust_id";

            try
            {
                command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@cust_id", cust_id);
                command.Parameters.AddWithValue("@fn", textBox1.Text);
                command.Parameters.AddWithValue("@sn", textBox2.Text);
                command.Parameters.AddWithValue("@ad", textBox3.Text);
                command.Parameters.AddWithValue("@pn", textBox4.Text);
                command.Parameters.AddWithValue("@ssn", textBox5.Text);
                if (command.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Row changed succesfully.");
                    fillTheDataGrid();
                }

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }
    }
}
