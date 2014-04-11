using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace EVE_corp_manager_V2
{
    public partial class cacheview : Form
    {
        string cachelist;
        public cacheview()
        {
            InitializeComponent();
            cachelist = Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\AppData\Local\EVECM\cache");
            label2.Text = cachelist;
        }
        private void cacheview_Load(object sender, EventArgs e)
        {
            string[] files = Directory.GetFiles(cachelist);
            foreach (string i in files)
            {
                try
                {
                    label3.Text = Convert.ToString(files.Count()) + " files";
                    listBox1.Items.Add(Path.GetFileName(i));
                }
                catch (Exception) { }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                listBox1.Items.Clear();
                //label3.Text = 0 + " files";
                string[] purge = Directory.GetFiles(cachelist);
                foreach (string i in purge)
                {
                    File.Delete(i);
                }
                string[] files = Directory.GetFiles(cachelist);
                label3.Text = Convert.ToString(files.Count()) + " files";
            }
            catch (IOException)
            {
                Color color = Color.Red;
                label3.Text = "Something broke, please retry";
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            string[] files = Directory.GetFiles(cachelist);
            label3.Text = Convert.ToString(files.Count()) + " files";
            foreach (string i in files)
            {
                listBox1.Items.Add(Path.GetFileName(i));
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(cachelist);
            }
            catch (Exception) { }
        }
    }
}
