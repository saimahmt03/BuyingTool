using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BuyingTool.Services;
using BuyingTool.DTO.Request;
using BuyingTool.Repository;
using BuyingTool.Data;
using System.Threading.Tasks;

namespace BuyingTool.Services
{
    public class UserAccountService
    {
        // USER ACCOUNT ONLY
        private readonly Service _service = new Service();

        public string ValidateUserAccount(UserRequired user)
        {
            string response = "Proceed";

            if (string.IsNullOrEmpty(user.Id_Number) || string.IsNullOrEmpty(user.Password))
            {
                response = "ID Number or Password is empty. Pls try again.";
            }
            else if (string.IsNullOrWhiteSpace(user.Id_Number) || string.IsNullOrWhiteSpace(user.Password))
            {
                response = "ID Number or Password is empty. Pls try again.";
            }

            return response;
        }

        public string ValidateUser(UserRequired user)
        {
            string response = ValidateUserAccount(user);

            if (response == "Proceed")
            {
                bool validated = _service.CheckMainTableForLogin(user.Id_Number, user.Password);

                if (validated)
                {
                    return user.Id_Number;
                }
                else
                {
                    return "User does not exist, please try again.";
                }
            }

            return response;
        }
    }
}