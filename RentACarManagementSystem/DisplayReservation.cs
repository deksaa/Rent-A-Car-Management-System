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
    public partial class DisplayReservation : Form
    {
        public static string connectionString = "server=.\\DESTAN;Database=RentACar;Integrated Security = True";
        SqlConnection connection = new SqlConnection(connectionString);
        SqlDataAdapter dataAdapter;
        string querySearch = "";
        public DisplayReservation()
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
            string query = "SELECT C.FirstName,C.SurName,K.Brand,K.Model,K.Price,R.Rent_Date,R.Return_Date "+
                            "FROM CUSTOMER C INNER JOIN RENT R ON C.Cust_id = R.Cust_id INNER JOIN CARS K ON "+
                            "K.Car_id = R.Car_id";
            try
            {
                dataAdapter = new SqlDataAdapter(query, connection);
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                dataGridView1.DataSource = dataSet.Tables[0];
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

       private string setQuery(string tableName)
        {
            querySearch = "SELECT C.FirstName,C.SurName,K.Brand,K.Model,K.Price,R.Rent_Date,R.Return_Date " +
                            "FROM CUSTOMER C INNER JOIN RENT R ON C.Cust_id = R.Cust_id INNER JOIN CARS K ON " +
                            "K.Car_id = R.Car_id WHERE " + tableName + " LIKE '" ;

            return querySearch;
        }


        private void searchDataGrid(string queryStatement)
        {
            queryStatement = queryStatement + textBox1.Text + "%';";
            try
            {
                dataAdapter = new SqlDataAdapter(queryStatement, connection);
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                dataGridView1.DataSource = dataSet.Tables[0];
                queryStatement = querySearch;
            }catch(Exception error)
            {
                MessageBox.Show(error.Message);
            }
            
        }

        private void DisplayReservation_Load(object sender, EventArgs e)
        {

            makeAconnection();
            fillTheDataGrid();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            setQuery(checkBox1.Text);
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

            setQuery(checkBox2.Text);
            checkBox1.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

            setQuery(checkBox3.Text);
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox4.Checked = false;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

            setQuery(checkBox4.Text);
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            fillTheDataGrid();
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
                searchDataGrid(querySearch);
            else
                fillTheDataGrid();
        }
    }
}
