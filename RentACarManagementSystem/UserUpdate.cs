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
    public partial class UserUpdate : Form
    {
        static string connectionString = "server=.\\DESTAN;Database=RentACar;Integrated Security = True";
        SqlConnection connection = new SqlConnection(connectionString);
        SqlCommand command;
        SqlDataReader dataReader;
        public UserUpdate()
        {
            InitializeComponent();
        }

        private void makeAconnection()
        {
            try
            {
                connection.Open();
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void setTheTextboxes()
        {
            string query = "SELECT UserName,UserSname,Password FROM USERS WHERE Position=1;";
            command = new SqlCommand(query, connection);

            if (connection.State == ConnectionState.Closed)
                connection.Open();
            try
            {
                dataReader = command.ExecuteReader();
                dataReader.Read();
                if (dataReader.HasRows)
                {
                    textBox1.Text = dataReader["UserName"].ToString();
                    textBox2.Text = dataReader["UserSname"].ToString();
                    textBox3.Text = dataReader["Password"].ToString();
                }
            }catch(Exception error)
            {
                MessageBox.Show(error.Message);
            }
            finally
            {
                dataReader.Close();
            }
            

        }

        private void UserUpdate_Load(object sender, EventArgs e)
        {
            textBox3.PasswordChar = '•';
            textBox4.PasswordChar = '•';
            label5.Text = "If you dont want to change \npass you can enter nothing.";
            label6.Text = "";
            makeAconnection();
            setTheTextboxes();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                textBox3.PasswordChar = '•';
            else
                textBox3.PasswordChar = '\0';
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
                textBox4.PasswordChar = '•';
            else
                textBox4.PasswordChar = '\0';
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string queryUpdate = "UPDATE USERS SET UserName=@name,UserSname=@sname,Password=@pass WHERE Position=1;";
            command = new SqlCommand(queryUpdate, connection);

            try
            {
                command.Parameters.AddWithValue("@name", textBox1.Text);
                command.Parameters.AddWithValue("@sname", textBox2.Text);
                command.Parameters.AddWithValue("@pass", textBox4.Text);
                if (textBox4.Text == "")
                {
                    queryUpdate = "UPDATE USERS SET UserName=@name,UserSname=@sname WHERE Position=1";
                    command = new SqlCommand(queryUpdate, connection);
                    command.Parameters.AddWithValue("@name", textBox1.Text);
                    command.Parameters.AddWithValue("@sname", textBox2.Text);
                }
                    

                if (command.ExecuteNonQuery() > 0)
                    label6.Text = "Process is succeed.";
                else
                    label6.Text = "Something is wrong.";

            }catch(Exception error)
            {
                MessageBox.Show(error.Message);
            }
                  
        }
    }
}
