using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace EVE_corp_manager_V2
{
    public partial class membermanager : Form
    {
        
        apiArrays aa = new apiArrays();
        CorpApiArrays caa = new CorpApiArrays();
        CorpWalletapiArray cwaa = new CorpWalletapiArray();
        EVECM_common common = new EVECM_common();
        private string API = "0R3vi2JzPYjiDSmnxtjivFhaEfn0N2LflEYGZ6t2jAn6w1oFjBJZYRw9LxVMEZ3t";
        private int KEY = 940653;
        private int USER = 90981690;
        private string CORP_API = "0BWloK6rD8FzIxjm7YPe50Of2Kfv78C9vSu1K8o3tKI2nR06Cq1t6gHKA9KBCvCj";
        private int CORP_KEY = 2436985;
        private int CORP_USER = 98233354;
        public membermanager()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void membermanager_Load(object sender, EventArgs e)
        {
            doCMC();
            domemcount();
            doCharacterInfo();
            docharskills();
            Skillintraining();
            CharEmployment();
        }

        //PUll Corporation Member Count
        private void doCMC()
        {
            int _doCMC = caa.CorpSheet(CORP_KEY, CORP_API, CORP_USER).MemberCount;
            label1.Text = "Member Count: " + Convert.ToString(_doCMC);
        }

        //loads the mmember list from CorpApiArrays.cs
        //using the member count from doCMC to set "c"
        List<string> mems;
        private void domemcount()
        {
            int c = caa.CorpSheet(CORP_KEY, CORP_API, CORP_USER).MemberCount - 1;
            mems = caa.mems(CORP_KEY, CORP_API, USER);
            for (int i = 0; i <= c; i++)
            {
                listBox1.Items.Add(mems[i]);
            }
            int C = caa.CorpSheet(CORP_KEY, CORP_API, USER).MemberLimit;
            

        }
        //Character Sheet info and Portrait
        private void doCharacterInfo()
        {
            label2.Text = aa.CSheet(API, KEY, USER).Name;
            pictureBox1.Load("http://image.eveonline.com/Character/90981690_256.jpg");
            string clklsys = aa.CInfo(API, KEY, USER).LastKnownLocation;
            string clklship = aa.CInfo(API, KEY, USER).ShipTypeName;
            string clklshipname = aa.CInfo(API, KEY, USER).ShipName;
            label7.Text = clklsys;
            label7.Text += " in a " + clklship + " [" + clklshipname + "]";
            
        }
        private void CharEmployment()
        {
            foreach (var emp in aa.CharEmployment(API, KEY, USER))
            {
                listBox2.Items.Add(emp);
            }
        }
        //Pull Character Skills info
        private void docharskills()
        {
            foreach (var skill in aa.Cskill(API, KEY, USER).Skills)
            {
                var nam = Regex.Replace(skill.ToString(), @"[\d-]", string.Empty);
                var num = Regex.Replace(skill.ToString(), @"[^\d]", string.Empty);
                ListViewItem item = new ListViewItem
                (new[] 
                   { 
	                  nam, num 
	               }
                );
                listView1.Items.Insert(0, item);
            }
        }

        //Skill info
        private void Skillintraining()
        {
            label3.Text = "Training " + Convert.ToString(aa.skillintraining(API, KEY, USER).Skill.Name) + " to level " + Convert.ToString(aa.skillintraining(API, KEY, USER).TrainingToLevel);
            label6.Text = aa.CSheet(API, KEY, USER).CloneName;
            int _SPT = aa.CSheet(API, KEY, USER).SkillpointTotal;
            label5.Text = "Total SP: " + Convert.ToString(string.Format("{0:n0}", _SPT));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime Skillendtime = aa.skillintraining(API, KEY, USER).TrainingEndLocalTime;
            DateTime now = DateTime.Now;
            double _skilltimeleft = (Skillendtime.Subtract(now).TotalSeconds);
            label4.Text = "";
            if (_skilltimeleft >= 86400)
            {
                double _skilldays = (_skilltimeleft / 86400);
                label4.Text = Convert.ToString(Math.Floor(_skilldays)) + " d, ";
            }
            double _dayremainder = (_skilltimeleft % 86400);
            if (_dayremainder > 3600)
            {
                double _skillhours = (_dayremainder / 3600);
                label4.Text += Convert.ToString(Math.Floor(_skillhours)) + " h, ";
            }
            double _hourremainder = (_dayremainder % 3600);
            if (_hourremainder > 60)
            {
                double _skillminutes = (_hourremainder / 60);
                label4.Text += Convert.ToString(Math.Floor(_skillminutes)) + " m, ";
            }
            double _minuteremainder = (_hourremainder % 60);
            if (_minuteremainder > 1)
            {
                double _skillseconds = _minuteremainder;
                label4.Text += Convert.ToString(Math.Floor(_skillseconds)) + " s ";
            }
        }

       
    }
}
