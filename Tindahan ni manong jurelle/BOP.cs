using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Tindahan_ni_manong_jurelle
{
    public partial class BOP: Form
    {
        int Pcode;
        public SqlConnection sqlconnection = new SqlConnection(@"
             Data Source=(LocalDB)\MSSQLLocalDB;
             AttachDbFilename=""C:\Users\Seanv\source\repos\Tindahan ni manong jurelle\Tindahan ni manong jurelle\Inventory.mdf"";
             Integrated Security=True");
        //public BindingSource pNameBSource = new BindingSource();
        //public BindingSource priceSource = new BindingSource();
        IM im = new IM();


        public BOP()
        {
            InitializeComponent();
            dataGridView3.DataSource = im.displayBindingSource;
            DisplayList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Admins_Option op = new Admins_Option();
            op.Show();
            this.Hide();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            AdminLogin login = new AdminLogin();
            login.Show();
            this.Hide();
        }

        private void BOP_Load(object sender, EventArgs e)
        {
            //add order invoice
            comboBox1.DataSource = im.addBindingSource;
            comboBox1.ValueMember = "ProductCode";
            comboBox1.DisplayMember = "ProductCode";
            im.addBindingSource.ResetBindings(false);
        }

        public void DisplayList()
        {
            /*  im.dataTable.Clear();
               string viewInventory = "SELECT ProductCode, ProductName, Price, Quantity , TotalPrice FROM inventoryTable";
               SqlDataAdapter sqldataadapter = new SqlDataAdapter(viewInventory, sqlconnection);

               sqldataadapter.Fill(im.dataTable);
               im.displayBindingSource.DataSource = im.dataTable;


               im.displayBindingSource.ResetBindings(false);*/
            DataTable dt = new DataTable();
            string viewInventory = "SELECT ProductCode, ProductName, Price, Quantity, TotalPrice FROM inventoryTable";
            SqlDataAdapter sqldataadapter = new SqlDataAdapter(viewInventory, sqlconnection);

            sqldataadapter.Fill(dt);
            dataGridView3.DataSource = dt;

        }
      
        private void button1_Click(object sender, EventArgs e)
        {
            //add order

            //Pcode = Convert.ToInt32(comboBox1.SelectedValue);
            Pcode = 2;
            sqlconnection.Open();
            SqlCommand cmd = new SqlCommand("BillingTransaction", sqlconnection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@_Quantity", Convert.ToInt32(textBox1.Text));
            cmd.Parameters.AddWithValue("@_ProductCode", Pcode);
            cmd.ExecuteNonQuery();
            sqlconnection.Close();
            MessageBox.Show("Item subtracted.");

            DisplayList();

        }
    }
}
