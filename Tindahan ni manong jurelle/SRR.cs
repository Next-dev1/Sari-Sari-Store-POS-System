using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tindahan_ni_manong_jurelle
{
    public partial class SRR: Form
    {
        public SqlConnection sqlconnection = new SqlConnection(@"
             Data Source=(LocalDB)\MSSQLLocalDB;
             AttachDbFilename=""C:\Users\Seanv\source\repos\Tindahan ni manong jurelle\Tindahan ni manong jurelle\Inventory.mdf"";
             Integrated Security=True");
        public SqlDataAdapter sda;
        public BindingSource bs;
        public DataTable dt;

        public SRR()
        {
            InitializeComponent();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Admins_Option op = new Admins_Option();
            op.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            AdminLogin login = new AdminLogin();
            login.Show();
            this.Hide();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            sqlconnection.Open();

            string getTotalRevenue = "Select SUM(TotalPrice) As [Total Revenue] from transactionTable";
            sda = new SqlDataAdapter(getTotalRevenue, sqlconnection);
            object totalRevenueResult = sda.SelectCommand.ExecuteScalar();

            label7.Text = "₱" + totalRevenueResult.ToString();

            string getTotalOrders = "Select COUNT(TransactionID) As [Total Orders] from transactionTable";
            sda = new SqlDataAdapter(getTotalOrders, sqlconnection);
            object totalOrdersResult = sda.SelectCommand.ExecuteScalar();

            label8.Text = totalOrdersResult.ToString();

            string getTotalProductsSold = "Select COUNT(TransactionID) As [Total Products Sold] from transactionTable";
            sda = new SqlDataAdapter(getTotalProductsSold, sqlconnection);
            object totalProductsSoldResult = sda.SelectCommand.ExecuteScalar();

            label10.Text = totalProductsSoldResult.ToString();

            sqlconnection.Close();



        }

        public void displayTopProducts()
        {   
            
            dt = new DataTable();
            dt.Clear();
            sda = new SqlDataAdapter("SELECT TOP 5 ProductName, SUM(Quantity) AS TotalSold FROM transactionTable GROUP BY ProductName ORDER BY TotalSold DESC;", sqlconnection);
            bs = new BindingSource();
            sda.Fill(dt);
            bs.DataSource = dt;
            dataGridView1.DataSource = bs;
            bs.ResetBindings(false);
        }
        public void displayProducts()
        {

            dt = new DataTable();
            bs = new BindingSource();
            dt.Clear();
            sda = new SqlDataAdapter("SELECT ProductCode, ProductName, Price FROM inventoryTable ORDER BY ProductCode ASC", sqlconnection);
            bs = new BindingSource();
            sda.Fill(dt);
            bs.DataSource = dt;
            dataGridView2.DataSource = bs;
            bs.ResetBindings(false);
        }

        private void SRR_Load(object sender, EventArgs e)
        {
            displayTopProducts();
            displayProducts();
        }
    }
}
