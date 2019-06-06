using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorTemplate.Shared.WebApis.Accounts
{
    public class UserInfomationResult
    {
        public string UserName { get; set; }

        public UserInfomationResult()
        {

        }

        public UserInfomationResult(string userName)
        {
            UserName = userName;
        }
    }
}
