using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace EVE_corp_manager_V2.Common.onload
{
    class onload
    {

        public void Directorybuilder()
        {
            try
            {
                if (Directory.Exists(Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\appdata\local\EVECM\")) == false)
                {
                    Directory.CreateDirectory(Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\appdata\local\EVECM\"));
                }
            }
            catch (Exception) { }
            try
            {
                if (Directory.Exists(Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\appdata\local\EVECM\Cache\")) == false)
                {
                    Directory.CreateDirectory(Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\appdata\local\EVECM\Cache\"));
                }
            }
            catch (Exception) { }
            try
            {
                if (Directory.Exists(Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\AppData\Local\EVECM\db\")) == false)
                {
                    Directory.CreateDirectory(Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\AppData\Local\EVECM\db\"));
                }
            }
            catch (Exception) { }
            try
            {
                if (Directory.Exists(Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\AppData\Local\EVECM\images\")) == false)
                {
                    Directory.CreateDirectory(Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\AppData\Local\EVECM\images\"));
                }
            }
            catch (Exception) { }
        }
    }
}
