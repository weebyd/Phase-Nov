using GaragesManagement.Database;
using GaragesManagement.Models;
using GaragesManagement.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GaragesManagement.Controllers
{
    public class AccountController : ApiController
    {
        [HttpOptions, HttpPost]
        public ResponseModel Login([FromBody] dynamic input)
        {
            var username = input.username.Value;
            var password = input.password.Value;
            try
            {
                int response = -99;
                var acc = AccountDAO.Instance.Login(username, password, out response);

                switch (response)
                {
                    case 0:
                        return new ResponseModel
                        {
                            ResponseCode = -1,
                            ResponseData = null,
                            Message = "Wrong username or password, please try again ! !"
                        };
                    case 1:
                        return new ResponseModel
                        {
                            ResponseCode = 1,
                            ResponseData = acc,
                            Message = "Login Successful !"
                        };
                    default:
                        return new ResponseModel
                        {
                            ResponseCode = -99,
                            ResponseData = null,
                            Message = "Encountered an unknown error, please try again later !"
                        };
                }
            }
            catch (Exception ex)
            {
                // TODO: implement ErrLog

                return new ResponseModel
                {
                    ResponseCode = -99,
                    ResponseData = null,
                    Message = "Encountered an unknown error, please try again later !"
                };
            }
        }
    }
}
