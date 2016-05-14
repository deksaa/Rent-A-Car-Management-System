using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace RentACarManagementSystem
{
    public partial class SettingsForm : Form
    {
        static string connectionString = "server=.\\DESTAN;Database=RentACar;Integrated Security=True";
        SqlConnection connection = new SqlConnection(connectionString);
        SqlCommand command;
        SqlDataReader dataReader;
        private readonly Form2 _form2;
        Boolean isDefault = false;
        string path;
        string defaultFileName = "C:\\Users\\erikd\\Documents\\Visual Studio 2015\\Projects\\RentACarManagementSystem\\Photos\\default.jpg";
        static string destinationDirectory = "C:\\Users\\erikd\\Documents\\Visual Studio 2015\\Projects\\RentACarManagementSystem\\Photos\\";
        public SettingsForm(Form2 form2)
        {
            _form2 = form2;
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                textBox1.PasswordChar = '•';
            else
                textBox1.PasswordChar = '\0';
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
                textBox2.PasswordChar = '•';
            else
                textBox2.PasswordChar = '\0';
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            loadPicture(_form2.activeUserInfos);
            makeAConnection();
            if (isDefault != true)
            {
                button3.Enabled = false;
                label4.Text = "You already have a photo.";
            }
            else
            {
                label3.Text = "Default Photo";
                label4.Text = "You can set your profile picture.\n" +
                    "Be careful you have one chance \nto choose any photo." +
                    "After that you \ncannot change " +
                    "your profile photo.";
            }
            
        }

        public void loadPicture(string activeUserInfos)
        {
            string fileName = destinationDirectory + activeUserInfos + ".Jpg";
            
            if (File.Exists(fileName))
            {
                pictureBox1.Load(fileName);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }
            else
            {
                isDefault = true;
                pictureBox1.Load(defaultFileName);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void makeAConnection()
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

        

        private void button1_Click(object sender, EventArgs e)
        {
            string queryControlPass = "SELECT Password FROM USERS WHERE UserName=@name AND Password=@pass;";
            string queryChangePass = "UPDATE USERS SET Password=@Newpass WHERE UserName=@name AND Password=@Oldpass;";

            if (!(textBox1.Text == string.Empty) && !(textBox2.Text == string.Empty))
            {
                if (connection.State == ConnectionState.Closed) connection.Open();

                command = new SqlCommand(queryControlPass, connection);
                command.Parameters.AddWithValue("@name", _form2.name);
                command.Parameters.AddWithValue("@pass", textBox1.Text);
                dataReader = command.ExecuteReader();
                dataReader.Read();
                if (dataReader.HasRows)
                {
                    try
                    {
                        dataReader.Close();
                        command = new SqlCommand(queryChangePass, connection);
                        command.Parameters.AddWithValue("@Newpass", textBox2.Text);
                        command.Parameters.AddWithValue("@name", _form2.name);
                        command.Parameters.AddWithValue("@Oldpass", textBox1.Text);
                        if (command.ExecuteNonQuery() > 0)
                            MessageBox.Show("Password is changed succesfully.");
                        else
                            MessageBox.Show("Something is wrong.");
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show(error.Message);
                        connection.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Current password is wrong.");
                }

            }
            else
            {
                MessageBox.Show("Must fill the field(s)");
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string newName;
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Image Files (*.jpg, *.gif, *.bmp)|*.jpg*;*.gif;*.bmp";
            try
            {
                if (file.ShowDialog() == DialogResult.OK) path = file.FileName;
                newName = _form2.activeUserInfos + ".jpg";
                File.Copy(path, destinationDirectory + newName);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }
    }
}
