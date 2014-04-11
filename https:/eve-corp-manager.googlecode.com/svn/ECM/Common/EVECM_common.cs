using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using EveAI.Live.Corporation;
using EveAI.Live.Character;
using EveAI.Live;
using EveAI.Live.Generic;

namespace EVE_corp_manager_V2
{
    class EVECM_common
    {

        apiArrays aa = new apiArrays();
        CorpApiArrays caa = new CorpApiArrays();
        CorpWalletapiArray cwaa = new CorpWalletapiArray();

        public ServerStatus ServStatus()
        {
            ServerStatus _Servstatus = new ServerStatus();
            EveApi api = new EveApi();
            ServerStatus serverstatus = api.GetServerStatus();
            return serverstatus;
        }
    }
}
