using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EveAI.Live.Account;
using EveAI.Live;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Data.SQLite;
using System.Text.RegularExpressions;
using EveAI.Live.Corporation;


namespace EVE_corp_manager_V2
{
    public partial class API_KEY_mgmt : Form
    {
        int KEY = 0, CORP_KEY = 0,USER = 0, CORP_USER = 0, NEXT = 0;
        string API = "",  CORP_API = "", nonexp = "1/1/0001 12:00:00 AM", Keyexp;
        bool result;
        SQLiteConnection ECMDB = new SQLiteConnection();
        string ECMDBlocation = Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\AppData\Local\EVECM\db\EVECM.db");
        Common.Database.dbconn dbconn = new Common.Database.dbconn();
        apiArrays aa = new apiArrays();
        CorpApiArrays caa = new CorpApiArrays();
        CorpWalletapiArray cwaa = new CorpWalletapiArray();
        Common.Database.DBWorker worker = new Common.Database.DBWorker();
        
        public API_KEY_mgmt()
        {
            InitializeComponent();
            LinkLabel.Link _APIsitelink = new LinkLabel.Link();
            _APIsitelink.LinkData = "https://community.eveonline.com/support/api-key";
            linkLabel1.Links.Add(_APIsitelink);
            listBox1.Visible = false;
            button4.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button3.Visible = false;
            int KEY = 0;
            string API = "";
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            try
            {
                if (textBox1.Text.Trim().Length != 0)
                {
                    bool Keytest = Int32.TryParse(textBox1.Text, out KEY);
                    if (Keytest)
                    {
                        if (textBox2.Text.Trim().Length != 0)
                        {
                            KEY = Convert.ToInt32(textBox1.Text);
                            API = textBox2.Text;
                            string keytype = aa.keyinfo(API, KEY);
                            if (keytype == "Account")
                            {
                                string keyType = "Account";
                                AccountKey(keyType);
                            }
                            else if (keytype == "Character")
                            {
                                string keyType = "Character";
                                CharacterKey(keyType);
                            }
                            else if (keytype == "Corporation")
                            {
                                string keyType = "Corporation";
                                CorporateKey(keyType);
                            }
                        }
                        else
                        {
                            label8.ForeColor = Color.Red;
                            label8.Text = "The API field cannot be blank";
                            button4.Visible = true;
                        }
                    }
                    else
                    {
                        label8.ForeColor = Color.Red;
                        label8.Text = "The Key field only accepts numbers";
                        button4.Visible = true;
                    }
                }
                else
                {
                    label8.ForeColor = Color.Red;
                    label8.Text = "The Key field cannot be blank";
                    button4.Visible = true;
                }

            }
            catch (Exception)
            {
                label8.ForeColor = Color.Red;
                label8.Text = "I dont acutally know what happened here";
            }
        }

        private void CharacterKey(string keyType)
        {
            if (NEXT == 0)
                {
                    try
                    {
                        NEXT++;
                        button2.Text = "Continue";
                        KEY = Convert.ToInt32(textBox1.Text);
                        API = textBox2.Text;
                        List<AccountEntry> NEW_USER = aa.new_user(API, KEY);
                        label3.Text = "Key Type: " + keyType;
                        foreach (var c in NEW_USER)
                        {
                            listBox1.Items.Add(c);
                        }
                        string fname = "APIKeyInfoApi." + KEY + ".0.xml";
                        label4.Text = "Character ID: " + Convert.ToString(aa.ReturnCharID(fname));
                        USER = aa.ReturnCharID(fname);
                        listBox1.Visible = true;
                        Keyexp = aa.Keyexp(API, KEY);
                        result = String.Equals(Keyexp, nonexp, StringComparison.Ordinal);
                        if (result == true)
                        {
                            label6.Text = "Key Expires: Never";
                        }
                        else
                        {
                            label6.Text = "Key Expires: " + Keyexp;
                        }
                        label7.Text = "Acct Expires: " + aa.ACStatus(API, KEY);
                        string Userimg = "http://image.eveonline.com/Character/" + USER + "_256.jpg";
                        pictureBox1.Load(Userimg);
                        pictureBox1.Image.Save(Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\AppData\Local\EVECM\images\" + USER + "_256.jpg"));
                        int CorpID = Convert.ToInt32(aa.CSheet(API, KEY, USER).CorporationID);
                    }
                    catch (Exception)
                    {
                        label3.Text = "";
                        listBox1.Visible = false;
                        label8.ForeColor = Color.Red;
                        button4.Visible = true;
                    }
                }

            else if (NEXT != 0)
            {
                try
                {

                    KEY = Convert.ToInt32(textBox1.Text);
                    API = textBox2.Text;
                    string fname = "APIKeyInfoApi." + KEY + ".0.xml";
                    USER = aa.ReturnCharID(fname);
                    string name = aa.CInfo(API, KEY, USER).Name;
                    string SkillinTraining = aa.skillintraining(API, KEY, USER).Skill.Name;
                    int SkillLevel = aa.skillintraining(API, KEY, USER).TrainingToLevel;
                    Keyexp = aa.Keyexp(API, KEY);
                    result = String.Equals(Keyexp, nonexp, StringComparison.Ordinal);
                    DateTime SkillendTime = aa.skillintraining(API, KEY, USER).TrainingEndLocalTime;
                    int CorpID = Convert.ToInt32(aa.CSheet(API, KEY, USER).CorporationID);
                    int TSP = aa.CSheet(API, KEY, USER).SkillpointTotal;
                    string clone = aa.CSheet(API, KEY, USER).CloneName;
                    string clklsys = aa.CInfo(API, KEY, USER).LastKnownLocation;
                    string clklship = aa.CInfo(API, KEY, USER).ShipTypeName;
                    string clklshipname = aa.CInfo(API, KEY, USER).ShipName;
                    result = String.Equals(Keyexp, nonexp, StringComparison.Ordinal);
                    if (result == true)
                    {
                        Keyexp = "Never";
                    }
                    else
                    {
                        DateTime dtKeyexp = Convert.ToDateTime(Keyexp);
                        Keyexp = dbconn.datetimestring(dtKeyexp);
                    }
                    SQLiteConnection testdb = new SQLiteConnection();
                    testdb = new SQLiteConnection("Data Source=" + ECMDBlocation);
                    string Acctexp = aa.ACStatus(API, KEY);
                    try
                    {
                        testdb.Open();
                        string sql = "insert into CharacterInfo (KEY, API, charID, Name, CorpID, SkillinTraining, SkillLevel, SkillEndTime, Keyexp, Acctexp, TSP, CloneGrade, LastLocation, ShipType, ShipName) values (" + KEY + ", '" + API + "', " + USER + ", '" + dbconn.stringApoph(name) + "', " + CorpID + ", '" + SkillinTraining + "', " + SkillLevel + ", '" + dbconn.datetimestring(SkillendTime) + "', '" + Keyexp + "', '" + dbconn.datetimestring(Convert.ToDateTime(Acctexp)) + "', " + TSP + ", '" + clone + "', '" + clklsys + "', '" + clklship + "', '" + dbconn.stringApoph(clklshipname) + "' )";
                        SQLiteCommand cmd = new SQLiteCommand(sql, testdb);
                        cmd.ExecuteNonQuery();
                        testdb.Close();

                    }
                    catch (SQLiteException ex)
                    {
                        MessageBox.Show(Convert.ToString(ex));
                    }
                    try
                    {
                        testdb.Open();
                        string sql = "create table if not exists Char_" + USER + "_skills (SkillName char, SkillID int UNIQUE, SkillLevel int, SkillRank int, SPCurrent int, SPToNextLevel int, SPForNextLevel int, timeNeededForNextLevel char, timeTotalForNextLevel char)";
                        SQLiteCommand cmd = new SQLiteCommand(sql, testdb);
                        cmd.ExecuteNonQuery();
                        testdb.Close();

                    }
                    catch (SQLiteException ex)
                    {
                        MessageBox.Show(Convert.ToString(ex));
                    }
                }
                catch (Exception ex)
                {
                    label7.Text = Convert.ToString(ex);
                }
                resetkeyMGMT();
            }
            
            }

        private void AccountKey(string keyType)
        {
            
            if (NEXT == 0)
            {
                try
                {
                    NEXT++;
                    button2.Text = "Continue";
                    KEY = Convert.ToInt32(textBox1.Text);
                    API = textBox2.Text;
                    List<AccountEntry> NEW_USER = aa.new_user(API, KEY);
                    label3.Text = "Key Type: " + keyType;
                    foreach (var c in NEW_USER)
                    {
                        listBox1.Items.Add(c);
                    }
                    string fname = "APIKeyInfoApi." + KEY + ".0.xml";
                    label4.Text = "Character ID: " + Convert.ToString(aa.ReturnCharID(fname));
                    USER = aa.ReturnCharID(fname);
                    listBox1.Visible = true;
                    Keyexp = aa.Keyexp(API, KEY);
                    result = String.Equals(Keyexp, nonexp, StringComparison.Ordinal);
                    if (result == true)
                    {
                        label6.Text = "Key Expires: Never";
                    }
                    else
                    {
                        label6.Text = "Key Expires: " + Keyexp;
                    }
                    label7.Text = "Acct Expires: " + aa.ACStatus(API, KEY);
                    string Userimg = "http://image.eveonline.com/Character/" + USER + "_256.jpg";
                    pictureBox1.Load(Userimg);
                    pictureBox1.Image.Save(Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\AppData\Local\EVECM\images\" + USER + "_256.jpg"));
                    int CorpID = Convert.ToInt32(aa.CSheet(API, KEY, USER).CorporationID);
                }
                catch (Exception)
                {
                    label3.Text = "";
                    listBox1.Visible = false;
                    label8.ForeColor = Color.Red;
                    button4.Visible = true;
                }
            }

            else if (NEXT != 0)
            {
                try
                {

                    KEY = Convert.ToInt32(textBox1.Text);
                    API = textBox2.Text;
                    string fname = "APIKeyInfoApi." + KEY + ".0.xml";
                    USER = aa.ReturnCharID(fname);
                    string name = aa.CInfo(API, KEY, USER).Name;
                    string SkillinTraining = aa.skillintraining(API, KEY, USER).Skill.Name;
                    int SkillLevel = aa.skillintraining(API, KEY, USER).TrainingToLevel;
                    Keyexp = aa.Keyexp(API, KEY);
                    result = String.Equals(Keyexp, nonexp, StringComparison.Ordinal);
                    DateTime SkillendTime = aa.skillintraining(API, KEY, USER).TrainingEndLocalTime;
                    int CorpID = Convert.ToInt32(aa.CSheet(API, KEY, USER).CorporationID);
                    int TSP = aa.CSheet(API, KEY, USER).SkillpointTotal;
                    string clone = aa.CSheet(API, KEY, USER).CloneName;
                    string clklsys = aa.CInfo(API, KEY, USER).LastKnownLocation;
                    string clklship = aa.CInfo(API, KEY, USER).ShipTypeName;
                    string clklshipname = aa.CInfo(API, KEY, USER).ShipName;
                    result = String.Equals(Keyexp, nonexp, StringComparison.Ordinal);
                    if (result == true)
                    {
                        Keyexp = "Never";
                    }
                    else
                    {
                        DateTime dtKeyexp = Convert.ToDateTime(Keyexp);
                        Keyexp = dbconn.datetimestring(dtKeyexp);
                    }
                    SQLiteConnection testdb = new SQLiteConnection();
                    testdb = new SQLiteConnection("Data Source=" + ECMDBlocation);
                    string Acctexp = aa.ACStatus(API, KEY);
                    try
                    {
                        testdb.Open();
                        string sql = "insert into CharacterInfo (KEY, API, charID, Name, CorpID, SkillinTraining, SkillLevel, SkillEndTime, Keyexp, Acctexp, TSP, CloneGrade, LastLocation, ShipType, ShipName) values (" + KEY + ", '" + API + "', " + USER + ", '" + dbconn.stringApoph(name) + "', " + CorpID + ", '" + SkillinTraining + "', " + SkillLevel + ", '" + dbconn.datetimestring(SkillendTime) + "', '" + Keyexp + "', '" + dbconn.datetimestring(Convert.ToDateTime(Acctexp)) + "', " + TSP + ", '" + clone + "', '" + clklsys + "', '" + clklship + "', '" + dbconn.stringApoph(clklshipname) + "' )";
                        SQLiteCommand cmd = new SQLiteCommand(sql, testdb);
                        cmd.ExecuteNonQuery();
                        testdb.Close();

                    }
                    catch (SQLiteException ex)
                    {
                        MessageBox.Show(Convert.ToString(ex));
                    }
                    try
                    {
                        testdb.Open();
                        string sql = "create table if not exists Char_" + USER + "_skills (SkillName char, SkillID int UNIQUE, SkillLevel int, SkillRank int, SPCurrent int, SPToNextLevel int, SPForNextLevel int, timeNeededForNextLevel char, timeTotalForNextLevel char)";
                        //create table char_90981690_kills (KillID char, KillTime char, KillTimeLocal char, Attackers char, DestroyedItems char, Moon char, MoonID char, SolarSystem char, solarSystemID char, Victim char
                        SQLiteCommand cmd = new SQLiteCommand(sql, testdb);
                        cmd.ExecuteNonQuery();
                        testdb.Close();

                    }
                    catch (SQLiteException ex)
                    {
                        MessageBox.Show(Convert.ToString(ex));
                    }
                    try
                    {
                        testdb.Open();
                        string sql = "create table char_90981690_kills (KillID  char UNIQUE, KillTime char, KillTimeLocal char, Attackers char, DestroyedItems char, Moon char, MoonID char, SolarSystem char, solarSystemID char, Victim char)";
                        SQLiteCommand cmd = new SQLiteCommand(sql, testdb);
                        cmd.ExecuteNonQuery();
                        testdb.Close();
                    }
                    catch { }
                }
                catch (Exception ex)
                {
                    label7.Text = Convert.ToString(ex);
                }
                resetkeyMGMT();
            }
        }

        private void CorporateKey(string keyType)
        {
            
            if (NEXT == 0)
            {
                try
                {
                    NEXT++;
                    button2.Text = "Continue";
                    CORP_KEY = Convert.ToInt32(textBox1.Text);
                    CORP_API = textBox2.Text;
                    label3.Text = "Key Type: " + keyType;
                    string cfname = "APIKeyInfoApi." + CORP_KEY + ".0.xml";
                    CORP_USER = caa.ReturnCharID(cfname);
                    string CorpName = caa.CorpSheet(CORP_KEY, CORP_API, CORP_USER).CorporationName + ": " + caa.CorpSheet(CORP_KEY, CORP_API, CORP_USER).Ticker;
                    listBox1.Items.Add(CorpName);
                    label4.Text = "Corp ID: " + Convert.ToString(CORP_USER);
                    Keyexp = aa.Keyexp(API, KEY);
                    result = String.Equals(Keyexp, nonexp, StringComparison.Ordinal);
                    if (result == true)
                    {
                        label6.Text = "Key Expires: Never";
                    }
                    else
                    {
                        label6.Text = "Key Expires: " + Keyexp;
                    }
                    label7.Text = "Acct Expires: " + aa.ACStatus(API, KEY);
                    string Userimg = "http://image.eveonline.com/Corporation/" + CORP_USER + "_256.png";
                    pictureBox1.Load(Userimg);
                    pictureBox1.Image.Save(Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\AppData\Local\EVECM\images\" + CORP_USER + "_256.jpg"));
                    listBox1.Visible = true;
                }
                catch (Exception)
                {
                    label3.Text = "";
                    listBox1.Visible = false;
                    label8.ForeColor = Color.Red;
                    button4.Visible = true;
                }
            }
            else if (NEXT != 0)
            {
                CORP_KEY = Convert.ToInt32(textBox1.Text);
                CORP_API = textBox2.Text;
                string cfname = "APIKeyInfoApi." + CORP_KEY + ".0.xml";
                CORP_USER = caa.ReturnCharID(cfname);
                string CorpName = caa.CorpSheet(CORP_KEY, CORP_API, CORP_USER).CorporationName;
                string CEOName = caa.CorpSheet(CORP_KEY, CORP_API, CORP_USER).CeoName;
                int CEOId = Convert.ToInt32(caa.CorpSheet(CORP_KEY, CORP_API, CORP_USER).CeoID);
                int MemberCount = caa.CorpSheet(CORP_KEY, CORP_API, CORP_USER).MemberCount;
                int MemberLimit = caa.CorpSheet(CORP_KEY, CORP_API, CORP_USER).MemberLimit;
                Keyexp = aa.Keyexp(CORP_API, CORP_KEY);
                int CorpID = Convert.ToInt32(caa.CorpSheet(CORP_KEY, CORP_API, CORP_USER).CorporationID);
                string AllianceName = Convert.ToString(caa.CorpSheet(CORP_KEY, CORP_API, CORP_USER).Alliance);
                int AllianceID = Convert.ToInt32(caa.CorpSheet(CORP_KEY, CORP_API, CORP_USER).AllianceID);
                string Faction = Convert.ToString(caa.CorpSheet(CORP_KEY, CORP_API, CORP_USER).Faction);
                int FactionID = caa.CorpSheet(CORP_KEY, CORP_API, CORP_USER).FactionID;
                List<string> wn = cwaa.wallet(CORP_KEY, CORP_API, CEOId);
                List<string> wi = cwaa.walletID(CORP_KEY, CORP_API, CEOId);
                List<string> ADName = caa.AssetDivisionsString(CORP_KEY, CORP_API, CEOId);
                List<string> ADInt = caa.AssetDivisionsID(CORP_KEY, CORP_API, CEOId);
                result = String.Equals(Keyexp, nonexp, StringComparison.Ordinal);
                if (result == true)
                {
                    Keyexp = "Never";
                }
                else
                {
                    DateTime dtKeyexp = Convert.ToDateTime(Keyexp);
                    Keyexp = dbconn.datetimestring(dtKeyexp);
                }
                string Acctexp = aa.ACStatus(CORP_API, CORP_KEY);
                try
                {
                    ECMDB = new SQLiteConnection("Data Source=" + ECMDBlocation);
                    ECMDB.Open();
                    string sql = "insert into CorporationInfo (CORPKEY, CORPAPI, CorpUser, CorpName, CorpID, CEOName, CEOId, AllianceName, AllianceID, Faction, FactionID,  Members, MemberLimit, Keyexp, Wallet1Name, Wallet1ID, Wallet2Name, Wallet2ID, Wallet3Name, Wallet3ID, Wallet4Name, Wallet4ID, Wallet5Name, Wallet5ID, Wallet6Name, Wallet6ID, Wallet7Name, Wallet7ID, AD1Name, AD1ID, AD2Name, AD2ID, AD3Name, AD3ID, AD4Name, AD4ID, AD5Name, AD5ID, AD6Name, AD6ID, AD7Name, AD7ID) values (" + CORP_KEY + ", '" + CORP_API + "', " + CORP_USER + ", '" + CorpName + "', " + CorpID + ", '" + CEOName + "', " + CEOId + ", '" + AllianceName + "', " + AllianceID + ", '" + Faction + "', " + FactionID + ", " + MemberCount + ", " + MemberLimit + ", '" + Keyexp + "', '" + dbconn.stringApoph(wn[0]) + "', " + Convert.ToInt32(wi[0]) + ", '" + dbconn.stringApoph(wn[1]) + "', " + Convert.ToInt32(wi[1]) + ", '" + wn[2] + "', " + Convert.ToInt32(wi[2]) + ", '" + dbconn.stringApoph(wn[3]) + "', " + Convert.ToInt32(wi[3]) + ", '" + dbconn.stringApoph(wn[4]) + "', " + Convert.ToInt32(wi[4]) + ", '" + dbconn.stringApoph(wn[5]) + "', " + Convert.ToInt32(wi[5]) + ", '" + dbconn.stringApoph(wn[6]) + "', " + Convert.ToInt32(wi[6]) + ", '" + ADName[0] + "', " + Convert.ToInt32(ADInt[0]) + ", '" + ADName[1] + "', " + Convert.ToInt32(ADInt[1]) + ", '" + ADName[2] + "', " + Convert.ToInt32(ADInt[2]) + ", '" + ADName[3] + "', " + Convert.ToInt32(ADInt[3]) + ", '" + ADName[4] + "', " + Convert.ToInt32(ADInt[4]) + ", '" + ADName[5] + "', " + Convert.ToInt32(ADInt[5]) + ", '" + ADName[6] + "', " + Convert.ToInt32(ADInt[6]) + ")";
                    SQLiteCommand cmd = new SQLiteCommand(sql, ECMDB);
                    cmd.ExecuteNonQuery();
                    string sql1 = "Create table Corp_" + CORP_USER + "_Wallet_Journal (Wallet_ID int, AdditionalDataID int, AdditionalData char, Amount int, Balance int, Date char, DateTimeLocal char, GivingPartyID int, GivingPartyName char, GivingPartyType char, GivingPartyTypeID int, JournalID char unique, Reason char, RecvdPartyID int, RecvdPartyName char, RecvdPartyType int, RecvdPartyTypeID int, Tax int, TaxRecvdID int, XferType char, XferTypeID int)";
                    string sql2 = "Create table Corp_" + CORP_USER + "_Wallet_Transactions (Wallet_ID int, ClientID int, ClientName char, ClientType char, ClientTypeID, Date char, DateTimeLocal char,Price int, PriceTotal int, Quantity int, Station char, StationID int, StationName char, TransactionFor char, TransactionID char unique, TransactionType char, ProductType char, TypeID int, TypeName char)";
                    string sql3 = "create table Corp_" + CORP_USER + "_Members (Base char,BaseID int, BaseName char, CharacterID int Unique, GrantableRoles char, JoinDate char, JoinDateLocalTime char, LocationID int, LocationName char, LogOffDate char, LogoffdateLocalTime char, LogonDate char, LogonDateLocalTime char, Name char, ShipType char, ShipTypeID int, ShipTypeName char, Title char, UserRoles char)";
                    string sql4 = "Create Table Corp_" + CORP_USER + "_Wallet_Balances (AccountID int Unique, Balance char)";
                    SQLiteCommand cmd1 = new SQLiteCommand(sql1, ECMDB);
                    SQLiteCommand cmd2 = new SQLiteCommand(sql2, ECMDB);
                    SQLiteCommand cmd3 = new SQLiteCommand(sql3, ECMDB);
                    SQLiteCommand cmd4 = new SQLiteCommand(sql4, ECMDB);
                    cmd1.ExecuteNonQuery();
                    cmd2.ExecuteNonQuery();
                    cmd3.ExecuteNonQuery();
                    cmd4.ExecuteNonQuery();
                    ECMDB.Close();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(Convert.ToString(ex));
                }
                try
                {
                    foreach (var i in wi)
                    {
                        int Walletint = Convert.ToInt32(i);
                        worker.JournalEntryBuilder(CORP_KEY, CORP_API, CEOId, Walletint, CORP_USER);
                    }
                    foreach (var i in wi)
                    {
                        int Walletint = Convert.ToInt32(i);
                        worker.TransactionEntryBuilder(CORP_KEY, CORP_API, CEOId, Walletint, CORP_USER);
                    }
                    WalletBalance(CORP_KEY, CORP_API, CORP_USER, CEOId);
                    //CorpMembers(CORP_KEY, CORP_API, CEOId, CORP_USER);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Convert.ToString(ex));
                }
                resetkeyMGMT();
            }
            
        }

        private void resetkeyMGMT()
        {
            button2.Text = "Next";
            listBox1.Visible = false;
            listBox1.Items.Clear();
            label3.Text = "";
            label4.Text = "";
            label6.Text = "";
            label7.Text = "";
            label8.Text = "Accepts Account, Character, and Corporate keys";
            textBox1.Clear();
            textBox2.Clear();
            pictureBox1.Image = null;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            button4.Visible = false;
            label8.ForeColor = Color.Black;
            NEXT = 0;
        }

        private void WalletBalance(int CORP_KEY, string CORP_API, int CORP_USER, int CEOId)
        {
            SQLiteConnection ECMDB = new SQLiteConnection("Data Source=" + ECMDBlocation);
            EveApi api = new EveApi(CORP_KEY, CORP_API, CEOId);
            List<AccountBalance> caccount = api.GetCorporationAccountBalance();
            ECMDB.Open();
            foreach (var line in caccount)
            {
                try
                {
                    string apiString = line.ToString();
                    string[] tokens = apiString.Split(' ');
                    string sql = "insert into Corp_" + CORP_USER + "_Wallet_Balances (AccountID, Balance) values ('" + tokens[0] + "', '" + tokens[2] + "')";
                    SQLiteCommand cmd = new SQLiteCommand(sql, ECMDB);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception) { }
            }
            ECMDB.Close();
        }

        public void CorpMembers(int CORP_KEY, string CORP_API, int CEOId, int CORP_USER)
        {
            SQLiteConnection ECMDB = new SQLiteConnection("Data Source=" + ECMDBlocation);
            EveApi api = new EveApi(CORP_KEY, CORP_API, CEOId);
            List<MemberTrackingEntry> memcount = api.GetCorporationMemberTracking();
            ECMDB.Open();
            foreach (var line in memcount)
            {
                try
                {
                    string Base = Convert.ToString(line.Base);
                    long BaseID = line.BaseID;
                    string BaseName = line.BaseName;
                    long CharacterID = line.CharacterID;
                    string GrantableRoles = Convert.ToString(line.GrantableRoles);
                    string JoinDate = dbconn.datetimestring(line.JoinDate);
                    string JoinDateLocalTime = dbconn.datetimestring(line.JoinDateLocalTime);
                    long LocationID = line.LocationID;
                    string LocationName = line.LocationName;
                    string LogoffDate = dbconn.datetimestring(line.LogoffDate);
                    string LogoffDateLocalTime = dbconn.datetimestring(line.LogoffDateLocalTime);
                    string LogonDate = dbconn.datetimestring(line.LogonDate);
                    string LogonDateLocalTime = dbconn.datetimestring(line.LogonDateLocalTime);
                    string Name = line.Name;
                    string ShipType = Convert.ToString(line.ShipType);
                    int ShipTypeID = line.ShipTypeID;
                    string ShipTypeName = line.ShipTypeName;
                    string Title = line.Title;
                    string UserRoles = Convert.ToString(line.UserRoles);
                    string sql = "Insert into Corp_" + CORP_USER + "_Members (Base, BaseID, BaseName, CharacterID, GrantableRoles, JoinDate, JoinDateLocalTime, LocationID, LocationName, LogoffDate, LogoffDateLocalTime, LogonDate, LogonDateLocaltime, Name, ShipType, ShipTypeID, ShipTypeName, Title, UserRoles) values ('" + Base + "', " + BaseID + ", '" + BaseName + "', " + CharacterID + ", '" + GrantableRoles + "', '" + JoinDate + "', '" + JoinDateLocalTime + "', " + LocationID + ", '" + LocationName + "', '" + LogoffDate + "', '" + LogoffDateLocalTime + "', '" + LogonDate + "', '" + LogonDateLocalTime + "', '" + Name + "', '" + ShipType + "', " + ShipTypeID + ", '" + ShipTypeName + "', '" + Title + "', = '" + UserRoles + "')";
                    SQLiteCommand cmd = new SQLiteCommand(sql, ECMDB);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception)
                { }
            }
            ECMDB.Close();

        }

        private void API_KEY_mgmt_FormClosing(object sender, FormClosingEventArgs e)
        {
            worker.mainFormStartUpdater();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            resetkeyMGMT();
        }
    }
}
