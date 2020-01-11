using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorTemplate.Shared.WebApis.Accounts
{
    public class GetUserInfomationResult
    {
        public string UserName { get; set; }

        public GetUserInfomationResult() { }

        public GetUserInfomationResult(string userName)
        {
            UserName = userName;
        }
    }
}
