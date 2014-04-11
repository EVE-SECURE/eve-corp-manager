using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveAI.Live;
using EveAI.Live.Corporation;
using EveAI.Live.Character;
using System.Xml;
using System.Xml.Linq;
using System.IO;




namespace ECM
{
    class CorpApiArrays
    {

        public CorporationSheet CorpSheet(int CORP_KEY, string CORP_API, int USER)
        {
            CorporationSheet _CScount = new CorporationSheet();
            EveApi api = new EveApi( CORP_KEY, CORP_API, USER);
            CorporationSheet CScount = api.GetCorporationSheet();
            return CScount;
        }
        
        public List<string> mems(int CORP_KEY, string CORP_API, int USER)
        {
            List<string> _mems = new List<string>();
            EveApi api = new EveApi(CORP_KEY, CORP_API, USER);
            List<MemberTrackingEntry> memcount = api.GetCorporationMemberTracking();
            foreach (var line in memcount)
            {
                //split off the effing dash
                string apiString = line.ToString();
                string[] names = apiString.Split('-');
                _mems.Add(names[0]);
            }
            return _mems;
        }
        /// <summary>
        /// Asset Divisions. Returns the names and ID's of the Corporation Hangars.
        /// </summary>
        /// <param name="CORP_KEY"></param>
        /// <param name="CORP_API"></param>
        /// <param name="USER"></param>
        /// <returns></returns>
        public List<string> AssetDivisionsString(int CORP_KEY, string CORP_API, int USER)
        {
            List<string> _asset = new List<string>();
            EveApi api = new EveApi(CORP_KEY, CORP_API, USER);
            List<CorporationAccount> wn = api.GetCorporationSheet().Divisions;
            foreach (var line in wn)
            {
                // everything after that colon in the output
                string apiString = line.ToString();
                string[] tokens = apiString.Split(':');
                _asset.Add(tokens[1]);
            }
            return _asset;
        }

        public List<string> AssetDivisionsID(int CORP_KEY, string CORP_API, int USER)
        {
            List<string> _asset = new List<string>();
            EveApi api = new EveApi(CORP_KEY, CORP_API, USER);
            List<CorporationAccount> wn = api.GetCorporationSheet().Divisions;
            foreach (var line in wn)
            {
                // everything after that colon in the output
                string apiString = line.ToString();
                string[] tokens = apiString.Split(':');
                _asset.Add(tokens[0]);
            }
            return _asset;
        }
        
        //Returns the Corporation ID from the XML file "APIKeyInfoApi." + KEY + ".0.xml" located in the cache directory.
        public int ReturnCharID(string cfname)
        {
            XmlReader reader = XmlReader.Create(Environment.ExpandEnvironmentVariables(@"%USERPROFILE%") + @"\AppData\Local\EVECM\cache\" + cfname);
            int myID = 0;
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "row")
                {
                    myID = Convert.ToInt32(reader.GetAttribute(2));
                }
            }
            return myID;
        }

        


         
    }
}
