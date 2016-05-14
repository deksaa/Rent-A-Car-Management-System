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
    public partial class UserAdd : Form
    {

        static string connectionString = "server=.\\DESTAN;Database=RentACar;Integrated Security = True";
        SqlConnection connection = new SqlConnection(connectionString);
        SqlCommand command;
        SqlDataReader dataReader;

        public UserAdd()
        {
            InitializeComponent();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar)
                 && !char.IsSeparator(e.KeyChar);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar)
                 && !char.IsSeparator(e.KeyChar);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) 
                && !char.IsControl(e.KeyChar) && !char.IsSeparator(e.KeyChar);
        }

        private int controlFields()
        {
            errorProvider1.Clear();
            foreach (Control item in groupBox1.Controls)
            {
                if (item is TextBox)
                {
                    if (item.Text == "")
                    {
                        errorProvider1.SetError(item, "Fill the field.");
                        return -1;
                    }
                         
                }
            }
            return 1;
        }

        private void controlPermission()
        {
            string queryControl = "SELECT UserName FROM USERS WHERE Position=1;";
            string baskan = "destan";
            command = new SqlCommand(queryControl, connection);
            
            try
            {
                dataReader = command.ExecuteReader();
                dataReader.Read();
                string name = dataReader["UserName"].ToString();
                if (name != baskan)
                {
                    
                    groupBox1.Enabled = false;
                    label6.Text = "Only system administrator can add new user.\n" +
                                  "Please apply to your system administrator.";
                }
            }catch(Exception error)
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

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            int count = textBox3.Text.Count();
            if(count < 5)
            {
                label7.Text = "Strong Level:Poor";
                label7.ForeColor = Color.Red;
            }
            else if(count < 7)
            {
                label7.Text = "Strong Level:Not bad";
                label7.ForeColor = Color.SkyBlue;
            }
            else
            {
                label7.Text = "Strong Level:Good";
                label7.ForeColor = Color.Green;
            }
        }

        private void UserAdd_Load(object sender, EventArgs e)
        {
            label6.Text = "";
            makeAconnection();
            controlPermission();
            label7.Text = "Strong Level:";
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string queryAdd = "INSERT INTO USERS VALUES(@name,@sname,@position,@pass);";

            if (controlFields() < 0) return;
            try
            {
                command = new SqlCommand(queryAdd, connection);
                command.Parameters.AddWithValue("@name", textBox1.Text);
                command.Parameters.AddWithValue("@sname", textBox2.Text);
                command.Parameters.AddWithValue("@position", 0);
                command.Parameters.AddWithValue("@pass", textBox3.Text);
                if (command.ExecuteNonQuery() > 0)
                    label6.Text = "Operation is done\nNew User's user name : " + textBox1.Text + " and pass : " + textBox3.Text +"." ;
                else
                    label6.Text = "Something is wrong";

            }catch(Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }
    }
}
