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
    public partial class AddTaxes : Form
    {
        static string connectionString = "server=.\\DESTAN;Database=RentACar;Integrated Security=True";
        SqlConnection connection = new SqlConnection(connectionString);
        private readonly TaxesInformations _taxes;
        string carid = "";
        DateTime date;
        public AddTaxes(TaxesInformations taxesInfo)
        {
            _taxes = taxesInfo;
            carid = _taxes.carid;
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

        private void button1_Click(object sender, EventArgs e)
        {
            string query = "INSERT INTO TAXES VALUES(@taxName,@carid,@date1,@date2,@amount);";
            try
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@taxName", textBox2.Text);
                command.Parameters.AddWithValue("@date1", DateTime.Today.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@date2", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@amount", textBox1.Text);
                command.Parameters.AddWithValue("@carid", carid);
                if (command.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Taxes added.");
                }
                else
                {
                    MessageBox.Show("Taxes cannot add.");
                }
            }catch(Exception error)
            {
                MessageBox.Show(error.Message);
            }
            
        }

        private void AddTaxes_Load(object sender, EventArgs e)
        {
            makeAconnection();
        }
    }
}
