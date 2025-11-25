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
    public partial class UpdateIM: Form
    {
        public UpdateIM()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            IM im = new IM();
            im.Show();
            this.Hide();
        }
    }
}
