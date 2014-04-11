using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveAI.Live;
using EveAI.Live.Character;
using EveAI.Live.Account;
using System.Xml;
using System.IO;

namespace ECM
{
    class apiArrays
    {

        #region APIkeys

        //API key info pulls the key type as Account, Corporation, or Character.
        public string keyinfo(string API, int KEY)
        {
            string _str = null;
            EveApi api = new EveApi(KEY, API);
            var keytype = api.getApiKeyInfo().KeyType.ToString();
            _str = keytype;
            return _str;
        }

        public string Keyexp(string API, int KEY)
        {
            string _str = null;
            EveApi api = new EveApi(KEY, API);
            var keyexp = api.getApiKeyInfo().Expires.ToString();
            _str = keyexp;
            return _str;
        }

        //Returns the Character name and Corporation
        public List<AccountEntry> new_user(string API, int KEY)
        {
            List<AccountEntry> _charid = new List<AccountEntry>();
            EveApi api = new EveApi(KEY, API);
            List<AccountEntry> charid = api.getApiKeyInfo().Characters;
            
            foreach (var c in charid)
            {
                _charid.Add(c);
            }
            return _charid;
        }

        //Returns the Character ID from the XML file "APIKeyInfoApi." + KEY + ".0.xml" located in the cache directory.
        public int ReturnCharID(string fname)
        {
            XmlReader reader = XmlReader.Create(Environment.ExpandEnvironmentVariables(@"%USERPROFILE%") + @"\AppData\Local\EVECM\cache\" + fname);
            int myID = 0;
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "row")
                {
                   myID = Convert.ToInt32(reader.GetAttribute(0));
                }
            }
            return myID;
        }

        //Returns Account status
        public string ACStatus(string API, int KEY)
        {
            string _str = null;
            EveApi api = new EveApi(KEY, API);
            var _ACStatus = api.GetAccountStatus().PaidUntil.ToString();
            _str = _ACStatus;
            return Convert.ToString(_str);
        }

        #endregion

        #region Character sheet

        //Character info
        public CharacterInfo CInfo(string API, int KEY, int USER)
        {
            CharacterInfo _CInfo = new CharacterInfo();
            EveApi api = new EveApi(KEY, API, USER);
            CharacterInfo cinfo = api.GetCharacterInfo();
            return cinfo;
        }

        //Character Sheet
      
        public CharacterSheet CSheet(string API, int KEY, int USER)
        {
           
                CharacterSheet CharSheet = new CharacterSheet();
                EveApi api = new EveApi(KEY, API, USER);
                CharacterSheet csname = api.GetCharacterSheet();
                return csname;
        }
   
        //Employment History
        public List<CharacterEmploymentHistory> CharEmployment(string API, int KEY, int USER)
        {
            List<CharacterEmploymentHistory> _emp = new List<CharacterEmploymentHistory>();
            EveApi api = new EveApi(KEY, API, USER);
            List<CharacterEmploymentHistory> emp = api.GetCharacterInfo().EmploymentHistory;
            foreach (CharacterEmploymentHistory job in emp)
            {
                _emp.Add(job);
            }
            return _emp;
        }

        //Get Character skills        
        public CharacterSheet Cskill(string API, int KEY, int USER)
        {
            CharacterSheet _skill = new CharacterSheet();
            EveApi api = new EveApi(KEY, API, USER);
            CharacterSheet charskills = api.GetCharacterSheet();
            foreach (CharacterSheet.LearnedSkill LS in charskills.Skills)
            {
                _skill.Skills.Add(LS);
            }
            return _skill;
        }

        public SkillInTraining skillintraining(string API, int KEY, int USER)
        {
            SkillInTraining _skill = new SkillInTraining();
            EveApi api = new EveApi(KEY, API, USER);
            SkillInTraining skill = api.GetCharacterSkillInTraining();
            return skill;
        }


        public List<string> CharKillLog(int CORP_KEY, string CORP_API, int USER)
        {
            List<string> _kills = new List<string>();
            EveApi api = new EveApi(CORP_KEY, CORP_API, USER);
            List<KillMail> kill = api.GetCharacterKillMails();
            foreach (KillMail pow in kill)
            {
                _kills.Add(Convert.ToString(pow));
            }
            return _kills;
        }

        #endregion

        #region Test Region
        
        /*
        //return Character Journal Entries
        public List<JournalEntry> JournalEntry(string API, int KEY, int USER)
        {
            List<JournalEntry> _lst = new List<JournalEntry>();
            EveApi api = new EveApi(KEY, API, USER);
            List<JournalEntry> je = api.GetCharacterWalletJournal();
            foreach (var line in je)
            {
                _lst.Add(line);
            }
            return _lst;
        }
         * 
        //Return Character assets
        public List<Asset> Asset(string API, int KEY, int USER)
        {
            List<Asset> _lst = new List<Asset>();
            EveApi api = new EveApi(KEY, API, USER);
            List<Asset> ass = api.GetCharacterAssets();
            foreach (var line in ass)
            {
                _lst.Add(line);
            }
            return _lst;
        }

        //Return Characters Corporation
        public CharacterSheet CPN(string API, int KEY, int USER)
        {
            CharacterSheet charsheet = new CharacterSheet();
            EveApi api = new EveApi(KEY, API, USER);
            CharacterSheet cpname = api.GetCharacterSheet();
            return cpname;
        }
         * 
        //Return Characters Name
        public CharacterSheet CS(string API, int KEY, int USER)
        {
            CharacterSheet CharSheet = new CharacterSheet();
            EveApi api = new EveApi(KEY, API, USER);
            CharacterSheet csname = api.GetCharacterSheet();
            return csname;
        }
         * 
        //Return Character Skillpoint Totals
        public CharacterSheet CSP(string API, int KEY, int USER)
        {
            CharacterSheet CharSkilliT = new CharacterSheet();
            EveApi api = new EveApi(KEY, API, USER);
            CharacterSheet csit = api.GetCharacterSheet();
           
            return csit;
        }

        //return Corp ID
        public CharacterSheet CorpID(string API, int KEY, int USER)
        {
            CharacterSheet _CorpID = new CharacterSheet();
            EveApi api = new EveApi(KEY, API, USER);
            CharacterSheet CID = api.GetCharacterSheet();
            return CID;
        }
         * 
        public string arraytostring(System.Collections.ArrayList ar)
        {
            StringBuilder sb = new StringBuilder();
            System.Xml.XmlWriterSettings st = new System.Xml.XmlWriterSettings();
            st.OmitXmlDeclaration = true;
            st.Indent = false;
            System.Xml.XmlWriter w = System.Xml.XmlWriter.Create(sb, st);
            System.Xml.Serialization.XmlSerializer s = new System.Xml.Serialization.XmlSerializer(ar.GetType());
            s.Serialize(w, ar);
            w.Close();
            return sb.ToString();
        }
        public static void SaveArraytoFile(System.Collections.ArrayList ar, string filename)
        {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(filename))
            {
                foreach (var item in ar)
                {
                    sw.WriteLine(item);
                }
            }
        }
         */
        #endregion 
    }
}


