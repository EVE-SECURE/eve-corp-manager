using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ECM
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }
        private void About_Load(object sender, EventArgs e)
        {
            LinkLabel.Link link = new LinkLabel.Link();
            link.LinkData = "https://code.google.com/p/eve-corp-manager/";
            linkLabel1.Links.Add(link);
            string Userimg = "http://image.eveonline.com/Character/90940287_256.jpg";
            pictureBox1.Load(Userimg);

            pictureBox1.Image.Save(Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\AppData\Local\EVECM\images\90940287_256.jpg"));
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData as string);  
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }                   
    }
}
