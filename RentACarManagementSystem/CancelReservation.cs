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
    public partial class CancelReservation : Form
    {
        static string connectionString = "server=.\\DESTAN;Database=RentACar;Integrated Security = True";
        SqlConnection connection = new SqlConnection(connectionString);
        SqlDataAdapter dataAdapter;
        SqlCommand command;
        string deletedRent = "";
        string rent_id = "";

        public CancelReservation()
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
            string query = "SELECT R.Rent_id,C.FirstName,C.SurName,K.Brand,K.Model,K.Price,Rent_Date,R.Return_Date " +
                            "FROM CUSTOMER C INNER JOIN RENT R ON C.Cust_id = R.Cust_id INNER JOIN CARS K ON " +
                            "K.Car_id = R.Car_id";
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
            deletedRent = "";
            string[] cellDatas = new string[8];
            for (i = 0; i < 8; i++)
            {
                if(i==0) rent_id = dataGrid.Rows[index].Cells[i].Value.ToString();
                if (i == 6 || i == 7)
                {
                    cellDatas[i] = dataGrid.Rows[index].Cells[i].Value.ToString().Substring(0,10);
                    deletedRent += cellDatas[i].ToUpper() + " ";
                    continue;
                }
                else
                {
                    cellDatas[i] = dataGrid.Rows[index].Cells[i].Value.ToString();
                    deletedRent += cellDatas[i].ToUpper() + " ";
                }
                
            }
            return deletedRent;
        }

        private void deleteRow(DataGridView dataGrid, int index, string info)
        {
            string queryDelete = "DELETE FROM RENT WHERE Rent_id=@id;";

            try
            {
                command = new SqlCommand(queryDelete, connection);
                command.Parameters.AddWithValue("@id", rent_id);

                if (command.ExecuteNonQuery() > 0)
                {
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

        private void CancelReservation_Load(object sender, EventArgs e)
        {
            makeAconnection();
            label1.Text = "If you want to delete any reservation \nyou can click one times on the row.";
            label2.Text = "";
            fillTheDataGrid();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to delete?", "Warning", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                deleteRow(dataGridView1, e.RowIndex, getCells(dataGridView1, e.RowIndex));
                label2.Text = fillTheDataGrid().ToString() + " Reservations indexed.";
            }
        }
    }
}
