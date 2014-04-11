using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace EVE_corp_manager_V2.Common.Database
{
    class dbconn
    {
        SQLiteConnection ECMDB;
        string DBlocation = Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\AppData\Local\EVECM\db\EVECM.db");


        #region Creates the directory tree, database, and base tables.
        int dbexist = 0;
        /// <summary>
        /// Creates the database and directories
        /// </summary>
        public void CreateDB()
        {
           
            if (File.Exists(DBlocation) == true)
            { 
                dbexist = 1;
            }
            else
            {
                try
                {
                    SQLiteConnection.CreateFile(DBlocation);
                }
                catch (Exception) { }
            }
        }
        /// <summary>
        /// Builds the basic tables for the database
        /// </summary>
        public void TableBuilder()
        {
            
            if (dbexist == 0)
            {
                try
                {
                    ECMDB = new SQLiteConnection("Data Source=" + DBlocation);
                    ECMDB.Open();
                    string sql = "create table CharacterInfo (KEY INTEGER, API varchar(64), CharID INTEGER PRIMARY KEY, Name varchar(64), CorpID int, SkillinTraining varchar(64), SkillLevel INT, SkillEndTime char, Keyexp char, Acctexp char, TSP int, CloneGrade char, LastLocation char, ShipType char, ShipName char)";
                    SQLiteCommand cmd = new SQLiteCommand(sql, ECMDB);
                    cmd.ExecuteNonQuery();
                    ECMDB.Close();
                }
                catch (SQLiteException ex) 
                {
                    MessageBox.Show(Convert.ToString(ex));
                }
                try
                {
                    ECMDB = new SQLiteConnection("Data Source=" + DBlocation);
                    ECMDB.Open();
                    string sql = "create table CorporationInfo (CORPKEY INTEGER, CORPAPI varchar(64), CorpUser INTEGER, CorpName varchar(64), CorpID int, CEOName char,  CEOId int, AllianceName char, AllianceID int, Faction char, FactionID int, Members int, MemberLimit int, Keyexp char, Wallet1Name char, Wallet1ID int, Wallet2Name char, Wallet2ID int, Wallet3Name char, Wallet3ID int, Wallet4Name char, Wallet4ID int, Wallet5Name char, Wallet5ID int, Wallet6Name char, Wallet6ID int, Wallet7Name char, Wallet7ID int, AD1Name char, AD1ID int, AD2Name char, AD2ID int, AD3Name char, AD3ID int, AD4Name char, AD4ID int, AD5Name char, AD5ID int, AD6Name char, AD6ID int, AD7Name char, AD7ID int)";
                    SQLiteCommand cmd = new SQLiteCommand(sql, ECMDB);
                    cmd.ExecuteNonQuery();
                    ECMDB.Close();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(Convert.ToString(ex));
                }
            }
        }

        

        #endregion

        #region Conversions required by the database
        /// <summary>
        /// Converts DateTime to a string for the database.
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public string datetimestring(DateTime datetime)
        {
            string datetimeformat = "{0}-{1}-{2} {3}:{4}:{5} ";
            return string.Format(datetimeformat, datetime.Year, datetime.Month, datetime.Day, datetime.Hour, datetime.Minute, datetime.Second);
        }
        /// <summary>
        /// Converts a DateTime into a more normal format
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public string datetimestandard(DateTime datetime)
        {
            string datetimeformat = "{0}/{1}/{2} {3}:{4}:{5} ";
            return string.Format(datetimeformat, datetime.Year, datetime.Month, datetime.Day, datetime.Hour, datetime.Minute, datetime.Second);
        }

        /// <summary>
        /// Converts the timespan to a string for the DB.
        /// </summary>
        /// <param name="timeStringConversion"></param>
        /// <returns></returns>
        public string SkillTimeConverter(TimeSpan timeStringConversion)
        {
            string datetimeformat = "{0}:{1}:{2}:{3} ";
            return string.Format(datetimeformat, timeStringConversion.Days, timeStringConversion.Hours, timeStringConversion.Minutes, timeStringConversion.Seconds);
        }
        /// <summary>
        /// Escapes apostrophe's in the SQL string and returns the escaped string
        /// </summary>
        /// <param name="noApoph"></param>
        /// <returns></returns>
        public string stringApoph(string noApoph)
        {
            string apoph = "'";
            if (noApoph.Contains(apoph) == true)
            {
                string fixedEscape = noApoph.Replace("'", "''");
                noApoph = fixedEscape;
            }
            return noApoph;
        }

        #endregion
    }
}
