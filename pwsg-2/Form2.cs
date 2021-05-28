using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pwsg_2
{
    public partial class Form2 : Form
    {
        public int Szerokosc, Wysokosc;

        private void button1_Click(object sender, EventArgs e)
        {
            Szerokosc = (int) szerokosc.Value;
            Wysokosc = (int)wysokosc.Value;

        }

        public Form2()
        {
            InitializeComponent();
        }

    }
}
