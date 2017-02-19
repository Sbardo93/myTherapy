using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MyTherapy
{
    public class Utility
    {
        const string ConnString = "ConnString";
        public static string GetConnectionString
        {
            get
            {
                string value = string.Empty;
                if (ConfigurationManager.ConnectionStrings[ConnString] != null)
                    value = ConfigurationManager.ConnectionStrings[ConnString].ToString();
                return value;
            }
        }
    }
}