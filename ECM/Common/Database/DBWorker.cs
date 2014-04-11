using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using EveAI.Live;
using EveAI.Live.Corporation;
using EveAI.Live.Character;
using EveAI.Live.Generic;
using EveAI.SpaceStation;
using System.Threading;
using System.IO;
using System.Windows.Forms;

namespace ECM.Common.Database
{
    class DBWorker
    {
        bool result;
        string nonexp = "1/1/0001 12:00:00 AM", Keyexp = "";
        public int CORP_KEY = 0, CORP_USER = 0, CEOId = 0;
        string CORP_API = "";
        int KEY = 0, USER = 0;
        string API = "";
        public int startupToken = 0;
        public string ECMDBlocation = Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\AppData\Local\EVECM\db\EVECM.db");
        //string ECM_cache = Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\AppData\Local\EVECM\");
        Common.Database.dbconn dbconn = new Common.Database.dbconn();
        CorpApiArrays caa = new CorpApiArrays();
        apiArrays aa = new apiArrays();
        CorpWalletapiArray cwaa = new CorpWalletapiArray();
        POSapiArrays POS = new POSapiArrays();
        


        /// <summary>
        /// Starts the update threads for the corp and characters
        /// </summary>
        public void mainFormStartUpdater()
        {
            int test;
            try
            {
                SQLiteConnection ECMDB = new SQLiteConnection("Data Source=" + ECMDBlocation);
                ECMDB.Open();
                string sql = "select Count(*) from CharacterInfo";
                SQLiteCommand cmd = new SQLiteCommand(sql, ECMDB);
                test = Convert.ToInt32(cmd.ExecuteScalar());
                if (test != 0)
                {
                    startupToken = 1;
                    Thread CUI = new Thread(corpInitializer); //Corp Initializer
                    Thread CHUI = new Thread(characterIntializer); //Character Initializer
                    CUI.Start();
                    CHUI.Start();
                }
            }
            catch { }
                

        }

        #region Corporation DB Workers

        /// <summary>
        /// PUll The 3 Variables required for updating Corporation info.
        /// </summary>
        private void corpInitializer()
        {
            SQLiteConnection ECMDB = new SQLiteConnection("Data Source=" + ECMDBlocation);
            string sql = "select CorpKEY, CORPAPI, CORPUSER, CEOId from CorporationInfo";
            ECMDB.Open();
            SQLiteCommand cmd = new SQLiteCommand(sql, ECMDB);
            SQLiteDataReader TableReader = cmd.ExecuteReader();
            while (TableReader.Read())
            {
                CORP_KEY = Convert.ToInt32(TableReader["CORPKEY"]);
                CORP_API = Convert.ToString(TableReader["CORPAPI"]);
                CORP_USER = Convert.ToInt32(TableReader["CORPUSER"]);
                CEOId = Convert.ToInt32(TableReader["CEOId"]);
            }
            TableReader.Close();
            ECMDB.Close();
            corpSheetBuilder(CORP_KEY, CORP_API, CORP_USER, CEOId);
            CorpMembers(CORP_KEY, CORP_API);
            WalletUpdate();
        }
        /// <summary>
        /// Uses the variables from the previous method to pull the Corpsheet, Sets the CEOId from the EVE APi,
        /// and updates the Corpsheet info into the database.
        /// </summary>
        /// <param name="CORP_KEY"></param>
        /// <param name="CORP_API"></param>
        /// <param name="CORP_USER"></param>
        /// <param name="CEOId"></param>
        private void corpSheetBuilder(int CORP_KEY, string CORP_API, int CORP_USER, int CEOId)
        {
            try
            {
                SQLiteConnection ECMDB = new SQLiteConnection("Data Source=" + ECMDBlocation);
                string CorpName = caa.CorpSheet(CORP_KEY, CORP_API, CORP_USER).CorporationName;
                string CEOName = caa.CorpSheet(CORP_KEY, CORP_API, CORP_USER).CeoName;
                CEOId = Convert.ToInt32(caa.CorpSheet(CORP_KEY, CORP_API, CORP_USER).CeoID);
                int MemberCount = caa.CorpSheet(CORP_KEY, CORP_API, CORP_USER).MemberCount;
                int MemberLimit = caa.CorpSheet(CORP_KEY, CORP_API, CORP_USER).MemberLimit;
                //Keyexp = aa.Keyexp(CORP_API, CORP_KEY);
                int CorpID = Convert.ToInt32(caa.CorpSheet(CORP_KEY, CORP_API, CORP_USER).CorporationID);
                string AllianceName = Convert.ToString(caa.CorpSheet(CORP_KEY, CORP_API, CORP_USER).Alliance);
                int AllianceID = Convert.ToInt32(caa.CorpSheet(CORP_KEY, CORP_API, CORP_USER).AllianceID);
                string Faction = Convert.ToString(caa.CorpSheet(CORP_KEY, CORP_API, CORP_USER).Faction);
                int FactionID = caa.CorpSheet(CORP_KEY, CORP_API, CORP_USER).FactionID;
                List<string> wn = cwaa.wallet(CORP_KEY, CORP_API, CEOId);
                List<string> ADName = caa.AssetDivisionsString(CORP_KEY, CORP_API, CEOId);
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
                string sql = "update CorporationInfo set CorpName = '" + CorpName + "', CEOName = '" + CEOName + "', CEOId = " + CEOId + ", Members = " + MemberCount + ", MemberLimit = " + MemberLimit + ", Keyexp = '" + Keyexp + "', CorpID = " + CorpID + ", AllianceName = '" + AllianceName + "', AllianceID = " + AllianceID + ", Faction = '" + Faction + "', FactionID = " + FactionID + ", Wallet1Name = '" + wn[0] + "', Wallet2Name = '" + wn[1] + "', Wallet3Name = '" + wn[2] + "', Wallet4Name = '" + wn[3] + "', Wallet5Name = '" + wn[4] + "', Wallet6Name = '" + wn[5] + "', Wallet7Name = '" + wn[6] + "', AD1Name = '" + ADName[0] + "', AD2Name = '" + ADName[1] + "', AD3Name = '" + ADName[2] + "', AD3Name = '" + ADName[3] + "', AD5Name = '" + ADName[4] + "', AD6Name = '" + ADName[5] + "', AD7Name = '" + ADName[6] + "'";
                SQLiteCommand cmd = new SQLiteCommand(sql, ECMDB);
                ECMDB.Open();
                cmd.ExecuteNonQuery();
                ECMDB.Close();
            }
            catch (Exception) { }
        }
        /// <summary>
        /// Uses the Corp variables to update the member info from the API.
        /// </summary>
        /// <param name="CORP_KEY"></param>
        /// <param name="CORP_API"></param>
        /// <param name="CEOId"></param>
        private void CorpMembers(int CORP_KEY, string CORP_API)
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
                    
                    string sql1 = "update Corp_" + CORP_USER + "_Members set Base = '" + Base + "', BaseID = " + BaseID + ", BaseName = '" + BaseName + "', CharacterID = " + CharacterID + ", GrantableRoles = '" + GrantableRoles + "', JoinDate = '" + JoinDate + "', JoinDateLocalTime = '" + JoinDateLocalTime +"', LocationID = " + LocationID + ", LocationName = '" + LocationName + "', LogoffDate = '" + LogoffDate + "', LogoffDateLocalTime = '" + LogoffDateLocalTime + "', LogonDate = '" + LogonDate + "', LogonDateLocaltime = '" + LogonDateLocalTime + "', Name = '" + Name + "', ShipType = '" + ShipType + "', ShipTypeID = " + ShipTypeID + ", ShipTypeName = '" + ShipTypeName + "', Title = '" + Title + "', UserRoles = '" + UserRoles + "' where CharID = "+ CharacterID;
                    SQLiteCommand cmd = new SQLiteCommand(sql, ECMDB);
                    //SQLiteCommand updateCMD = new SQLiteCommand(sql1, ECMDB);
                    cmd.ExecuteNonQuery();
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Convert.ToString(ex));
                }
                ECMDB.Close();
            }
        }
        /// <summary>
        /// Runs the wwallet update tasks.
        /// </summary>
        private void WalletUpdate()
        {
            List<string> wi = cwaa.walletID(CORP_KEY, CORP_API, CEOId);
            try
            {
                foreach (var i in wi)
                {
                    int Walletint = Convert.ToInt32(i);
                    JournalEntryBuilder(CORP_KEY, CORP_API, CEOId, Walletint, CORP_USER);
                }
                foreach (var i in wi)
                {
                    int Walletint = Convert.ToInt32(i);
                    TransactionEntryBuilder(CORP_KEY, CORP_API, CEOId, Walletint, CORP_USER);
                }
                WalletBalance(CORP_KEY, CORP_API, CEOId);
            }
            catch (Exception) { }
        }
        /// <summary>
        /// Populates Wallet balances.
        /// </summary>
        /// <param name="CORP_KEY"></param>
        /// <param name="CORP_API"></param>
        /// <param name="CEOId"></param>
        private void WalletBalance(int CORP_KEY, string CORP_API, int CEOId)
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
                    string sql = "update Corp_" + CORP_USER + "_Wallet_Balances Set  Balance = '" + tokens[2] + "' Where AccountID ='" + tokens[0] + "'";
                    SQLiteCommand cmd = new SQLiteCommand(sql, ECMDB);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception) { }
            }
            ECMDB.Close();
        }
        /// <summary>
        /// Udates wallet Journal info from the API
        /// </summary>
        /// <param name="CORP_KEY"></param>
        /// <param name="CORP_API"></param>
        /// <param name="CEOId"></param>
        /// <param name="CW"></param>
        /// <param name="CORP_USER"></param>
        public void JournalEntryBuilder(int CORP_KEY, string CORP_API, int CEOId, int CW, int CORP_USER)
        {
            SQLiteConnection ECMDB = new SQLiteConnection("Data Source=" + ECMDBlocation);
            EveApi api = new EveApi(CORP_KEY, CORP_API, CEOId, CW);
            List<JournalEntry> je = api.GetCorporationWalletJournal();
            ECMDB.Open();
            foreach (var line in je)
            {
                try
                {
                    int WalletID = CW;
                    long AddtlDataID = line.AdditionalDataID;
                    string AddtlData = line.AdditionalDataName;
                    decimal Amount = line.Amount;
                    decimal Balance = line.Balance;
                    string JournalDate = dbconn.datetimestring(line.Date);
                    string JournalLocaldate = dbconn.datetimestring(line.DateLocalTime);
                    long GivingPartyID = line.GivingPartyID;
                    string GivingPartyName = line.GivingPartyName;
                    string GivingPartyType = Convert.ToString(line.GivingPartyType);
                    int GivingPartyTypeID = line.GivingPartyTypeID;
                    long JournalID = line.JournalID;
                    string Reason = line.Reason;
                    long RecvdID = line.ReceivingPartyID;
                    string RecvdName = line.ReceivingPartyName;
                    string RecvdPartyType = Convert.ToString(line.ReceivingPartyType);
                    int RecvdPartyID = line.ReceivingPartyTypeID;
                    decimal tax = line.TaxAmount;
                    long TaxRecvdID = line.TaxReceiverID;
                    string TransferType = Convert.ToString(line.TransferType);
                    int TransferTypeID = line.TransferTypeID;
                    string sql = "insert into Corp_" + CORP_USER + "_Wallet_Journal (Wallet_ID, AdditionalDataID, AdditionalData, Amount, Balance, Date, DateTimeLocal, GivingPartyID, GivingPartyName, GivingPartyType, GivingPartyTypeID, JournalID, Reason, RecvdPartyID, RecvdPartyName, RecvdPartyType, RecvdPartyTypeID, Tax , TaxRecvdID, XferType, XferTypeID) values (" + CW + ", " + AddtlDataID + ", '" + AddtlData + "', " + Amount + ", " + Balance + ", '" + JournalDate + "', '" + JournalLocaldate + "', " + GivingPartyID + ", '" + GivingPartyName + "', '" + GivingPartyType + "', " + GivingPartyTypeID + ", " + JournalID + ", '" + Reason + "', " + RecvdID + ", '" + RecvdName + "', '" + RecvdPartyType + "', " + RecvdPartyID + ", " + tax + ", " + TaxRecvdID + ", '" + TransferType + "', " + TransferTypeID + ")";
                    SQLiteCommand cmd = new SQLiteCommand(sql, ECMDB);
                    cmd.ExecuteNonQuery();
                }
                catch (SQLiteException)
                {

                }
            }
            ECMDB.Close();
        }
        /// <summary>
        /// Udates wallet transactions info from the API
        /// </summary>
        /// <param name="CORP_KEY"></param>
        /// <param name="CORP_API"></param>
        /// <param name="CEOId"></param>
        /// <param name="CW"></param>
        /// <param name="CORP_USER"></param>
        public void TransactionEntryBuilder(int CORP_KEY, string CORP_API, int CEOId, int CW, int CORP_USER)
        {
            SQLiteConnection ECMDB = new SQLiteConnection("Data Source=" + ECMDBlocation);
            List<TransactionEntry> _lst = new List<TransactionEntry>();
            EveApi api = new EveApi(CORP_KEY, CORP_API, CEOId, CW);
            List<TransactionEntry> te = api.GetCorporationWalletTransactions();
            ECMDB.Open();
            foreach (var line in te)
            {
                try
                {
                    int WalletID = CW;
                    long ClientID = line.ClientID;
                    string ClientName = line.ClientName;
                    string ClientType = Convert.ToString(line.ClientType);
                    int ClientTypeID = line.ClientTypeID;
                    string Transdate = dbconn.datetimestring(line.Date);
                    string DateLocal = dbconn.datetimestring(line.DateLocalTime);
                    double Price = line.Price;
                    double PriceTotal = line.PriceTotal;
                    long Quantity = line.Quantity;
                    string Station = Convert.ToString(line.Station);
                    int StationID = line.StationID;
                    string StationName = line.StationName;
                    string TransactionFor = Convert.ToString(line.TransactionFor);
                    long TransactionID = line.TransactionID;
                    string TransactionType = Convert.ToString(line.TransactionType);
                    string ProductType = Convert.ToString(line.Type);
                    int ProductTypeID = line.TypeID;
                    string ProductTypeName = line.TypeName;
                    string sql = "Insert into Corp_" + CORP_USER + "_Wallet_Transactions (Wallet_ID, ClientID, ClientName, ClientType , ClientTypeID, Date, DateTimeLocal, Price, PriceTotal, Quantity, Station, StationID, StationName, TransactionFor, TransactionID, TransactionType, ProductType, TypeID, TypeName) values (" + WalletID + ", " + ClientID + ", '" + ClientName + "', '" + ClientType + "', " + ClientTypeID + ", '" + Transdate + "', '" + DateLocal + "', " + Price + ", " + PriceTotal + ", " + Quantity + ", '" + Station + "', " + StationID + ", '" + StationName + "', '" + TransactionFor + "', " + TransactionID + ", '" + TransactionType + "', '" + ProductType + "', " + ProductTypeID + ", '" + ProductTypeName + "')";
                    SQLiteCommand cmd = new SQLiteCommand(sql, ECMDB);
                    cmd.ExecuteNonQuery();
                }
                catch (SQLiteException)
                {

                }
            }
            ECMDB.Close();
        }

        public void CorpIndustryJobs(int CORP_KEY, string CORP_API, int CEOId)
        {
            EveApi api = new EveApi(CORP_KEY, CORP_API, CEOId);
            List<IndustryJob> jobs = api.GetCorporationIndustryJobs();
            foreach (var job in jobs)
            {
                long installerID = job.InstallerID;
            }
        }






        #endregion

        #region Corporation Data Returns, Pulls data for display on main form

        int walletToken = 0;
        /// <summary>
        /// returns the selected wallet division balance.
        /// </summary>
        /// <param name="CW"></param>
        /// <returns></returns>
        public string returnWalletBalance(int CW)
        {
            string bal = "";
            if (walletToken == 0)
            {
                corpInitializer();
                walletToken = 1;
            }
            else
            {
                SQLiteConnection ECMDB = new SQLiteConnection("Data Source=" + ECMDBlocation);
                string sql = "select Balance from Corp_" + CORP_USER + "_Wallet_Balances where AccountID =" + CW;
                ECMDB.Open();
                SQLiteCommand cmd1 = new SQLiteCommand(sql, ECMDB);
                SQLiteDataReader TableReader = cmd1.ExecuteReader();
                while (TableReader.Read())
                {
                    bal = Convert.ToString(TableReader["Balance"]);
                }
                TableReader.Close();
                ECMDB.Close();
            }
            return bal;
        }
        /// <summary>
        /// Returns Wallet Division Names
        /// </summary>
        /// <returns></returns>
        public Tuple<string, string, string, string, string, string, string> doWN()
        {
            SQLiteConnection ECMDB = new SQLiteConnection("Data Source=" + ECMDBlocation);
            ECMDB.Open();
            string wn1 = "", wn2 = "", wn3 = "", wn4 = "", wn5 = "", wn6 = "", wn7 = "";
            string sql = "Select Wallet1Name, Wallet2Name, Wallet3Name, Wallet4Name,  Wallet5Name, Wallet6Name, Wallet7Name from CorporationInfo Where CorpID = " + CORP_USER;
            SQLiteCommand cmd = new SQLiteCommand(sql, ECMDB);
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                wn1 = Convert.ToString(reader["Wallet1Name"]);
                wn2 = Convert.ToString(reader["Wallet2Name"]);
                wn3 = Convert.ToString(reader["Wallet3Name"]);
                wn4 = Convert.ToString(reader["Wallet4Name"]);
                wn5 = Convert.ToString(reader["Wallet5Name"]);
                wn6 = Convert.ToString(reader["Wallet6Name"]);
                wn7 = Convert.ToString(reader["Wallet7Name"]);

            }
            reader.Close();
            return Tuple.Create(wn1, wn2, wn3, wn4, wn5, wn6, wn7);
        }

        /// <summary>
        /// Generates the Corporation member list.
        /// </summary>
        /// <returns>
        /// Returns a list that is sent to listbox1 on the main form.
        /// </returns>
        public List<string> listMems()
        {
            List<string> _mems = new List<string>();
            string Name = "";
            int _charcorpID = 0, _CorpID = 0;
            SQLiteConnection ECMDB = new SQLiteConnection("Data Source=" + ECMDBlocation);
            string CorpID = "select CorpID from CorporationInfo";
            string charCorpID = "select Name, CorpID from CharacterInfo";
            ECMDB.Open();
            SQLiteCommand cmd = new SQLiteCommand(CorpID, ECMDB);
            SQLiteCommand cmd1 = new SQLiteCommand(charCorpID, ECMDB);
            SQLiteDataReader TableReader1 = cmd.ExecuteReader();
            while (TableReader1.Read())
            {
                _CorpID = Convert.ToInt32(TableReader1["CorpID"]);
            }
            TableReader1.Close();
            SQLiteDataReader TableReader2 = cmd1.ExecuteReader();
            while (TableReader2.Read())
            {
                Name = Convert.ToString(TableReader2["Name"]);
                _charcorpID = Convert.ToInt32(TableReader2["CorpID"]);
                if (_charcorpID == _CorpID)
                {
                    ;
                    _mems.Add(Name);
                }
            }
            TableReader2.Close();
            ECMDB.Close();
            return _mems;
        }
        /// <summary>
        /// Generates the member count and limit found above the member list.
        /// </summary>
        /// <returns></returns>
        public Tuple<int, int> doCMC()
        {
            SQLiteConnection ECMDB = new SQLiteConnection("Data Source=" + ECMDBlocation);
            ECMDB.Open();
            int _members = 0, _limit = 0;
            string sql1 = "Select Members, MemberLimit from CorporationInfo where CorpUser =" + CORP_USER;
            SQLiteCommand cmd1 = new SQLiteCommand(sql1, ECMDB);
            SQLiteDataReader reader = cmd1.ExecuteReader();
            while (reader.Read())
            {
                _members = Convert.ToInt32(reader["Members"]);
                _limit = Convert.ToInt32(reader["MemberLimit"]);
            }
            reader.Close();
            return Tuple.Create(_limit, _members);
        }



        #endregion


        #region Character DB Workers, updates character data to the datbase


        /// <summary>
        /// Initializes the Character Update functions
        /// </summary>
        private void characterIntializer()
        {
            
            List<int> CharID = new List<int>();
            SQLiteConnection ECMDB = new SQLiteConnection("Data Source=" + ECMDBlocation);
            ECMDB.Open();
            string sqlarray = "select CharID from CharacterInfo";
            SQLiteCommand cmd = new SQLiteCommand(sqlarray, ECMDB);
            SQLiteDataReader Charreader = cmd.ExecuteReader();
            while (Charreader.Read())
            {
                CharID.Add(Convert.ToInt32(Charreader["CharID"]));
            }
            foreach (var ID in CharID)
            {
                string sql1 = "select KEY, API from CharacterInfo where CharID = " + ID;
                USER = ID;
                SQLiteCommand cmd1 = new SQLiteCommand(sql1, ECMDB);
                SQLiteDataReader TableReader = cmd1.ExecuteReader();
                while (TableReader.Read())
                {
                    KEY = Convert.ToInt32(TableReader["KEY"]);
                    API = Convert.ToString(TableReader["API"]);
                }
                TableReader.Close();
                CharacterSkillspdater(API, KEY, USER);
                CharacterUpdateBuilder(API, KEY, USER);
                writeKills(API, KEY, USER);
            }
            ECMDB.Close();
        }
        
        /// <summary>
        /// Updates the characters in the characterinfo table.
        /// </summary>
        /// <param name="API"></param>
        /// <param name="KEY"></param>
        /// <param name="USER"></param>
        private void CharacterUpdateBuilder(string API, int KEY, int USER)
        {
            try
            {
                SQLiteConnection ECMDB = new SQLiteConnection("Data Source=" + ECMDBlocation);
                ECMDB.Open();
                string skillinTraining = aa.skillintraining(API, KEY, USER).Skill.Name;
                int skillLevel = aa.skillintraining(API, KEY, USER).TrainingToLevel;
                DateTime skillendTime = aa.skillintraining(API, KEY, USER).TrainingEndLocalTime;
                Keyexp = aa.Keyexp(API, KEY);
                int CorpID = Convert.ToInt32(aa.CSheet(API, KEY, USER).CorporationID);
                int TSP = aa.CSheet(API, KEY, USER).SkillpointTotal;
                string clone = aa.CSheet(API, KEY, USER).CloneName;
                string clklsys = aa.CInfo(API, KEY, USER).LastKnownLocation;
                string clklship = aa.CInfo(API, KEY, USER).ShipTypeName;
                string clklshipname = aa.CInfo(API, KEY, USER).ShipName;
                string sql = "update CharacterInfo set SkillInTraining = '" + skillinTraining + "', SkillLevel = " + skillLevel + ", SkillendTime = '" + skillendTime + "', CorpID = " + CorpID + ", TSP = " + TSP + ", CloneGrade = '" + clone + "', LastLocation = '" + clklsys + "', ShipType = '" + clklship + "', ShipName = '" + dbconn.stringApoph(clklshipname) + "' where CharID = " + USER;
                SQLiteCommand cmd1 = new SQLiteCommand(sql, ECMDB);
                cmd1.ExecuteNonQuery();
                ECMDB.Close();
            }
            catch (Exception) { }
        }
        /// <summary>
        /// Adds new skills and updates skills in the characters Skills table.
        /// </summary>
        /// <param name="API"></param>
        /// <param name="KEY"></param>
        /// <param name="USER"></param>
        internal void CharacterSkillspdater(string API, int KEY, int USER)
        {
            string skillName = "", timeNeededForNextLevel = "", timeTotalForNextLevel = "";
            int skillID = 0, skillLevel = 0, sPCurrent = 0, sPToNextLevel = 0, sPForNextLevel = 0;
            SQLiteConnection ECMDB = new SQLiteConnection("Data Source=" + ECMDBlocation);
            CharacterSheet _skill = new CharacterSheet();
            EveApi api = new EveApi(KEY, API, USER);
            CharacterSheet charskills = api.GetCharacterSheet();
            ECMDB.Open();
            foreach (CharacterSheet.LearnedSkill LS in charskills.Skills)
            {
                try
                {
                    skillName = Convert.ToString(LS.Skill);
                    skillID = Convert.ToInt32(LS.SkillTypeID);
                    skillLevel = Convert.ToInt32(LS.Level);
                    sPCurrent = Convert.ToInt32(LS.SkillPoints);
                    sPToNextLevel = Convert.ToInt32(LS.SkillPointsTotalForNextLevel);
                    sPForNextLevel = Convert.ToInt32(LS.SkillPointsNeededForNextLevel);
                    timeNeededForNextLevel = dbconn.SkillTimeConverter(LS.TimeNeededForNextLevel);
                    timeTotalForNextLevel = dbconn.SkillTimeConverter(LS.TimeTotalForNextLevel);
                    string SkillInsert = "Insert into Char_" + USER + "_skills (SkillName, SkillID, SkillLevel, SPCurrent, SPToNextLevel, SPForNextLevel, timeNeededForNextLevel, timeTotalForNextLevel) Values ('" + skillName + "', " + skillID + ", " + skillLevel + ", " + sPCurrent + ", " + sPToNextLevel + ", " + sPForNextLevel + ", '" + timeNeededForNextLevel + "', '" + timeTotalForNextLevel + "')";
                    SQLiteCommand cmd1 = new SQLiteCommand(SkillInsert, ECMDB);
                    string SkillUpdate = "Update Char_" + USER + "_skills set SkillLevel = " + skillLevel + ", SPCurrent =" + sPCurrent + ", SPToNextLevel =" + sPToNextLevel + ", SPForNextLevel =" + sPForNextLevel + ", timeNeededForNextLevel ='" + timeNeededForNextLevel + "', timeTotalForNextLevel ='" + timeTotalForNextLevel + "' where SkillID =" + skillID;
                    SQLiteCommand cmd = new SQLiteCommand(SkillUpdate, ECMDB);
                    cmd1.ExecuteNonQuery();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception) { }
            }
            ECMDB.Close();
        }

        private void writeKills(string API, int KEY, int USER)
        {
            //List<string> _kills = new List<string>();
            EveApi api = new EveApi(CORP_KEY, CORP_API, USER);
            List<KillMail> kill = api.GetCharacterKillMails();
            SQLiteConnection ECMDB = new SQLiteConnection("Data Source=" + ECMDBlocation);
            ECMDB.Open();
            foreach (KillMail pow in kill)
            {
                try
                {
                    string killID = Convert.ToString(pow.KillID);
                    string killTime = Convert.ToString(pow.KillTime);
                    string killTimeLocalTime = Convert.ToString(pow.KillTimeLocalTime);
                    string attackers = Convert.ToString(pow.Attackers);
                    string destroyedItems = Convert.ToString(pow.DestroyedItems);
                    string moon = Convert.ToString(pow.Moon);
                    string moonID = Convert.ToString(pow.MoonID);
                    string solarSystem = Convert.ToString(pow.SolarSystem);
                    string solarSystemID = Convert.ToString(pow.SolarSystemID);
                    string victim = Convert.ToString(pow.Victim);
                    string sql = "insert into Char_" + USER + "_kills (killID, killTime, killTimeLocalTime, attackers, destroyedItems, moon, moonID, solarSystem, SolarSystemID, victim) values ('" + killID + "', '" + killTime + "', '" + killTimeLocalTime + "', '" + attackers + "', '" + destroyedItems + "'. '" + moon + "', '" +moonID + "', '"+ solarSystem + "', '" + solarSystemID + "', '" + victim + "')";
                }
                catch { }
            }
            ECMDB.Close();
        }


        #endregion

        #region Character Data returns, Pulls data for display on the main form

               
         public string[] returnCharData(string selectChar)
        {
            string[] charData = new string[10];
            string sql = "select charID, Name, SkillinTraining, SkillLevel, SkillEndTime, TSP, CloneGrade, LastLocation, ShipType, ShipName from CharacterInfo Where Name LIKE '" + selectChar + "'";
            SQLiteConnection ECMDB = new SQLiteConnection("Data Source=" + ECMDBlocation);
            SQLiteCommand cmd = new SQLiteCommand(sql, ECMDB);
            ECMDB.Open();
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                charData[0] = Convert.ToString(reader["charID"]);
                charData[1] = Convert.ToString(reader["Name"]);
                charData[2] = Convert.ToString(reader["SkillinTraining"]); //Skill info
                charData[3] = Convert.ToString(reader["SkillLevel"]);
                charData[4] = Convert.ToString(reader["SkillEndTime"]); //Training End Local Time
                charData[5] = Convert.ToString(string.Format("{0:n0}", reader["TSP"]));
                charData[6] = Convert.ToString(reader["CloneGrade"]);
                charData[7] = Convert.ToString(reader["LastLocation"]);
                charData[8] = Convert.ToString(reader["ShipType"]);
                charData[9] = Convert.ToString(reader["ShipName"]);
            }
            return charData;
        }

         
        
        #endregion

    }



}
