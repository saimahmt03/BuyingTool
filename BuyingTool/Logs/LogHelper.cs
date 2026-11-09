using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BuyingTool.Shared;
using System.IO;

namespace BuyingTool.Logs
{
    public class LogHelper
    {
        public static void LogError(string message, Exception ex)
        {
            string logPath = AppSettings.ErrorLogPath;

            if (string.IsNullOrEmpty(logPath))
            {
                logPath = @"C:\Log\BuyingTool_LogErrors.txt";
            }

            string logMessage = string.Concat(
                DateTime.UtcNow,
                ": ",
                message,
                " ",
                ex.Message,
                Environment.NewLine);

            File.AppendAllText(logPath, logMessage);
        }
    }
}