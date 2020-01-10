using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlazorTemplate.Shared.WebApis.Accounts
{
    public class CreateAccountResult
    {
        public string Password { get; set; } = "";

        public CreateAccountResult(){ }

        public CreateAccountResult(string password)
        {
            Password = password;
        }
    }
}
