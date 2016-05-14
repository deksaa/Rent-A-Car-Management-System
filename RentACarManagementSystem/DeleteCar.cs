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
    public partial class DeleteCar : Form
    {
        public static string connectionString = "server=.\\DESTAN;Database=RentACar;Integrated Security = True";
        SqlConnection connection = new SqlConnection(connectionString);
        SqlDataAdapter dataAdapter;
        SqlCommand command;
        string deletedCar = "";

        public DeleteCar()
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

        private int fillTheDataGrid()
        {
            int count = 0;
            string query = "SELECT * FROM CARS;";
            try
            {
                dataAdapter = new SqlDataAdapter(query, connection);            
                DataSet dataSet = new DataSet();
                count = dataAdapter.Fill(dataSet);                       
                dataGridView1.DataSource = dataSet.Tables[0];  
            }                                                                   
            catch (Exception error)                                             
            {                                                                   
                MessageBox.Show(error.Message);
            }

            return count;
        }

        private string getCells(DataGridView dataGrid, int index)
        {
            int i = 0;
            deletedCar = "";
            string[] cellDatas = new string[8];
            for (i = 0; i < 8; i++)
            {
                cellDatas[i] = dataGrid.Rows[index].Cells[i].Value.ToString();
                deletedCar += cellDatas[i].ToUpper() + " ";
        }
            return deletedCar;
        }

        private void deleteRow(DataGridView dataGrid, int index, string info)
        {
            string queryDelete = "DELETE FROM CARS WHERE Car_id=@carid;";

            try{
                command = new SqlCommand(queryDelete, connection);
                command.Parameters.AddWithValue("@carid", dataGrid.Rows[index].Cells["Car_id"].Value.ToString());

                if (command.ExecuteNonQuery() > 0)
                {
                    label2.Text =   " Car you have.";
                    listBox1.Items.Add("Operation is succeed.Deleted Item : " + info);
                }
                else
                {
                    listBox1.Items.Add("Something is wrong.\n");
                }

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void DeleteCar_Load(object sender, EventArgs e)
        {
            makeAconnection();
            label1.Text = "If you want to delete any row \nyou can click one times on the row.";
            label2.Text = "";
            fillTheDataGrid();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to delete?", "Warning", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                deleteRow(dataGridView1,e.RowIndex,getCells(dataGridView1, e.RowIndex));
                label2.Text = fillTheDataGrid().ToString() + " Car in the store.";
            }
        }
    }
}
