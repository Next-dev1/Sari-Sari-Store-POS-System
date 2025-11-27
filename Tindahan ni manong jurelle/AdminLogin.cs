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
            string username = textBox1.Text;
            string password = textBox2.Text;

            if(username.Equals("Admin") && password.Equals("admin01"))
            {
                Admins_Option op = new Admins_Option();
                op.Show();
                this.Hide();
            }
            else if(username != "Admin" )
            {
                label1.Text = "Unknown username...";
                label1.ForeColor = Color.Red;
            }
            else
            {
                label1.Text = "Wrong Password...";
                label1.ForeColor = Color.Red;
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
