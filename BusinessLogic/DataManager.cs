using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyTherapy.BusinessLogic
{
    public class DataManager
    {
        public static bool TestConnection(string connString)
        {
            if (string.IsNullOrEmpty(connString))
                return false;

            MySqlConnection dbConn = new MySqlConnection(connString);

            MySqlCommand cmd = dbConn.CreateCommand();
            cmd.CommandText = "SELECT * from user";
            try
            {
                dbConn.Open();
            }
            catch (Exception e)
            {
            }
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string a = reader.ToString();
            }

            return true;
        }
    }
}