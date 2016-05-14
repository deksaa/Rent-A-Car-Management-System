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
    public partial class UserDelete : Form
    {
        static string connectionString = "server=.\\DESTAN;Database=RentACar;Integrated Security = True";
        SqlConnection connection = new SqlConnection(connectionString);
        SqlCommand command;
        SqlDataReader dataReader;
        SqlDataAdapter dataAdapter;
        string baskan = "destan";
        string U_name;
        string id, name, surname, password;
        String query = "DELETE FROM USERS WHERE Userid=@userid AND UserName=@name AND UserSname=@sname AND Password=@pass";

        public UserDelete()
        {
            InitializeComponent();
        }

        private void controlPermission()
        {
            string queryControl = "SELECT UserName FROM USERS WHERE Position=1;";
            
            command = new SqlCommand(queryControl, connection);

            try
            {
                dataReader = command.ExecuteReader();
                dataReader.Read();
                U_name = dataReader["UserName"].ToString();
                if (U_name != baskan)
                {

                    groupBox1.Enabled = false;
                    label1.Text = "Only system administrator can delete user.\n" +
                                  "Please apply to your system administrator.";
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
            dataReader.Close();
            command.Cancel();
        }

        private void makeAconnection()
        {
            try
            {
                connection.Open();
            }catch(Exception error)
            {
                MessageBox.Show(error.Message); 
            }
        }

        private void fillTheDataGrid()
        {
            string query = "SELECT Userid,UserName,UserSname,Password FROM USERS;";
            try
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                dataGridView1.DataSource = dataSet.Tables[0];
            }catch(Exception error)
            {
                MessageBox.Show(error.Message);
            }
            
        }

        private void setNullStrings()
        {
            id = null;
            name = null;
            surname = null;
            password = null;
        }

        private void deleteUser()
        {
            try
            {
                command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@userid", id);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@sname", surname);
                command.Parameters.AddWithValue("@pass", password);
                if (command.ExecuteNonQuery() > 0)
                {
                    label2.Text = "Operation is done." + id + ". " + name + " " + surname + " deleted from database.";
                }
                else
                {
                    label2.Text = "Operation failed.The user didn't delete from database.";
                }
            }catch(Exception error)
            {
                MessageBox.Show(error.Message);
            }
            finally
            {
                setNullStrings();
            }
            
        }

        private void UserDelete_Load(object sender, EventArgs e)
        {
            makeAconnection();
            label1.Text = "All users are above.If you want to delete \nany one,you should click double.";
            label2.Text = " ";
            controlPermission();
            fillTheDataGrid();
            if (U_name != baskan) dataGridView1.DataSource = null;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to delete?", "Warning", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                id = dataGridView1.Rows[e.RowIndex].Cells["Userid"].Value.ToString();
                name = dataGridView1.Rows[e.RowIndex].Cells["UserName"].Value.ToString();
                surname = dataGridView1.Rows[e.RowIndex].Cells["UserSname"].Value.ToString();
                password = dataGridView1.Rows[e.RowIndex].Cells["Password"].Value.ToString();
                deleteUser();
                fillTheDataGrid();

            }
            
        }
    }
}
