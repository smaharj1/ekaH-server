using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace ekaH_server.Models.UserModels
{
    public class UserWorker
    {
        public static string getStringSafe(MySqlDataReader reader, int index)
        {
            if (!reader.IsDBNull(index))
            {
                return reader.GetString(index);
            }
            else
            {
                return "";
            }
        }

        
    }
}