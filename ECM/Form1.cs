using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.IO;
using EveAI.Live.Corporation;
using EveAI.Live.Character;
using EveAI.Live;

namespace ECM
{
    public partial class Form1 : Form
    {
        EVECM_common common = new EVECM_common();
        POSapiArrays paa = new POSapiArrays();
        Common.Database.dbconn dbconn = new Common.Database.dbconn();
        Common.Database.DBWorker worker = new Common.Database.DBWorker();
        Common.onload.onload onload = new Common.onload.onload();
        //private string API = "0R3vi2JzPYjiDSmnxtjivFhaEfn0N2LflEYGZ6t2jAn6w1oFjBJZYRw9LxVMEZ3t";
        //private int KEY = 940653;
        //private int USER = 90981690;
        //private string CORP_API = "0BWloK6rD8FzIxjm7YPe50Of2Kfv78C9vSu1K8o3tKI2nR06Cq1t6gHKA9KBCvCj";
        //private int CORP_KEY = 2436985;
        //private int CORP_USER = 98233354;

        //Rad's key for testing
        //key 6593
        //API K798Uim7xOOC0C8alv2sdmOBFwAMbsWBp19STyZIO545sBsyuedjRaPGCZBWPgHf

        //long ITEMID = 1012606556642;

        public Form1()
        {
            InitializeComponent();
            EveApiBase.DefaultConfiguration.BaseDirectory = Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\AppData\Local\EVECM\");
            doTranqCheck();
            onload.Directorybuilder();
            dbconn.CreateDB();
            dbconn.TableBuilder();
            worker.mainFormStartUpdater();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            if (worker.startupToken == 1)
            {
                initializeThis();
                toolStripStatusLabel2.Text = "Last update at: " + DateTime.Now;
            }
            else 
            {
                tabControl1.Visible = false;
                //MessageBox.Show("Go to File >>> API Key management to add a corporation and characters.");
            }
        }

        private void initializeThis()
        {
            listMembers();
            doWN();
        }

        #region Menus and Buttons

        /// <summary>
        /// Menu stuff for menus
        /// </summary>

        
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //about box
            About About = new About();
            About.Show();
        }

        private void purgeCacheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Loads the cache data
            cacheview Cacheview = new cacheview();
            Cacheview.Show();
        }

        private void importCharactersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //import a character from evemon
            importcharacters imchars = new importcharacters();
            imchars.Show();
        }

        private void memberManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //member management tool
            membermanager membertool = new membermanager();
            membertool.Show();
        }

        private void aPIKeyManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //API KEY launcher tool
            API_KEY_mgmt API_tool = new API_KEY_mgmt();
            API_tool.Show();

        }

        private void dBTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DBTester db = new DBTester();
            db.Show();
        }
        private void jobsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IndyJobs IJ = new IndyJobs();
            IJ.Show();
        }

       
        #endregion

        #region CORP Wallet 
        int CW;
        private void doMasterWallet()
        {
            CW = 1000;
            label11.Text = worker.returnWalletBalance(CW) + " ISK";
            popDGV1(CW);
            popDGV2(CW);
            radioButton1.Checked = true;
        }

        //Wallet buttons on second tab
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            CW = 1000;
            popDGV1(CW);
            popDGV2(CW);
            label11.Text = worker.returnWalletBalance(CW) + " ISK";
        }
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            CW = 1001;
            popDGV1(CW);
            popDGV2(CW);
            label11.Text = worker.returnWalletBalance(CW) + " ISK";
        }
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            CW = 1002;
            popDGV1(CW);
            popDGV2(CW);
            label11.Text = worker.returnWalletBalance(CW) + " ISK";
        }
        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            CW = 1003;
            popDGV1(CW);
            popDGV2(CW);
            label11.Text = worker.returnWalletBalance(CW) + " ISK";
        }
        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            CW = 1004;
            popDGV1(CW);
            popDGV2(CW);
            label11.Text = worker.returnWalletBalance(CW) + " ISK";
        }
        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            CW = 1005;
            popDGV1(CW);
            popDGV2(CW);
            label11.Text = worker.returnWalletBalance(CW) + " ISK";
        }
        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            CW = 1006;
            popDGV1(CW);
            popDGV2(CW);
            label11.Text = worker.returnWalletBalance(CW) + " ISK";
        }

        //sets wallet names
        private void doWN()
        {
            radioButton1.Text = worker.doWN().Item1;
            radioButton2.Text = worker.doWN().Item2;
            radioButton3.Text = worker.doWN().Item3;
            radioButton4.Text = worker.doWN().Item4;
            radioButton5.Text = worker.doWN().Item5;
            radioButton6.Text = worker.doWN().Item6;
            radioButton7.Text = worker.doWN().Item7;
        }

        private void popDGV1(int CW)
        {
            string ECMDBlocation = Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\AppData\Local\EVECM\db\EVECM.db");
            string CORP_USER = Convert.ToString(worker.CORP_USER);
            SQLiteConnection ECMDB = new SQLiteConnection("Data Source=" + ECMDBlocation);
            ECMDB.Open();
            string sql = "select * from Corp_" + CORP_USER + "_Wallet_Journal where Wallet_ID = " + CW;
            SQLiteCommand cmd = new SQLiteCommand(sql, ECMDB);
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            DataSet ds = new DataSet();
            try
            {
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                this.dataGridView1.DataSource = dt;
                dataGridView1.Columns["Amount"].DefaultCellStyle.Format = "c";
                dataGridView1.Columns["Balance"].DefaultCellStyle.Format = "c";
                dataGridView1.Columns[11].HeaderCell.SortGlyphDirection = SortOrder.Descending;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].Visible = false;
                dataGridView1.Columns[2].Visible = false;
                dataGridView1.Columns[5].Visible = false;
                dataGridView1.Columns[7].Visible = false;
                dataGridView1.Columns[9].Visible = false;
                dataGridView1.Columns[10].Visible = false;
                dataGridView1.Columns[12].Visible = false;
                dataGridView1.Columns[13].Visible = false;
                dataGridView1.Columns[14].Visible = false;
                dataGridView1.Columns[15].Visible = false;
                dataGridView1.Columns[16].Visible = false;
                dataGridView1.Columns[17].Visible = false;
                dataGridView1.Columns[18].Visible = false;
                dataGridView1.Columns[20].Visible = false;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AllowUserToDeleteRows = false;
                dataGridView1.AllowUserToOrderColumns = true;
                dataGridView1.ReadOnly = true;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.MultiSelect = false;
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(Convert.ToString(ex));
            }
            ECMDB.Close();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            int dg1Amount = 0, dg1Bal = 0;
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;
                int selectedrowindexAMT = dataGridView1.SelectedCells[3].RowIndex;
                int selectedrowindexBal = dataGridView1.SelectedCells[4].RowIndex;
                int selectedrowindexGivingParty = dataGridView1.SelectedCells[7].RowIndex;
                int selectedrowindexxferType = dataGridView1.SelectedCells[19].RowIndex;
                int selectedrowindexrecvdName = dataGridView1.SelectedCells[14].RowIndex;
                int selectedrowindexReason = dataGridView1.SelectedCells[13].RowIndex;
                DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];
                DataGridViewRow selectedRowAMT = dataGridView1.Rows[selectedrowindexAMT];
                DataGridViewRow selectedRowBal = dataGridView1.Rows[selectedrowindexBal];
                DataGridViewRow selectedRowGP = dataGridView1.Rows[selectedrowindexGivingParty];
                DataGridViewRow selectedRowXferType = dataGridView1.Rows[selectedrowindexxferType];
                DataGridViewRow selectedRowrecvdName = dataGridView1.Rows[selectedrowindexrecvdName];
                DataGridViewRow selectedRowReason = dataGridView1.Rows[selectedrowindexReason];
                label36.Text = "ID: " +  Convert.ToString(selectedRow.Cells["JournalID"].Value);
                dg1Amount = Convert.ToInt32(selectedRowAMT.Cells["Amount"].Value);
                dg1Bal = Convert.ToInt32(selectedRowBal.Cells["Balance"].Value);
                label39.Text = "Payor: " + Convert.ToString(selectedRowGP.Cells["GivingPartyName"].Value);
                label35.Text = "Transfer: " + Convert.ToString(selectedRowXferType.Cells["xferType"].Value);
                label32.Text = "Payee: " + Convert.ToString(selectedRowrecvdName.Cells["RecvdPartyName"].Value);
                label18.Text = Convert.ToString(selectedRowReason.Cells["Reason"].Value);
            }
            groupBox2.Text = "Journal Entry Detail";
            label37.Text = "Price: " + string.Format("{0:c}", dg1Amount);
            label38.Text = "Balance: " + string.Format("{0:c}", dg1Bal);

            
        }

        private void popDGV2(int CW)
        {
            string ECMDBlocation = Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\AppData\Local\EVECM\db\EVECM.db");
            string CORP_USER = Convert.ToString(worker.CORP_USER);
            SQLiteConnection ECMDB = new SQLiteConnection("Data Source=" + ECMDBlocation);
            ECMDB.Open();
            string sql = "select * from Corp_" + CORP_USER + "_Wallet_transactions where Wallet_ID = " + CW;
            SQLiteCommand cmd = new SQLiteCommand(sql, ECMDB);
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            DataSet ds = new DataSet();
            try
            {
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                this.dataGridView2.DataSource = dt;
                dataGridView2.Columns["Price"].DefaultCellStyle.Format = "c";
                dataGridView2.Columns["PriceTotal"].DefaultCellStyle.Format = "c";
                dataGridView2.Columns[6].HeaderCell.SortGlyphDirection = SortOrder.Descending;
                dataGridView2.Columns[0].Visible = false;
                dataGridView2.Columns[1].Visible = false;
                dataGridView2.Columns[3].Visible = false;
                dataGridView2.Columns[4].Visible = false;
                dataGridView2.Columns[5].Visible = false;
                dataGridView2.Columns[11].Visible = false;
                dataGridView2.Columns[12].Visible = false;
                dataGridView2.Columns[13].Visible = false;
                dataGridView2.Columns[17].Visible = false;
                dataGridView2.Columns[18].Visible = false;
                dataGridView2.AllowUserToAddRows = false;
                dataGridView2.AllowUserToDeleteRows = false;
                dataGridView2.AllowUserToOrderColumns = true;
                dataGridView2.ReadOnly = true;
                dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView2.MultiSelect = false;
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(Convert.ToString(ex));
            }
            ECMDB.Close();
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            string ID = "", ProductType = ""; // Type = "";
            int price = 0, pricetotal = 0, Quantity = 0;
            if (dataGridView2.SelectedCells.Count > 0)
            {
                int selectedrowindexID = dataGridView2.SelectedCells[0].RowIndex;
                int selectedRowIndexPrice = dataGridView2.SelectedCells[0].RowIndex;
                int selectedRowIndexPricetotal = dataGridView2.SelectedCells[0].RowIndex;
                int selectedRowIndexQuantity = dataGridView2.SelectedCells[0].RowIndex;
                int selectedRowIndexProductType = dataGridView2.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRowID = dataGridView2.Rows[selectedrowindexID];
                DataGridViewRow selectedRowPrice = dataGridView2.Rows[selectedRowIndexPrice];
                DataGridViewRow selectedRowPriceTotal = dataGridView2.Rows[selectedRowIndexPrice];
                DataGridViewRow selectedRowQuantity = dataGridView2.Rows[selectedRowIndexPrice];
                DataGridViewRow selectedRowProductType = dataGridView2.Rows[selectedRowIndexProductType];
                ID = Convert.ToString(selectedRowID.Cells["TransactionID"].Value);
                price = Convert.ToInt32(selectedRowPrice.Cells["Price"].Value);
                pricetotal = Convert.ToInt32(selectedRowPriceTotal.Cells["PriceTotal"].Value);
                Quantity = Convert.ToInt32(selectedRowQuantity.Cells["Quantity"].Value);
                ProductType = Convert.ToString(selectedRowProductType.Cells["ProductType"].Value);

            }
            groupBox2.Text = "Transaction Entry Detail";
            label36.Text = "ID: " + ID;
            label37.Text = "Price: " + string.Format("{0:c}", price);
            label38.Text = "Total Price: " + string.Format("{0:c}", pricetotal);
            label39.Text = "Quantity: " + string.Format("{0:n0}", Quantity);
            label35.Text = "Product: " + ProductType;
        }
        

        #endregion

        #region Character Info
        
        //loads selected character from list
        DateTime TrainingEndLocalTime;
        DateTime TELT;
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string ECM_cache = Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\AppData\Local\EVECM\");
                timer1.Enabled = false;
                string selectChar = Convert.ToString(listBox1.SelectedItem).Trim();
                string[] charData = worker.returnCharData(selectChar);
                label3.Text = charData[1]; //name
                label4.Text = "Training " + charData[2] + " to level " + charData[3]; //Skill in training and skill level
                label17.Text = charData[6]; //Clonegrade
                label6.Text = "Total SP: " + charData[5]; //Total Skill points
                label30.Text = charData[7] + " in a " + charData[8] + " [" + charData[9] + "]"; //Last location, shiptype, ship name
                TrainingEndLocalTime = Convert.ToDateTime(dbconn.datetimestandard(TELT));
                TELT = Convert.ToDateTime(charData[4]); //training end local time
                TrainingEndLocalTime = Convert.ToDateTime(dbconn.datetimestandard(TELT));
                try
                {
                    pictureBox3.Load(ECM_cache + @"images\" + charData[0] + "_256.jpg"); //Char ID
                }
                catch
                {
                    pictureBox3.Load("http://image.eveonline.com/Character/" + charData[0] + "_256.jpg"); //Char ID
                    pictureBox3.Image.Save(ECM_cache + @"images\" + charData[0] + "_256.jpg");  //Char ID
                }
                timer1.Enabled = true;
                docpn(selectChar);
            }
            catch { }
        }


        /*
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
        */


       

        #endregion

        #region Corporation Info

        
        int isRunning = 0;
        //PUll Corporation Member Count
        public void listMembers()
        {
            if (tabControl1.Visible == false)
            {
                tabControl1.Visible = true;
            }
            
            if (isRunning == 0)
            {
                try
                {
                    listBox1.Items.Clear();
                    label29.Text = "";
                    label31.Text = "";
                    foreach (var line in worker.listMems())
                    {
                        listBox1.Items.Add(line);
                    }
                    label29.Text = "Member Limit: " + Convert.ToString(worker.doCMC().Item1);
                    label31.Text = "Member Count: " + Convert.ToString(worker.doCMC().Item2);
                    listBox1.SelectedIndex = 0;
                    isRunning = 1;
                    doMasterWallet();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Convert.ToString(ex));
                }
            }
            else
            {
                try
                {
                    int lBS = listBox1.SelectedIndex;
                    listBox1.Items.Clear();
                    label29.Text = "";
                    label31.Text = "";
                    foreach (var line in worker.listMems())
                    {
                        listBox1.Items.Add(line);
                    }
                    label29.Text = "Member Limit: " + Convert.ToString(worker.doCMC().Item1);
                    label31.Text = "Member Count: " + Convert.ToString(worker.doCMC().Item2);
                    listBox1.SelectedIndex = lBS;
                }
                catch (Exception ex)
                {
                     MessageBox.Show(Convert.ToString(ex));
                }
            }
        }

        //Character Corporation and Alliance Name
        private void docpn(string selectedChar)
        {
            string ECM_cache = Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\AppData\Local\EVECM\");
            string[] corpData = worker.doCPN(selectedChar);
            label1.Text = corpData[1]; 
            label2.Text = corpData[2];
            try
            {
                pictureBox2.Load(ECM_cache + @"images\" + corpData[0] + "_256.jpg");
            }
            catch
            {
                pictureBox2.Load("http://image.eveonline.com/Corporation/" + corpData[0] + "_256.png");
                pictureBox2.Image.Save(ECM_cache + @"images\" + corpData[0] + "_256.jpg");
            }
        }

        #endregion

        #region POS detail

        private void button2_Click(object sender, EventArgs e)
        {
            //Starbase();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string f = "";
            try
            {
                //string ftemp = Convert.ToString(paa.CorpStarbase(CORP_KEY, CORP_API, ITEMID).Fuel);
                //EveApi api = new EveApi(KEY, CORP_API, ITEMID);
                //Starbase _SBS = api.GetCorporationStarbaseDetail().Fuel;
                //f = Convert.ToString( _SBS);

            }
            catch (Exception crap)
            {
                label21.Text = Convert.ToString(crap);
            }
            label21.Text = f;
        }
        /*
        public void Starbase()
        {

            foreach (var s in paa.CorpStarBaseList(CORP_KEY, CORP_API))
            {
                listBox5.Items.Add(s);
                string starbase = Convert.ToString(s);
            }
            foreach (var s in paa.CorpStarBaseList(CORP_KEY, CORP_API))
            {
                string type = Convert.ToString(s.Type);
                label22.Text = type;
            }
            foreach (var s in paa.CorpStarBaseList(CORP_KEY, CORP_API))
            {
                string state = Convert.ToString(s.State);
                label19.Text = state;
            }
            foreach (var s in paa.CorpStarBaseList(CORP_KEY, CORP_API))
            {
                string activedate = Convert.ToString(s.StateActivateDate);
            }
            foreach (var s in paa.CorpStarBaseList(CORP_KEY, CORP_API))
            {
                string onlinedate = Convert.ToString(s.OnlinedDate);
            }
            foreach (var s in paa.CorpStarBaseList(CORP_KEY, CORP_API))
            {
                //listBox5.Items.Add(s.Moon.Name);
            }
            foreach (var s in paa.CorpStarBaseList(CORP_KEY, CORP_API))
            {
                string itemID = Convert.ToString(s.ItemID);
                long ITEMID = s.ItemID;
                label20.Text = itemID;
            }

        }
        */
        #endregion

        #region Timers

        private void timer1_Tick(object sender, EventArgs e)
        {
            skillTimer();
            updater();
        }

        //Skill time to completion
        private void skillTimer()
        {
            DateTime Skillendtime = TrainingEndLocalTime;
            DateTime now = DateTime.Now;
            double _skilltimeleft = (Skillendtime.Subtract(now).TotalSeconds);
            label5.Text = "";
            if (_skilltimeleft >= 86400)
            {
                double _skilldays = (_skilltimeleft / 86400);
                label5.Text = Convert.ToString(Math.Floor(_skilldays)) + " d, ";
            }
            double _dayremainder = (_skilltimeleft % 86400);
            if (_dayremainder > 3600)
            {
                double _skillhours = (_dayremainder / 3600);
                label5.Text += Convert.ToString(Math.Floor(_skillhours)) + " h, ";
            }
            double _hourremainder = (_dayremainder % 3600);
            if (_hourremainder > 60)
            {
                double _skillminutes = (_hourremainder / 60);
                label5.Text += Convert.ToString(Math.Floor(_skillminutes)) + " m, ";
            }
            double _minuteremainder = (_hourremainder % 60);
            if (_minuteremainder > 1)
            {
                double _skillseconds = _minuteremainder;
                label5.Text += Convert.ToString(Math.Floor(_skillseconds)) + " s ";
            }
        }

        int i = 895;
        private void updater()
        {
            if (i < 900)
            {
                label34.Text = Convert.ToString(i);
                i++;
            }
            else
            {
                worker.mainFormStartUpdater();
                doWN();
                toolStripStatusLabel2.Text = "Last update at: " + DateTime.Now;
                doTranqCheck();
                listMembers();
                i = 0;
            }
        }

        #endregion

        #region Tranquility Server


        private void doTranqCheck()
        {
            var isOnline = common.ServStatus().Online;
            if (isOnline == true)
            {
                
                toolStripStatusLabel1.ForeColor = Color.Black;
                toolStripStatusLabel1.Text = "Tranquility is Online, ";
                toolStripStatusLabel1.Text += Convert.ToString(string.Format("{0:n0}", (common.ServStatus().PlayersOnline))) + " Players in game";
            }
            else
            {
                toolStripStatusLabel1.ForeColor = Color.Red;
                toolStripStatusLabel1.Text = "Tranquility is Offline.";
            }
        }
            
        #endregion

        

        

      

        #region Test region

        /*
         * The Code listed here is either not working yet or in testing ****
         * The Code listed here is either not working yet or in testing ****
         * The Code listed here is either not working yet or in testing ****
        */





        #endregion

       
    }
}
