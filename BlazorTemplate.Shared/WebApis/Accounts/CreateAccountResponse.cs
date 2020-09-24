using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlazorTemplate.Shared.WebApis.Accounts
{
    public class CreateAccountResponse : IApiResponse
    {
        public string Password { get; set; }

        public CreateAccountResponse(string password = default)
        {
            Password = password;
        }
    }
}
