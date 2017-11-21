using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace GaragesManagement.Models
{
    public class Config
    {
        public static string GaragesConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["GaragesConnectionString"].ConnectionString;
            }
        }
    }
}