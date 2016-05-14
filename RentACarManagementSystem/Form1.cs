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
    public partial class Form1 : Form
    {
        static string connectionString = "server=.\\DESTAN;Database=RentACar;Integrated Security = True";
        SqlConnection connection = new SqlConnection(connectionString);
        string queryLogin;
        SqlCommand command;

        public Form1()
        {
            InitializeComponent();
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            if((textBox1.Text == string.Empty) && (textBox2.Text == string.Empty))
            {
                MessageBox.Show("Please fill the field(s).");
                return;
            }
            else
            {
                queryLogin = "SELECT * FROM USERS WHERE UserName=@username AND Password=@pass;";
                command = new SqlCommand(queryLogin, connection);
                command.Parameters.AddWithValue("@username", textBox1.Text);
                command.Parameters.AddWithValue("@pass", textBox2.Text);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                {
                    reader.Close();
                    updateState();
                    Form2 f2 = new Form2();
                    f2.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid username or password");
                    reader.Close();
                    textBox1.Clear();
                    textBox2.Clear();
                    return;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
                connection.Close();
            }

        }

        private void updateState()//Bu method basarılı bir girişten sonra girilen username'i databasede true olarak kayıt eder.
        {                         //Bu sayede sistemin kim tarafından kullanıldıgı tespit edilir.

            string usersQuery = "UPDATE USERS SET Position=1 WHERE UserName=@name;";
            SqlCommand commandToUpdate = new SqlCommand(usersQuery, connection);
            commandToUpdate.Parameters.AddWithValue("@name",textBox1.Text);

            try
            {
                if(connection.State == ConnectionState.Closed) connection.Open(); 
                commandToUpdate.ExecuteNonQuery();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
                connection.Close();
            }
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                textBox2.PasswordChar = '•';
            else
                textBox2.PasswordChar = '\0';
        }
    }
}
