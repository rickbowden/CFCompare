using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CFComapre
{
    public partial class TextForm : Form
    {
        public TextForm()
        {
            InitializeComponent();

            try
            {
                textBox1.Text = Properties.Resources.About;
                textBox1.SelectionStart = 0;
            }
            catch (Exception ex)
            {
                textBox1.Text = ex.Message;
            }
        }
    }
}
