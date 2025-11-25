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
    public partial class Admins_Option: Form
    {
        public Admins_Option()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 mm = new Form1();
            mm.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BOP bop = new BOP();    
            bop.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SRR srr = new SRR();
            srr.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            IM im = new IM();
            im.Show();
            this.Hide();
        }
    }
}
