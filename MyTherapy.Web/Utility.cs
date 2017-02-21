using System.Configuration;

namespace MyTherapy.Web
{
    public class Utility
    {
        const string ConnString = "MyTherapyDB";
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