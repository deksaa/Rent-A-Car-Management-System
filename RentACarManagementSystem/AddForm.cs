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
    public partial class AddForm : Form
    {

        static string connectionString = "server=.\\DESTAN;Database=RentACar;Integrated Security=True";
        SqlConnection connection = new SqlConnection(connectionString);
        SqlCommand command;

        public AddForm()
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
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar)
                 && !char.IsSeparator(e.KeyChar);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar)
                 && !char.IsSeparator(e.KeyChar);
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        public int controlFields()
        {
            int count=7;
            if (checkBox1.Checked) count = 8;
            errorProvider1.Clear();

            foreach (Control item in groupBox1.Controls)
            {
                if (item is TextBox)
                {
                    if (item.Text == "")
                    {
                        
                        errorProvider1.SetError(item, "Fill the field");
                        count--;
                    }     
                }
                else if(item is ComboBox)
                {
                    if (item.Text == "")
                    {
                        errorProvider1.SetError(item, "Fill the field");
                        count--;
                    }   
                }
            }
            return count;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label8.Text = "";
            foreach (Control item in groupBox1.Controls)
            {
                if (item is TextBox)
                {
                    if (item.Text != "")
                        item.Text = "";
                }
                else if (item is ComboBox)
                {
                    if (item.Text != "")
                        item.Text = "";
                }
            }
            textBox6.Text = "1";
            checkBox1.Checked = false;
            textBox6.Enabled = false;
            errorProvider1.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string queryAdd = "INSERT INTO CARS VALUES(@BRAND,@MODEL,@P_YEAR,@PLATE,@FUEL,@GEARBOX,@PRICE);";
            int fleetCount = 1;
            if (checkBox1.Checked) fleetCount = Convert.ToInt32(textBox6.Text);
            if (!(controlFields() == 7) && checkBox1.Checked == false) return;
            if (!(controlFields() == 8) && checkBox1.Checked == true) return;


            try
            {
                for(int i = 0; i < fleetCount; i++)
                {
                    command = new SqlCommand(queryAdd, connection);
                    command.Parameters.AddWithValue("@BRAND", textBox1.Text);
                    command.Parameters.AddWithValue("@MODEL", textBox2.Text);
                    command.Parameters.AddWithValue("@P_YEAR", textBox3.Text);
                    command.Parameters.AddWithValue("@PLATE", textBox4.Text);
                    command.Parameters.AddWithValue("@FUEL", comboBox1.Text);
                    command.Parameters.AddWithValue("@GEARBOX", comboBox2.Text);
                    command.Parameters.AddWithValue("@PRICE", textBox5.Text);

                    if (command.ExecuteNonQuery() > 0)
                    {
                        if (fleetCount >= 1) label8.Text = fleetCount.ToString() + " car added succesfully.";
                    }
                    else
                    {
                        label8.Text = "Something is wrong.";
                    }
                }
                

            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void AddForm_Load(object sender, EventArgs e)
        {
            label8.Text = "";
            label9.Text = "";
            label9.Text = "You can change informations about cars \non the Update Car Form.";
            textBox6.Enabled = false;
            textBox6.Text = "1";

            try
            {
                connection.Open();
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox6.Enabled = true;
                textBox6.Text = "1";
                MessageBox.Show("You dont have to enter plate number to add fleet.\n" +
                                "After you can enter plate informations one by one on Update Car Form");
                textBox4.Enabled = false;
                textBox4.Text = "after should be set";
            }
            else
            {
                textBox6.Clear();
                textBox6.Text = "1";
                textBox6.Enabled = false;
                textBox4.Enabled = true;
            }
        }
    }
}

