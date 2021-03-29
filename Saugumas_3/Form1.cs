using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Saugumas_3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void calcNButton_Click(object sender, EventArgs e)
        {
            try
            {
                int q = Convert.ToInt32(qTextBox.Text);
                if (!RSATool.IsItPrimary(q))
                    throw new Exception("q: is not a primary number");

                int p = Convert.ToInt32(pTextBox.Text);
                if (!RSATool.IsItPrimary(p))
                    throw new Exception("p: is not a primary number");

                nTextBox.Text = (q * p).ToString();
            }
            catch(Exception exc)
            {
                MessageBox.Show(exc.Message);
            }


        }

        private void readButton_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog opd = new OpenFileDialog();
                opd.ShowDialog();
                if (opd.FileName == null)
                    return;

                string[] data = RSATool.ReadFile(opd.FileName);
                if(data == null)
                {
                    throw new Exception("Invalid file structure");
                }

                nTextBox.Text = data[0];
                yTextBox.Text = data[2];
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
    }
}
