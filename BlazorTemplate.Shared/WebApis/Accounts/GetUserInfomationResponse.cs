using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorTemplate.Shared.WebApis.Accounts
{
    public class GetUserInfomationResponse : IApiResponse
    {
        public string UserName { get; set; }

        public GetUserInfomationResponse() { }

        public GetUserInfomationResponse(string userName)
        {
            UserName = userName;
        }
    }
}
