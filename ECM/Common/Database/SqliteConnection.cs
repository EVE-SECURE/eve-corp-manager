 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;

namespace EVE_corp_manager_V2.Common.Database
{
    class SqliteConnection
    {
        private SQLiteConnection sqlite;

        public SqliteConnection()
        {
            string datasource = Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\AppData\Local\EVECM\db\EVECM.db");
            sqlite = new SQLiteConnection(datasource);

        }

        public DataTable selectQuery(string query)
        {
            SQLiteDataAdapter ad;
            DataTable dt = new DataTable();

            try
            {
                SQLiteCommand cmd;
                sqlite.Open();  //Initiate connection to the db
                cmd = sqlite.CreateCommand();
                cmd.CommandText = query;  //set the passed query
                ad = new SQLiteDataAdapter(cmd);
                ad.Fill(dt); //fill the datasource
            }
            catch (SQLiteException)
            {
                //Add your exception code here.
            }
            sqlite.Close();
            return dt;
        }
    }
}
