using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlazorTemplate.Shared.WebApis.Accounts
{
    public class ValidateRequest
    {
        // TODO: write validations
        [Required]
        public string UserName { get; protected set; }

        [Required]
        public string Password { get; protected set; }

        public ValidateRequest(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }
}
