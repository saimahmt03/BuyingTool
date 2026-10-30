using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BuyingTool.Services;
using System.Data;

namespace BuyingTool.Controllers
{
    public class OrganizationFiltersController : ApiController
    {
        // GET: api/RequiredFilters
        public DataTable Get(string idnumber)
        {
            Service service = new Service();
            DataTable dataSet = service.GetFiltersOrganizationCache(idnumber).GetAwaiter().GetResult();
            return dataSet;
        }
    }
}
