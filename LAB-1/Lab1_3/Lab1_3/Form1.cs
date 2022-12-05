using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;


namespace Lab1_3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\YASENOVIY"))
                {
                    Object o = key.GetValue("P5");
                    string[] valuesstring = o as string[];
                    string output = String.Join("\n", valuesstring);
                    MessageBox.Show(output);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                RegistryKey key;
                key = Registry.LocalMachine.OpenSubKey("Software\\YASENOVIY", true);
                string[] write_str = { "Я - учасник", "кафедри", "комп'ютерної", "інженерії!" };
                key.SetValue("P6", write_str, RegistryValueKind.MultiString);
                key.Close();
                MessageBox.Show("успішно");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
 
}
