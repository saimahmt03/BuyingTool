using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BuyingTool.Controllers
{
    public class SetFiltersController : ApiController
    {
        // GET: api/SetFilters
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/SetFilters/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/SetFilters
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/SetFilters/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/SetFilters/5
        public void Delete(int id)
        {
        }
    }
}
