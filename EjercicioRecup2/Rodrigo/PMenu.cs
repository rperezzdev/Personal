using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rodrigo
{
    public partial class PMenu : Form
    {
        public PMenu()
        {
            InitializeComponent();
        }

        private void productosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PProductos pProductos = new PProductos();
            pProductos.Show();
        }

        private void preciosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PPrecios pPrecios = new PPrecios();
            pPrecios.Show();
        }
    }
}
