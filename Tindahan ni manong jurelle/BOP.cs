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
        public IM im = new IM();
        int Pcode;
        public SqlConnection sqlconnection = new SqlConnection(@"
             Data Source=(LocalDB)\MSSQLLocalDB;
             AttachDbFilename=""C:\Users\Seanv\source\repos\Tindahan ni manong jurelle\Tindahan ni manong jurelle\Inventory.mdf"";
             Integrated Security=True");
        public DataTable dt = new DataTable();
        public BindingSource transacTableSource = new BindingSource();
        public BindingSource addTransacSource = new BindingSource();
        public BindingSource deleteTransacBindingSource = new BindingSource();



        public BOP()
        {
            InitializeComponent();
      
            DisplayList();
            DisplayTransacHistory();
            refreshData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Admins_Option op = new Admins_Option();
            op.Show();
            this.Hide();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            //useless
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            //useless
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AdminLogin login = new AdminLogin();
            login.Show();
            this.Hide();
        }

        private void BOP_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'transactionTable._transactionTable' table. You can move, or remove it, as needed.
            this.transactionTableTableAdapter.Fill(this.transactionTable._transactionTable);
            //add order invoice
            //DisplayList();
            //DisplayTransacHistory();
            

            //refreshData();

        }

        public void DisplayList()
        {
            /*  im.dataTable.Clear();
               string viewInventory = "SELECT ProductCode, ProductName, Price, Quantity , TotalPrice FROM inventoryTable";
               SqlDataAdapter sqldataadapter = new SqlDataAdapter(viewInventory, sqlconnection);

               sqldataadapter.Fill(im.dataTable);
               im.displayBindingSource.DataSource = im.dataTable;


               im.displayBindingSource.ResetBindings(false);*/
            im.dataTable.Clear();
            

            string viewInventory = "SELECT ProductCode, ProductName, Price, Quantity , TotalPrice FROM inventoryTable";
            SqlDataAdapter sqldataadapter = new SqlDataAdapter(viewInventory, sqlconnection);

            sqldataadapter.Fill(im.dataTable);
            im.displayBindingSource.DataSource = im.dataTable;
            

            im.displayBindingSource.ResetBindings(false);
            dataGridView3.DataSource = im.dataTable;


        }

        public void DisplayTransacHistory()
        {
            dt.Clear();   
            
            string viewInventory = "SELECT TransactionID, ProductCode, ProductName, Quantity, PricePerItem,TotalPrice, PaymentAmount AS Payment, ChangeAmount AS Change, Date FROM transactionTable";
            SqlDataAdapter sqldataadapter = new SqlDataAdapter(viewInventory, sqlconnection);

            sqldataadapter.Fill(dt);
            dataGridView2.DataSource = dt;
            deleteTransacBindingSource.DataSource = dt;

            transacTableSource.DataSource = dt;
            transacTableSource.ResetBindings(false);
            dataGridView2.DataSource = dt;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //add order

            Pcode = Convert.ToInt32(comboBox1.SelectedValue);

            sqlconnection.Open();
            SqlCommand cmd = new SqlCommand("BillingTransactionn", sqlconnection);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@_QuantityToDeduct", Convert.ToInt32(textBox1.Text));
            cmd.Parameters.AddWithValue("@_ProductCode", Pcode);
            cmd.Parameters.AddWithValue("@_Date", dateTimePicker1.Value);
            cmd.Parameters.AddWithValue("@_Payment", Convert.ToDecimal(textBox2.Text));

            cmd.ExecuteNonQuery();
            sqlconnection.Close();

            MessageBox.Show("Billing successful.");

            DisplayList();
            DisplayTransacHistory();

            deleteTransacBindingSource.ResetBindings(false);
            im.displayBindingSource.ResetBindings(false);

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            //useless
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // view 
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //delete transaction history    
            int transacID = Convert.ToInt32(comboBox2.SelectedValue);

            //blank choice for product code
            if (comboBox2.SelectedValue == null)
            {
                MessageBox.Show("Select a product code to delete an item.");
                return;
            }

            sqlconnection.Open();
            SqlCommand cmd = new SqlCommand("DeleteTransaction", sqlconnection);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@_TransactionID", transacID);

            cmd.ExecuteNonQuery();
            sqlconnection.Close();
            MessageBox.Show("Transaction deleted.");

            DisplayList();
            DisplayTransacHistory();

            deleteTransacBindingSource.ResetBindings(false);
            im.displayBindingSource.ResetBindings(false);

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //useless
        }

        public void refreshData()
        {
            //delete
            comboBox2.DataSource = deleteTransacBindingSource;
            comboBox2.ValueMember = "TransactionID";
            comboBox2.DisplayMember = "TransactionID";
            deleteTransacBindingSource.ResetBindings(false);

            //add transaction order
            comboBox1.DataSource = im.displayBindingSource;
            comboBox1.ValueMember = "ProductCode";
            comboBox1.DisplayMember = "ProductCode";
            im.displayBindingSource.ResetBindings(false);

        }
    }
}
