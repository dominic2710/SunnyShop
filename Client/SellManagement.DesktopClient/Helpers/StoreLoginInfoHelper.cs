using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SellManagement.DesktopClient.Helpers
{
    public static class StoreLoginInfoHelper
    {
        const string STORED_TOKEN_LOCATION = "stored";
        public static string GetStoredToken()
        {
            try
            {
                return File.ReadAllText(STORED_TOKEN_LOCATION);
            }
            catch { 
                return "";
            }
        }

        public static void SetStoredToken(string token)
        {
            try
            {
                File.WriteAllText(STORED_TOKEN_LOCATION, token);
            }
            catch { }
        }
    }
}
