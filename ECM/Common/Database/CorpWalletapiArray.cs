using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveAI.Live;
using EveAI.Live.Corporation;
using EveAI.Live.Character;
using System.Data.SQLite;

namespace ECM
{
    class CorpWalletapiArray
    {
        Common.Database.dbconn dbconn = new Common.Database.dbconn();
        /*
        //wallet balances formatted in a list ***Unused***
        public List<string> CorporateAccounts(int CORP_KEY, string CORP_API, int USER)
        {
            List<string> _str = new List<string>();
            EveApi api = new EveApi(CORP_KEY, CORP_API, USER);
            List<AccountBalance> caccount = api.GetCorporationAccountBalance();
            foreach (var line in caccount)
            {
               // everything after that dash in the output
                string apiString = line.ToString();
                string[] tokens = apiString.Split(' ');
                _str.Add(tokens[2]);
               
            }
            return _str;
        }

        */

        
        //wallet names formatted ***Needed***
        public List<string> wallet(int CORP_KEY, string CORP_API, int USER)
        {
            List<string> _wallet = new List<string>();
            EveApi api = new EveApi(CORP_KEY, CORP_API, USER);
            List<CorporationAccount> wn = api.GetCorporationSheet().Wallets;
            foreach (var line in wn)
            {
                // everything after that colon in the output
                string apiString = line.ToString();
                string[] tokens = apiString.Split(':');
                _wallet.Add(tokens[1]);
            }
            return _wallet;
        }

        //Wallet ID's put into list form
        public List<string> walletID(int CORP_KEY, string CORP_API, int USER)
        {
            List<string> _wallet = new List<string>();
            EveApi api = new EveApi(CORP_KEY, CORP_API, USER);
            List<CorporationAccount> wn = api.GetCorporationSheet().Wallets;
            foreach (var line in wn)
            {
                // everything before that colon in the output
                string apiString = line.ToString();
                string[] tokens = apiString.Split(':');
                _wallet.Add(tokens[0]);
            }
            return _wallet;
        }

        //Corp Journal Entries
        public List<JournalEntry> JournalEntry(int CORP_KEY, string CORP_API, int USER, int CW)
        {
            
            List<JournalEntry> _lst = new List<JournalEntry>();
            EveApi api = new EveApi(CORP_KEY, CORP_API, USER, CW);
            List<JournalEntry> je = api.GetCorporationWalletJournal();
            foreach (var line in je)
            {
                _lst.Add(line);
            }
            return _lst;
        }
        //Corp Transaction Entries
        public List<TransactionEntry> TransactionEntry(int CORP_KEY, string CORP_API, int USER, int CW)
        {
            List<TransactionEntry> _lst = new List<TransactionEntry>();
            EveApi api = new EveApi(CORP_KEY, CORP_API, USER, CW);
            List<TransactionEntry> te = api.GetCorporationWalletTransactions();
            foreach (var line in te)
            {
                _lst.Add(line);
            }
            return _lst;
        }
        
        
        
        
        

        //Corp Contracts go here


        //Corp Market Orders go here


        #region Test Region

        /*
        public List<JournalEntry> JournalEntryBuilder1(int CORP_KEY, string CORP_API, int USER, int CW, int CORP_USER)
        {
            SQLiteConnection ECMDB = new SQLiteConnection("Data Source=" + ECMDBlocation);
            List<JournalEntry> _lst = new List<JournalEntry>();
            EveApi api = new EveApi(CORP_KEY, CORP_API, USER, CW);
            List<JournalEntry> je = api.GetCorporationWalletJournal();
            ECMDB.Open();
            string sqlscalar = "Select Count(*) from Corp_" + CORP_USER + "_Wallet_Journal";
            SQLiteCommand cmdscalar = new SQLiteCommand(sqlscalar, ECMDB);
            int records = Convert.ToInt32(cmdscalar.ExecuteScalar());
            foreach (var line in je)
            {
                _lst.Add(line);
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
            ECMDB.Close();
            return _lst;
        }
         * 
         * 
         * 
        public List<TransactionEntry> TransactionEntryBuilder1(int CORP_KEY, string CORP_API, int USER, int CW, int CORP_USER)
        {
            SQLiteConnection ECMDB = new SQLiteConnection("Data Source=" + ECMDBlocation);
            List<TransactionEntry> _lst = new List<TransactionEntry>();
            EveApi api = new EveApi(CORP_KEY, CORP_API, USER, CW);
            List<TransactionEntry> te = api.GetCorporationWalletTransactions();
            ECMDB.Open();
            string sqlscalar = "Select Count(*) from Corp_" + CORP_USER + "_Wallet_Transactions";
            SQLiteCommand cmdscalar = new SQLiteCommand(sqlscalar, ECMDB);
            int records = Convert.ToInt32(cmdscalar.ExecuteScalar());
            foreach (var line in te)
            {
                _lst.Add(line);
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
            ECMDB.Close();
            return _lst;
        }
         * 
         * 
         * 
         * 
         * 
        */
        #endregion



    }
}
