using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlazorTemplate.Commons.WebApis.Accounts
{
    public class CreateUserResult
    {
        public string Password { get; set; }
        public List<string> Errors { get; set; }

        public CreateUserResult()
        {
            Errors = new List<string>();
        }

        public CreateUserResult(string password, List<string> errors = null)
        {
            Password = password;
            Errors = errors;
        }
    }
}
