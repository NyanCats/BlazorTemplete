using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlazorTemplate.Shared.WebApis.Accounts
{
    public class CreateUserResult
    {
        public string Password { get; set; } = "";

        public CreateUserResult(){ }

        public CreateUserResult(string password)
        {
            Password = password;
        }
    }
}
