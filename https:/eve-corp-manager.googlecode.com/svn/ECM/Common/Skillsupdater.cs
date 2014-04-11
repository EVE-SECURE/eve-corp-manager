using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Text.RegularExpressions;
using EveAI.Live.Character;
using EveAI.Live.Generic;
using EveAI.Live;

namespace EVE_corp_manager_V2.Common
{
    class Skillsupdater
    {
        apiArrays aa = new apiArrays();
        

        public void Skill_Loader(string API, int KEY, int USER)
        {
            foreach (var skill in aa.Cskill(API, KEY, USER).Skills)
            {
                SQLiteConnection testdb = new SQLiteConnection();
                string DBlocation = Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\AppData\Local\EVECM\db\EVECM.db");
                testdb = new SQLiteConnection("Data Source=" + DBlocation);
                var nam = Regex.Replace(skill.ToString(), @"[\d-]", string.Empty);
                var num = Regex.Replace(skill.ToString(), @"[^\d]", string.Empty);
                testdb.Open();
                string sql = "insert into Char_" + USER + "_skills (SkillName, SkillLevel) values ('" + nam + "', " + num + ")";
                SQLiteCommand cmd = new SQLiteCommand(sql, testdb);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
