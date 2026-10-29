using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BuyingTool.DTO.Request;

namespace BuyingTool.Services
{
    public class ServiceValidator
    {

        public bool ValidateFilters(string storegrade, string size)
        {
            if(string.IsNullOrEmpty(storegrade) && string.IsNullOrEmpty(size))
            {
                return false;
            }

            if(string.IsNullOrWhiteSpace(storegrade) && string.IsNullOrWhiteSpace(storegrade))
            {
                return false;
            }

            return true;
        }

        public bool ValidateFiltersValue(string storegrade, string size)
        {
            if (storegrade == "-- Select --" && size == "-- Select --")
            {
                return true;
            }

            return false;
        }


        public string ValidateFilename(string filename)
        {

            bool hasUnderscore = filename.Contains('_');

            if (hasUnderscore)
            {
                return "Proceed";
            }

            return "Failed, please check the filename input";
        }
    }
}