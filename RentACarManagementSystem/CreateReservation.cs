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
using System.Data.SqlTypes;
namespace RentACarManagementSystem
{
    public partial class CreateReservation : Form
    {
        public static string connectionString = "server=.\\DESTAN;Database=RentACar;Integrated Security = True";
        SqlConnection connection = new SqlConnection(connectionString);
        SqlDataAdapter dataAdapter;
        string selectedCar = "";
        string id = "";
        string ssn = "";
        string cust_id = "";
        string date1, date2;
        SqlDataReader dataReader;


        public CreateReservation()
        {
            InitializeComponent();
        }
        //-------------------------------------------------------------------------------------------------------
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
        //-------------------------------------------------------------------------------------------------------
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
        //-------------------------------------------------------------------------------------------------------
        private string getCells(DataGridView dataGrid, int index)
        {
            int i = 0;
            selectedCar = "";
            string[] cellDatas = new string[8];
            for (i = 0; i < 8; i++)
            {
                if (i == 0) id = dataGrid.Rows[index].Cells[i].Value.ToString();
                cellDatas[i] = dataGrid.Rows[index].Cells[i].Value.ToString();
                selectedCar += cellDatas[i].ToUpper() + " ";
            }
            return selectedCar;
        }
        //-------------------------------------------------------------------------------------------------------
        private void getCustId()
        {
            string queryGetCustId = "SELECT Cust_id FROM CUSTOMER WHERE ssn=@ssn";

            try{
                SqlCommand command = new SqlCommand(queryGetCustId, connection);
                command.Parameters.AddWithValue("@ssn", ssn);
                cust_id = command.ExecuteScalar().ToString();
            }catch(Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }
        //-------------------------------------------------------------------------------------------------------
        private void CreateReservation_Load(object sender, EventArgs e)
        {
            makeAconnection();
            label8.Text = "You have " + fillTheDataGrid().ToString() + " car in the store.";
            dateTimePicker3.Value = DateTime.Now;
            dateTimePicker3.Enabled = false;
        }
        //-------------------------------------------------------------------------------------------------------
        private void insertRent()
        {
            string insertQuery = "INSERT INTO RENT VALUES(@Cust_id,@Car_id,@Rent_Date,@Return_Date,@Operation_Date)";
            try
            {
                SqlCommand scom1 = new SqlCommand(insertQuery, connection);
                scom1.Parameters.AddWithValue("@Cust_id", cust_id);
                scom1.Parameters.AddWithValue("@Car_id", id);
                scom1.Parameters.AddWithValue("@Rent_Date", dateTimePicker1.Value);
                scom1.Parameters.AddWithValue("@Return_Date", dateTimePicker2.Value);
                scom1.Parameters.AddWithValue("@Operation_Date", dateTimePicker3.Value);
                if (scom1.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Reserved Succesfully");
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }

        }
        //-------------------------------------------------------------------------------------------------------
        private void insertCustomer()
        {
            string insertQuery = "INSERT INTO CUSTOMER VALUES(@FirstName,@SurName,@Address,@Phone,@ssn)";
            try
            {
                SqlCommand scom2 = new SqlCommand(insertQuery, connection);
                scom2.Parameters.AddWithValue("@FirstName", textBox1.Text);
                scom2.Parameters.AddWithValue("@SurName", textBox2.Text);
                scom2.Parameters.AddWithValue("@Address", textBox3.Text);
                scom2.Parameters.AddWithValue("@Phone", textBox4.Text);
                scom2.Parameters.AddWithValue("@ssn", textBox5.Text);
                if (scom2.ExecuteNonQuery() > 0)
                {
                    ssn = textBox5.Text;
                    getCustId();
                    //MessageBox.Show("OKcustomer");
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }

        }
        //-------------------------------------------------------------------------------------------------------
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to select?", "Warning", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                if (listBox1.Items.Count != 0) listBox1.Items.Clear();
                listBox1.Items.Add(getCells(dataGridView1, e.RowIndex));
            }
        }
        //-------------------------------------------------------------------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        {
            
            insertCustomer();
            insertRent();
        }
        //-------------------------------------------------------------------------------------------------------
        private void checkFields()
        {
            if (listBox1.Items.Count != 1)
            {
                MessageBox.Show("You didn't select a car");
                return;
            }

            errorProvider1.Clear();

            foreach (Control item in groupBox1.Controls)
            {
                if (item is TextBox)
                {
                    if (item.Text == "")
                    {

                        errorProvider1.SetError(item, "Fill the field");
                        return;
                    }
                }
            }
        }
        //------------------------------------------------------------------------------------------------------
        private int checkOverLapping()
        {
            int check = -1;
            string query = "SELECT R.* FROM CARS C INNER JOIN RENT R ON C.Car_id=R.car_id WHERE R.Car_id="+id.ToString()+" AND NOT " +
                           "((('"+date1+"' < R.Rent_Date)AND('"+ date2 + "' < R.Rent_Date)AND('"+ date1 + "' < R.Return_Date)AND('"+ date2 + "' < R.Return_Date)) OR " +
                           "(('"+ date1 + "' > R.Rent_Date)AND('"+ date1 + "' > R.Return_Date)AND('"+ date2 + "' > R.Return_Date)AND('"+ date2 + "' > R.Rent_Date))); ";
            try
            {
                SqlCommand command = new SqlCommand(query, connection);
                dataReader = command.ExecuteReader();
                dataReader.Read();
                if (!dataReader.HasRows)
                    check = 1; ;
                    //MessageBox.Show("Cakısma yok");
                
            }catch(Exception error)
            {
                MessageBox.Show(error.Message);
            }
            finally
            {
                dataReader.Close();
                
            }

            return check;
        }
        //-------------------------------------------------------------------------------------------------------
        private void button3_Click(object sender, EventArgs e)
        {
            
            checkFields();
            if (checkOverLapping() > 0)
                MessageBox.Show("Rent periot is available.");
            else
            {
                MessageBox.Show("Rent periot is not available.Change your periots.");
                button1.Enabled = false;
            }
                


        }
        //-------------------------------------------------------------------------------------------------------
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            date1 = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            button1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (Control item in groupBox1.Controls)
            {
                if (item is TextBox)
                {
                    if (item.Text != "")
                    {

                        item.Text = "";
                        return;
                    }
                }
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
           date2 = dateTimePicker2.Value.ToString("yyyy-MM-dd");
            button1.Enabled = true;
        }

        //-------------------------------------------------------------------------------------------------------
    }
}
