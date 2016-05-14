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
    public partial class TaxesInformations : Form
    {
        static string connectionString = "server=.\\DESTAN;Database=RentACar;Integrated Security=True";
        SqlConnection connection = new SqlConnection(connectionString);
        public string carid = "";
        private TaxesInformations taxesInformations;

        public TaxesInformations()
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
            string query = "SELECT Car_id,Brand,Model,P_Year,Plate,Fuel,GearBox,Price FROM CARS;";
            try
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                dataGridView1.DataSource = dataSet.Tables[0];
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }

        }

        private void TaxesInformations_Load(object sender, EventArgs e)
        {
            makeAconnection();
            fillTheDataGrid();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            carid = dataGridView1.Rows[e.RowIndex].Cells["Car_id"].Value.ToString();
            AddTaxes addTaxes = new AddTaxes(this);
            addTaxes.ShowDialog();
        }
    }
}
