using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
        string productNamePattern = @"^[a-zA-Z0-9\s\-\,\.]{1,50}$";
        int pCode;
        public DataTable dataTable = new DataTable();
        public BindingSource displayBindingSource = new BindingSource();
        public BindingSource updateBindingSource = new BindingSource();
        public BindingSource deleteBindingSource = new BindingSource();
        public static string text;

        public IM()
        {
            InitializeComponent();
            displayProducts.DataSource = displayBindingSource;
            DisplayList();

        }



        private void button2_Click(object sender, EventArgs e)
        {
            //back 
            Admins_Option op = new Admins_Option();
            op.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //log out
            AdminLogin login = new AdminLogin();
            login.Show();
            this.Hide();
        }


        private void IM_Load(object sender, EventArgs e)
        {
            DisplayList();
            refreshData();

        }
        
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            //useless
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            //add product 
            string productName = addProduct_txtBox.Text.Trim();
            string priceText = addPrice_txtBox.Text.Trim();
            string quantityText = addQuantity_txtBox.Text.Trim();

            //blank input
            if (string.IsNullOrWhiteSpace(priceText) || string.IsNullOrWhiteSpace(quantityText) || string.IsNullOrWhiteSpace(productName))
            {
                MessageBox.Show("Field(s) cannot be left blank.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //optional??
            if (!Regex.IsMatch(productName, productNamePattern))
            {
                MessageBox.Show("Product name must be valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(quantityText, out int quantityResult) && !decimal.TryParse(priceText, out decimal priceResult))
            {
                MessageBox.Show("Enter a valid quantity and price.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //price less than zero
            if (!decimal.TryParse(priceText, out  priceResult) || priceResult <= 0)
            {
                MessageBox.Show("Enter a valid price.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //negative quantity value
            if (!int.TryParse(quantityText, out  quantityResult) || quantityResult <= 0)
            {
                MessageBox.Show("Enter a valid quantity.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                sqlconnection.Open();
                SqlCommand cmd = new SqlCommand("AddProduct", sqlconnection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@_ProductName", productName);
                cmd.Parameters.AddWithValue("@_Price", priceResult);
                cmd.Parameters.AddWithValue("@_Quantity", quantityResult);


                cmd.ExecuteNonQuery();
                sqlconnection.Close();
                MessageBox.Show("Item added.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DisplayList();
            }
            catch (SqlException sex)
            {
                MessageBox.Show("An error occurred: " + sex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            catch (StackOverflowException sofex)
            {
                MessageBox.Show("An error occurred: " + sofex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (OutOfMemoryException oomex)
            {
                MessageBox.Show("An error occurred: " + oomex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (AccessViolationException avex)
            {
                MessageBox.Show("An error occurred: " + avex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (ThreadAbortException taex)
            {
                MessageBox.Show("An error occurred: " + taex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                if (sqlconnection.State == ConnectionState.Open)
                {
                    sqlconnection.Close();
                }
            }

        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            //update product

            int productID = Convert.ToInt32(updatePCode_cmbBox.SelectedValue);
            string productName = updatePName_cmbBox.Text.Trim();
            string price = updatePrice_cmbBox.Text.Trim();
            string quantity = updateQuantity_cmbBox.Text.Trim();

            if (updatePCode_cmbBox.SelectedValue == null)
            {
                MessageBox.Show("Select a product code to update an item.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //blank input
            if (updatePCode_cmbBox.SelectedValue == null || string.IsNullOrWhiteSpace(price) || string.IsNullOrWhiteSpace(productName) || string.IsNullOrWhiteSpace(price))
            {
                MessageBox.Show("Field(s) cannot be left blank.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            //invalid product name
            if (!Regex.IsMatch(productName, productNamePattern))
            {
                MessageBox.Show("Product name must be valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(quantity, out int quantityResult) && !decimal.TryParse(price, out decimal priceResult))
            {
                MessageBox.Show("Enter a valid quantity and price (zero or greater).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //price less than zero
            if (!decimal.TryParse(price, out priceResult) || priceResult <= 0)
            {
                MessageBox.Show("Enter a valid price (greater than zero).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //negative quantity value
            if (!int.TryParse(quantity, out quantityResult) || quantityResult <= 0)
            {
                MessageBox.Show("Enter a valid quantity (zero or greater).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try{
                sqlconnection.Open();

                SqlCommand cmd = new SqlCommand("UpdateProduct", sqlconnection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@_ProductID", productID);
                cmd.Parameters.AddWithValue("@_ProductName", productName);
                cmd.Parameters.AddWithValue("@_Price", price);
                cmd.Parameters.AddWithValue("@_Quantity", quantity);

                cmd.ExecuteNonQuery();

                sqlconnection.Close();

                MessageBox.Show("Item updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DisplayList();
                displayBindingSource.ResetBindings(false);
            }
            catch (SqlException sex)
            {
                MessageBox.Show("An error occurred: " + sex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            catch (StackOverflowException sofex)
            {
                MessageBox.Show("An error occurred: " + sofex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (OutOfMemoryException oomex)
            {
                MessageBox.Show("An error occurred: " + oomex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (AccessViolationException avex)
            {
                MessageBox.Show("An error occurred: " + avex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (ThreadAbortException taex)
            {
                MessageBox.Show("An error occurred: " + taex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                if (sqlconnection.State == ConnectionState.Open)
                {
                    sqlconnection.Close();
                }
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            //delete product
            pCode = Convert.ToInt32(deletePCode_cmbBox.SelectedValue);

            //blank choice for product code
            if (deletePCode_cmbBox.SelectedValue == null)
            {
                MessageBox.Show("Select a product code to delete an item.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                sqlconnection.Open();
                SqlCommand cmd = new SqlCommand("DeleteProduct", sqlconnection);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@_ProductCode", pCode);

                cmd.ExecuteNonQuery();
                sqlconnection.Close();

                MessageBox.Show("Item deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DisplayList();
                displayBindingSource.ResetBindings(false);
            }
            catch (SqlException sex)
            {
                MessageBox.Show("An error occurred: " + sex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            catch (StackOverflowException sofex)
            {
                MessageBox.Show("An error occurred: " + sofex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (OutOfMemoryException oomex)
            {
                MessageBox.Show("An error occurred: " + oomex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (AccessViolationException avex)
            {
                MessageBox.Show("An error occurred: " + avex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (ThreadAbortException taex)
            {
                MessageBox.Show("An error occurred: " + taex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                if (sqlconnection.State == ConnectionState.Open)
                {
                    sqlconnection.Close();
                }
            }
        }

        private void updatePrice_cmbBox_TextChanged(object sender, EventArgs e)
        {
            //useless
        }
        public void DisplayList()
        {
            try
            {
                dataTable.Clear();
                string viewInventory = "SELECT ProductCode, ProductName, Price, Quantity , TotalPrice FROM inventoryTable";
                SqlDataAdapter sqldataadapter = new SqlDataAdapter(viewInventory, sqlconnection);

                sqldataadapter.Fill(dataTable);
                displayBindingSource.DataSource = dataTable;
                updateBindingSource.DataSource = dataTable;
                deleteBindingSource.DataSource = dataTable;

                displayBindingSource.ResetBindings(false);

            }
            catch (SqlException sex)
            {
                MessageBox.Show("An error occurred: " + sex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                if (sqlconnection.State == ConnectionState.Open)
                {
                    sqlconnection.Close();
                }
            }
        }
        public void refreshData()
        {
            //delete

            deletePCode_cmbBox.DataSource = deleteBindingSource;
            deletePCode_cmbBox.ValueMember = "ProductCode";
            deletePCode_cmbBox.DisplayMember = "ProductCode";
            deleteBindingSource.ResetBindings(false);

            //update

            updatePCode_cmbBox.DataSource = updateBindingSource;
            updatePCode_cmbBox.ValueMember = "ProductCode";
            updatePCode_cmbBox.DisplayMember = "ProductCode";
            updateBindingSource.ResetBindings(false);
        }
    }
}
