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
    public partial class UpdateBOP: Form
    {
        public UpdateBOP()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            BOP bop = new BOP();
            bop.Show();
            this.Hide();
        }
    }
}
