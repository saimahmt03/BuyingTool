using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace BuyingTool.Shared
{
    public class AppSettings
    {
        public static string DatabaseProvider;
        public static string ErrorLogPath => ConfigurationManager.AppSettings["ErrorLogPath"];
    }
}