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
    public partial class UpdateAndDelete : Form
    {
        static string connectionString = "server=.\\DESTAN;Database=RentACar;Integrated Security=True";
        SqlConnection connection = new SqlConnection(connectionString);
        public string taxid = "";
        SqlCommand command;

        public UpdateAndDelete()
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
            string query = "SELECT T.*,C.Brand,C.Model FROM TAXES T INNER JOIN CARS C ON T.Car_id=C.Car_id;";
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

        private void UpdateAndDelete_Load(object sender, EventArgs e)
        {
            makeAconnection();
            fillTheDataGrid();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            taxid = dataGridView1.Rows[e.RowIndex].Cells["Tax_id"].Value.ToString();
            textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells["TaxAmount"].Value.ToString();
        }

        private void updateRow()
        {
            string queryUpdate = "UPDATE TAXES SET TaxAmount=@amount,Next_PayDate=@date WHERE Tax_id=@id";

            try
            {
                command = new SqlCommand(queryUpdate, connection);
                command.Parameters.AddWithValue("@amount", textBox1.Text);
                command.Parameters.AddWithValue("@date", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@id", taxid);

                if (command.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Record updated succesfully.");
                    taxid = "";
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }
        private void deleteRow()
        {
            string queryDelete = "DELETE FROM TAXES WHERE Tax_id=@taxid;";

            try
            {
                command = new SqlCommand(queryDelete, connection);
                command.Parameters.AddWithValue("@taxid", taxid);

                if (command.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Record deleted succesfully.");
                    taxid = "";
                }

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (taxid == "")
            {
                MessageBox.Show("There is no selected item");
            }
            else
            {
                deleteRow();
                fillTheDataGrid();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (taxid == "")
            {
                MessageBox.Show("There is no selected item");
            }
            else
            {
                updateRow();
                fillTheDataGrid();
            }
        }
    }
}
