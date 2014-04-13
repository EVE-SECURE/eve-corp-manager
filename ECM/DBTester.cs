using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Text.RegularExpressions;
using EveAI.Live.Corporation;
using EveAI.Live.Character;
using EveAI.Production;
using EveAI.Live;

namespace ECM
{
    public partial class DBTester : Form
    {
        string ECM_cache = Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\AppData\Local\EVECM\");
        string ECMDBlocation = Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\AppData\Local\EVECM\db\EVECM.db");
        Common.Database.dbconn dbconn = new Common.Database.dbconn();
        CorpWalletapiArray cwaa = new CorpWalletapiArray();
        apiArrays aa = new apiArrays();
        //string Keyexp = "";
        //int KEY = 0;
        int CORP_KEY = 0;
        int CORP_USER = 0;
        //int USER = 0; // CEOId = 0;
        //string API = "";
        string CORP_API = "";
        //int CW = 1000;

        public DBTester()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CorpInitializer();

            AuthenticationData auth = new AuthenticationData();
            auth.KeyID = CORP_KEY;
            auth.VCode = CORP_API;
            auth.CharacterID = CORP_USER;
            
            //AssemblyLineType
        }
        #region OHHH GOD THE DB WORK

       
        
        public void CorpInitializer()
        {
            SQLiteConnection ECMDB = new SQLiteConnection("Data Source=" + ECMDBlocation);
            string sql = "select CorpKEY, CORPAPI, CORPUSER from CorporationInfo";
            ECMDB.Open();
            SQLiteCommand cmd = new SQLiteCommand(sql, ECMDB);
            SQLiteDataReader TableReader = cmd.ExecuteReader();
            while (TableReader.Read())
            {
                CORP_KEY = Convert.ToInt32(TableReader["CORPKEY"]);
                CORP_API = Convert.ToString(TableReader["CORPAPI"]);
                CORP_USER = Convert.ToInt32(TableReader["CORPUSER"]);
            }
            TableReader.Close();
            ECMDB.Close();
            membertrack();
        }




        private void membertrack()
        {

            SQLiteConnection ECMDB = new SQLiteConnection("Data Source=" + ECMDBlocation);
            AuthenticationData auth = new AuthenticationData();
            auth.KeyID = CORP_KEY;
            auth.VCode = CORP_API;
            auth.TrackingExtended = true;

            MemberTrackingApi api = new MemberTrackingApi();
            api.AuthenticationData = auth;
            api.UpdateData(EveApiBase.UpdateCharaceristics.Default);

            List<MemberTrackingEntry> mt = api.Data;
            ECMDB.Open();
            foreach (var line in mt)
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
                    string sql = "Insert into Corp_" + CORP_USER + "_Members (Base, BaseID, BaseName, CharacterID, GrantableRoles, JoinDate, JoinDateLocalTime, LocationID, LocationName, LogoffDate, LogoffDateLocalTime, LogonDate, LogonDateLocalTime, Name, ShipType, ShipTypeID, ShipTypeName, Title, UserRoles) values ('" + Base + "', " + BaseID + ", '" + BaseName + "', " + CharacterID + ", '" + GrantableRoles + "','" + JoinDate + "', '" + JoinDateLocalTime + "', " + LocationID + ", '" + LocationName + "', '" + LogoffDate + "', '" + LogoffDateLocalTime + "', '" + LogonDate + "', '" + LogonDateLocalTime + "', '" + Name + "', '" + ShipType + "', " + ShipTypeID + ", '" + ShipTypeName + "', '" + Title + "', '" + UserRoles + "')";
                    //string sql = "Insert into Corp_" + CORP_USER + "_Members (BaseID, CharacterID, Name, JoinDate, JoinDateLocalTime) values (" + BaseID + ", " + CharacterID + ", '" + Name + "', '" + JoinDate + "', '" + JoinDateLocalTime + "')";
                    //string sql = "update Corp_" + CORP_USER + "_Members set Base = '" + Base + "', BaseID = " + BaseID + ", BaseName = '" + BaseName + "', CharacterID = " + CharacterID + ", GrantableRoles = '" + GrantableRoles + "', JoinDate = '" + JoinDate + "', JoinDateLocalTime = '" + JoinDateLocalTime +"', LocationID = " + LocationID + ", LocationName = '" + LocationName + "', LogoffDate = '" + LogoffDate + "', LogoffDateLocalTime = '" + LogoffDateLocalTime + "', LogonDate = '" + LogonDate + "', LogonDateLocaltime = '" + LogonDateLocalTime + "', Name = '" + Name + "', ShipType = '" + ShipType + "', ShipTypeID = " + ShipTypeID + ", ShipTypeName = '" + ShipTypeName + "', Title = '" + Title + "', UserRoles = '" + UserRoles + "'";
                    SQLiteCommand cmd = new SQLiteCommand(sql, ECMDB);
                    cmd.ExecuteNonQuery();
                }
                catch
                {

                }
                ECMDB.Close();
            }
        }


        #endregion




    }
}
