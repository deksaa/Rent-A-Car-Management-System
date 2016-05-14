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
    public partial class UpdateCar : Form
    {
        static string connectionString = "server=.\\DESTAN;Database=RentACar;Integrated Security=True";
        SqlConnection connection = new SqlConnection(connectionString);
        SqlCommand command;
        string carid;
        public UpdateCar()
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
            string query = "SELECT Car_id,Brand,Model,P_Year,Plate,Fuel,GearBox,Price FROM CARS;";
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
        
    

        private void UpdateCar_Load(object sender, EventArgs e)
        {
            makeAconnection();
            fillTheDataGrid();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            carid = dataGridView1.Rows[e.RowIndex].Cells["Car_id"].Value.ToString();
            textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells["Brand"].Value.ToString();
            textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells["Model"].Value.ToString();
            textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells["P_Year"].Value.ToString();
            textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells["Plate"].Value.ToString();
            textBox5.Text = dataGridView1.Rows[e.RowIndex].Cells["Price"].Value.ToString();
            comboBox1.Text = dataGridView1.Rows[e.RowIndex].Cells["Fuel"].Value.ToString();
            comboBox2.Text = dataGridView1.Rows[e.RowIndex].Cells["GearBox"].Value.ToString();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string query = "UPDATE CARS SET Brand=@brand,Model=@model,P_year=@pyear,Plate=@plate," +
                            "Price=@price,Fuel=@fuel,GearBox=@gearbox WHERE Car_id=@id";

            try
            {
                command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id",carid);
                command.Parameters.AddWithValue("@brand", textBox1.Text);
                command.Parameters.AddWithValue("@model", textBox2.Text);
                command.Parameters.AddWithValue("@pyear", textBox3.Text);
                command.Parameters.AddWithValue("@plate", textBox4.Text);
                command.Parameters.AddWithValue("@price", textBox5.Text);
                command.Parameters.AddWithValue("@fuel", comboBox1.Text);
                command.Parameters.AddWithValue("@gearbox", comboBox2.Text);
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
