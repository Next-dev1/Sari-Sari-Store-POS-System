using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Tindahan_ni_manong_jurelle
{
    public partial class AdminLogin: Form
    {
        public AdminLogin()
        {
            InitializeComponent();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScaleDimensions = new SizeF(96F, 96F);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.Trim().Equals("Admin") && textBox2.Text.Trim().Equals("admin01"))
            {
                Admins_Option op = new Admins_Option();
                op.Show();
                this.Hide();
            }
            else if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (!textBox1.Text.Equals("Admin") || !textBox2.Text.Equals("admin01"))
            {
                MessageBox.Show("Incorrect user credentials", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 mm = new Form1();
            mm.Show();
            this.Hide();
        }

        private void AdminLogin_Load(object sender, EventArgs e)
        {

        }
    }
}
