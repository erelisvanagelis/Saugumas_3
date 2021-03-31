using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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

        private void readButton_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog opd = new OpenFileDialog();
                opd.ShowDialog();
                if (opd.FileName == null)
                    return;

                string[] data = RSATool.ReadFile(opd.FileName);
                if (data == null || data.Length < 3)
                {
                    throw new Exception("Invalid file structure");
                }

                yTextBox.Text = "";
                nTextBox.Text = data[0];
                for (int i = 2; i < data.Length; i++)
                {
                    yTextBox.Text += data[i];

                    if (i != data.Length - 1)
                    {
                        yTextBox.Text += ';';
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void encryptButton_Click(object sender, EventArgs e)
        {
            Task task = Task.Run(() =>
            {
                try
                {
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        xTextBox.Text = RSATool.EncryptSequence(qTextBox.Text, pTextBox.Text, yTextBox.Text);
                    });
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
            });
        }

        private void decryptButton_Click(object sender, EventArgs e)
        {
            Task task = Task.Run(() =>
            {
                try
                {
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        xTextBox.Text = RSATool.DecryptSequence(nTextBox.Text, yTextBox.Text);
                    });
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
            });
        }

        private void writeButton_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.ShowDialog();

                if (sfd.FileName == null)
                    return;

                RSATool.WriteToAFile(sfd.FileName, xTextBox.Text);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void readRegularButton_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog opd = new OpenFileDialog();
                opd.ShowDialog();

                yTextBox.Text = File.ReadAllText(opd.FileName);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
    }
}
