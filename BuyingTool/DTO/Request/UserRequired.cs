using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuyingTool.DTO.Request
{
    public class UserRequired
    {
        public string Id_Number { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}