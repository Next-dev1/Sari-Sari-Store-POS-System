using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Tindahan_ni_manong_jurelle
{
    public partial class IM: Form
    {
        public SqlConnection sqlconnection = new SqlConnection(@"
             Data Source=(LocalDB)\MSSQLLocalDB;
             AttachDbFilename=""C:\Users\Seanv\source\repos\Tindahan ni manong jurelle\Tindahan ni manong jurelle\Inventory.mdf"";
             Integrated Security=True");
        int pCode;
        public DataTable dataTable = new DataTable();
        public BindingSource displayBindingSource = new BindingSource();
        public BindingSource updateBindingSource = new BindingSource();
        public BindingSource addBindingSource = new BindingSource();



        public static string text;

        public IM()
        {
            InitializeComponent();
            displayProducts.DataSource = displayBindingSource;
            DisplayList();

        }



        private void button2_Click(object sender, EventArgs e)
        {
            Admins_Option op = new Admins_Option();
            op.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AdminLogin login = new AdminLogin();
            login.Show();
            this.Hide();
        }


        private void IM_Load(object sender, EventArgs e)
        {
            DisplayList();
            //delete

            deletePCode_cmbBox.DataSource = addBindingSource;
            deletePCode_cmbBox.ValueMember = "ProductCode";
            deletePCode_cmbBox.DisplayMember = "ProductCode";
            addBindingSource.ResetBindings(false);

            //update

            updatePCode_cmbBox.DataSource = updateBindingSource;
            updatePCode_cmbBox.ValueMember = "ProductCode";
            updatePCode_cmbBox.DisplayMember = "ProductCode";
            updateBindingSource.ResetBindings(false);
        }
        
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void addButton_Click(object sender, EventArgs e)
        {
            //add
            string productNamePattern = @"^[a-zA-Z0-9\s\-\,\.]{1,50}$";

            string productName = addProduct_txtBox.Text.Trim();
            string priceText = addPrice_txtBox.Text.Trim();
            string quantityText = addQuantity_txtBox.Text.Trim();

            //blank input
            if (string.IsNullOrWhiteSpace(priceText) || string.IsNullOrWhiteSpace(quantityText) || string.IsNullOrWhiteSpace(productName))
            {
                MessageBox.Show("Field(s) cannot be left blank.");
                return;
            }

            //optional??
            if (!Regex.IsMatch(productName, productNamePattern))
            {
                MessageBox.Show("Product name must be valid characters.");
                return;
            }

            if (!int.TryParse(quantityText, out int quantityResult) && !decimal.TryParse(priceText, out decimal priceResult))
            {
                MessageBox.Show("Enter a valid quantity and price (zero or greater).");
                return;
            }

            //price less than zero
            if (!decimal.TryParse(priceText, out  priceResult) || priceResult <= 0)
            {
                MessageBox.Show("Enter a valid price (greater than zero).");
                return;
            }

            //negative quantity value
            if (!int.TryParse(quantityText, out  quantityResult) || quantityResult <= 0)
            {
                MessageBox.Show("Enter a valid quantity (zero or greater).");
                return;
            }

            

            sqlconnection.Open();
            SqlCommand cmd = new SqlCommand("AddProduct", sqlconnection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@_ProductName", productName);
            cmd.Parameters.AddWithValue("@_Price", priceResult);
            cmd.Parameters.AddWithValue("@_Quantity", quantityResult);
            

            cmd.ExecuteNonQuery();
            sqlconnection.Close();
            MessageBox.Show("Item added.");

            DisplayList();
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            //update
            int productID = Convert.ToInt32(updatePCode_cmbBox.SelectedValue);
            string productName = updatePName_cmbBox.Text;
            decimal price = decimal.Parse(updatePrice_cmbBox.Text);


            int quantity = int.Parse(updateQuantity_cmbBox.Text);

            //blank choice for product code
            if (updatePCode_cmbBox.SelectedValue == null)
            {
                MessageBox.Show("Select a product code to update an item.");
                return;
            }

            sqlconnection.Open();
            SqlCommand cmd = new SqlCommand("UpdateProduct", sqlconnection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@_ProductID", productID);
            cmd.Parameters.AddWithValue("@_ProductName", productName);
            cmd.Parameters.AddWithValue("@_Price", price);
            cmd.Parameters.AddWithValue("@_Quantity", quantity);

            cmd.ExecuteNonQuery();
            sqlconnection.Close();

            MessageBox.Show("Item updated.");
            DisplayList();
            //displayBindingSource.ResetBindings(false);
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            //delete
            pCode = Convert.ToInt32(deletePCode_cmbBox.SelectedValue);

            //blank choice for product code
            if (deletePCode_cmbBox.SelectedValue == null)
            {
                MessageBox.Show("Select a product code to delete an item.");
                return;
            }

            sqlconnection.Open();
            SqlCommand cmd = new SqlCommand("DeleteProduct", sqlconnection);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@_ProductCode", pCode);
            cmd.Parameters.AddWithValue("", pCode);

            cmd.ExecuteNonQuery();
            sqlconnection.Close();
            MessageBox.Show("Item deleted.");

            DisplayList();
            //displayBindingSource.ResetBindings(false);
        }

        private void updatePrice_cmbBox_TextChanged(object sender, EventArgs e)
        {

        }
        public void DisplayList()
        {
            /*dataTable.Clear();
            string viewInventory = "SELECT ProductCode, ProductName, Price, Quantity , TotalPrice FROM inventoryTable";
            SqlDataAdapter sqldataadapter = new SqlDataAdapter(viewInventory, sqlconnection);
            
            sqldataadapter.Fill(dataTable);
            displayBindingSource.DataSource = dataTable;
            updateBindingSource.DataSource = dataTable;
            addBindingSource.DataSource = dataTable;

            displayBindingSource.ResetBindings(false);
            updateBindingSource.ResetBindings(false);
            addBindingSource.ResetBindings(false);*/

            DataTable dt = new DataTable();

            string query = "SELECT ProductCode, ProductName, Price, Quantity , TotalPrice FROM inventoryTable";
            SqlDataAdapter da = new SqlDataAdapter(query, sqlconnection);

            da.Fill(dt);

            displayBindingSource.DataSource = dt;
            updateBindingSource.DataSource = dt;
            addBindingSource.DataSource = dt;
        }
    }
}
