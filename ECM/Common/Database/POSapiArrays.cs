using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveAI.Live;
using EveAI.Live.Corporation;
using EveAI.SpaceStation;
using EveAI.Planetary;

namespace ECM
{
    class POSapiArrays
    {
        public List<StarbaseListEntry> CorpStarBaseList(int CORP_KEY, string CORP_API)
        {
            List<StarbaseListEntry> _SBlist = new List<StarbaseListEntry>();
            EveApi api = new EveApi(CORP_KEY, CORP_API);
            List<StarbaseListEntry> SBlist = api.GetCorporationStarbaseList();
            foreach (var s in SBlist)
            {
                _SBlist.Add(s);
            }
            return _SBlist;
        }

        

         public Starbase CorpStarbase(int CORP_KEY, string CORP_API, long ITEMID)
        {
            //Starbase _SB = new Starbase();
            EveApi api = new EveApi(CORP_KEY, CORP_API, ITEMID);
            Starbase _SBS = api.GetCorporationStarbaseDetail();
            return _SBS;
        }
    }
}
