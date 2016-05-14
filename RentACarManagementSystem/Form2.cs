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
using System.IO;

namespace RentACarManagementSystem
{
    public partial class Form2 : Form
    {
        public static string connectionString = "server=.\\DESTAN;Database=RentACar;Integrated Security = True";
        SqlConnection connection = new SqlConnection(connectionString);
        SqlCommand command;
        SqlDataReader dataReader;
        string queryLogin;
        string queryFindActiveUser;
        public string activeUserInfos;
        public string name;
        
        

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            timer1_Tick(null, null);
            setUserLabel();
            loadPicture();
            load();
        }

        private string Fill(string s, int n)
        {
            int length = s.Length;
            for (int i = length + 1; i <= n; i++)
                s = s + " ";
            return s;
        }
        private void Save()
        {
            StreamWriter sw = new StreamWriter("Duyuru.txt");
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                string l = listBox1.Items[i].ToString();
                sw.WriteLine(l);
            }
            sw.Close();
        }
        private void load()
        {
            if (File.Exists("Duyuru.txt"))
            {
                StreamReader sr = new StreamReader("Duyuru.txt");
                listBox1.Items.Clear();
                while (!sr.EndOfStream)
                {
                    string l = sr.ReadLine();
                    listBox1.Items.Add(l);
                }
                sr.Close();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string s = DateTime.Now.DayOfWeek.ToString().ToUpper();
            label2.Text = Convert.ToString(DateTime.Now.Date).Substring(0, 10) + "       " + s;            
            label3.Text = Convert.ToString(DateTime.Now.TimeOfDay).Substring(0, 8);
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {//Bu method form kapanmadan evvel aktif olan kullanıcı bilgisini(boolean) USERS tablosunda false yapar.
            queryLogin = "UPDATE USERS SET Position=0 WHERE Position=1;";

            try
            {
                if (connection.State == ConnectionState.Closed) connection.Open();
                command = new SqlCommand(queryLogin, connection);
                command.ExecuteNonQuery();
            }catch(Exception error)
            {
                MessageBox.Show(error.Message);
                connection.Close();
            }
            finally
            {
                connection.Close();
                command.Cancel();
                Form1 f = new Form1();
                f.Show();
            } 
        }

        public void setUserLabel()
        { 
            queryFindActiveUser = "SELECT UserName,UserSname FROM USERS WHERE Position=1;";

            try
            {
                if (connection.State == ConnectionState.Closed) connection.Open();
                command = new SqlCommand(queryFindActiveUser, connection);
                dataReader = command.ExecuteReader();
                dataReader.Read();
                if (dataReader.HasRows)
                {
                    name = dataReader["UserName"].ToString();
                    activeUserInfos = dataReader["UserName"].ToString().ToUpper() + " " + dataReader["UserSname"].ToString().ToUpper();
                    label1.Text = activeUserInfos;
                }
                
            }catch(Exception error)
            {
                MessageBox.Show(error.Message);
                connection.Close();
            }
            finally
            {
                connection.Close();
                command.Cancel();
            }
        }

        public void loadPicture()
        {
            string fileName = "C:\\Users\\erikd\\Documents\\Visual Studio 2015\\Projects\\RentACarManagementSystem\\Photos\\" + activeUserInfos + ".Jpg";

            if (File.Exists(fileName))
            {
                pictureBox1.Load(fileName);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }
            else
            {
                pictureBox1.Load("C:\\Users\\erikd\\Documents\\Visual Studio 2015\\Projects\\RentACarManagementSystem\\Photos\\default.jpg");
            }
        }

        private void linkLabel1_Click(object sender, EventArgs e)
        {
            SettingsForm sf = new SettingsForm(this);
            sf.ShowDialog();
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if(e.Node.Text == "Add Car")
            {
                AddForm addForm = new AddForm();
                addForm.ShowDialog();
            }

            if(e.Node.Text == "Delete Car")
            {
                DeleteCar deleteCar = new DeleteCar();
                deleteCar.ShowDialog();
            }

            if(e.Node.Text == "User Add")
            {
                UserAdd userAdd = new UserAdd();
                userAdd.ShowDialog();
            }

            if(e.Node.Text == "User Update")
            {
                UserUpdate userUpdate = new UserUpdate();
                userUpdate.ShowDialog();
            }

            if (e.Node.Text == "User Delete")
            {
                UserDelete userDelete = new UserDelete();
                userDelete.ShowDialog();
            }

            if (e.Node.Text == "Create Reservation")
            {
                CreateReservation create = new CreateReservation();
                create.ShowDialog();
            }

            if(e.Node.Text == "Update Car")
            {
                UpdateCar updateCar = new UpdateCar();
                updateCar.ShowDialog();
            }
            if (e.Node.Text == "Display Reservations")
            {
                DisplayReservation displayReservation =new DisplayReservation();
                displayReservation.ShowDialog();
            }
            if (e.Node.Text == "Cancel Reservation")
            {
                CancelReservation cancelReservation = new CancelReservation();
                cancelReservation.ShowDialog();
            }
            if (e.Node.Text == "Taxes Informations")
            {
                TaxesInformations taxesInformations = new TaxesInformations();
                taxesInformations.ShowDialog();
            }
            if (e.Node.Text == "Update Customers")
            {
                UpdateCustomer updateCustomer = new UpdateCustomer();
                updateCustomer.ShowDialog();
            }
            if (e.Node.Text == "Delete & Update")
            {
                UpdateAndDelete updateAndDelete = new UpdateAndDelete();
                updateAndDelete.ShowDialog();
            }

        }

        private void linkLabel2_MouseClick(object sender, MouseEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to close program?", "Warning", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                this.Close();
            }
            else if (dialogResult == DialogResult.No)
            {
                //Nothing
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string date = Fill(DateTime.Today.ToString("dd-MM-yyyy"), 15);
            string note = textBox1.Text;
            listBox1.Items.Add(date + " " + note);
            textBox1.Text = "";
            Save();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //string date = Fill(DateTime.Today.ToString("dd-MM-yyyy"), 15);
           
            int index = listBox1.SelectedIndex;
            listBox1.Items.RemoveAt(index);
            listBox1.Items.Add(textBox1.Text);
            textBox1.Text = "";
            Save();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("There is no selection to delete");
                return;
            }
            if (MessageBox.Show("Are you sure to delete?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                Save();
            }
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            textBox1.Text = listBox1.Items[listBox1.SelectedIndex].ToString();
        }
    }
}
