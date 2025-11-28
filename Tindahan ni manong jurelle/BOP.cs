using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
        public DataTable dt1 = new DataTable();
        public BindingSource transacTableSource = new BindingSource();
        public BindingSource addTransacSource = new BindingSource();
        public BindingSource deleteTransacBindingSource = new BindingSource();
        public BindingSource receiptBindingSource = new BindingSource();
        public SqlCommand sCommand;
        public SqlDataAdapter sAdapter;



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
            this.transactionTableTableAdapter.Fill(this.transactionTable._transactionTable);
           
            
        }

        public void DisplayList()
        {
            

            try
            {
                im.dataTable.Clear();

                string viewInventory = "SELECT ProductCode, ProductName, Price, Quantity , TotalPrice FROM inventoryTable";
                SqlDataAdapter sqldataadapter = new SqlDataAdapter(viewInventory, sqlconnection);

                sqldataadapter.Fill(im.dataTable);
                im.displayBindingSource.DataSource = im.dataTable;


                im.displayBindingSource.ResetBindings(false);
                dataGridView3.DataSource = im.dataTable;

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

        public void DisplayTransacHistory()
        {
            

            try
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

        private void button1_Click(object sender, EventArgs e)
        {
            //add order

            //blank input
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Field(s) cannot be left blank.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(textBox1.Text, out int quantityResult) && !decimal.TryParse(textBox2.Text, out decimal priceResult))
            {
                MessageBox.Show("Enter a valid quantity and price.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //blank choice for product code
            if (comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Select a product code to delete an item.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //price less than zero
            if (!decimal.TryParse(textBox2.Text, out priceResult) || priceResult <= 0)
            {
                MessageBox.Show("Enter a valid price.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //negative quantity value
            if (!int.TryParse(textBox1.Text, out quantityResult) || quantityResult <= 0)
            {
                MessageBox.Show("Enter a valid quantity.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
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

                MessageBox.Show("Billing successful.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DisplayList();
                DisplayTransacHistory();

                deleteTransacBindingSource.ResetBindings(false);
                im.displayBindingSource.ResetBindings(false);
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            //useless
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //useless 
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //delete transaction history
            //
            int transacID = Convert.ToInt32(comboBox2.SelectedValue);

            //blank choice for product code
            if (comboBox2.SelectedValue == null)
            {
                MessageBox.Show("Select a product code to delete an item.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            try
            {
                sqlconnection.Open();
                SqlCommand cmd = new SqlCommand("DeleteTransaction", sqlconnection);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@_TransactionID", transacID);

                cmd.ExecuteNonQuery();
                sqlconnection.Close();
                MessageBox.Show("Transaction deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DisplayList();
                DisplayTransacHistory();

                deleteTransacBindingSource.ResetBindings(false);
                im.displayBindingSource.ResetBindings(false);

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

            //view receipt
            comboBox3.DataSource = deleteTransacBindingSource;
            comboBox3.ValueMember = "TransactionID";
            comboBox3.DisplayMember = "TransactionID";
            receiptBindingSource.ResetBindings(false);

        }
        public void ViewReceipt()
        {
            //view receipt
            try
            {
                dt1.Clear();
                sqlconnection.Open();
                int receiptID = Convert.ToInt32(comboBox3.SelectedValue);

                sCommand = new SqlCommand("SELECT * FROM transactionTable WHERE TransactionID = @TransactionID", sqlconnection);
                sAdapter = new SqlDataAdapter(sCommand);

                sAdapter.SelectCommand.Parameters.AddWithValue("@TransactionID", receiptID);

                sAdapter.Fill(dt1);

                DataRow row = dt1.Rows[0];


                StringBuilder receipt = new StringBuilder();

                receipt.AppendLine($"Product Name: {row["ProductName"]}");
                receipt.AppendLine($"Quantity: {row["Quantity"]}");

                receipt.AppendLine($"{row["ProductName"]}: {"₱"} {Convert.ToDecimal(row["PricePerItem"])}");
                receipt.AppendLine($"TOTAL PRICE: {"₱"} {Convert.ToDecimal(row["TotalPrice"])}");
                receipt.AppendLine("--------------------------");
                receipt.AppendLine($"Payment: {"₱"} {Convert.ToDecimal(row["PaymentAmount"])}");
                receipt.AppendLine($"Change: {"₱"} {Convert.ToDecimal(row["ChangeAmount"])}");
                receipt.AppendLine($"Date: {Convert.ToDateTime(row["Date"]):MM/dd/yyyy}");
                receipt.AppendLine("--------------------------");


                MessageBox.Show(receipt.ToString(), $"Receipt for ID {receiptID}");

                receiptBindingSource.DataSource = dt1;
                sqlconnection.Close();

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

        private void button3_Click(object sender, EventArgs e)
        {
            //view receipt
            if (comboBox3.SelectedValue == null)
            {
                MessageBox.Show("Select a product code to delete an item.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ViewReceipt();

        }
    }
}
