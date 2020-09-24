using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlazorTemplate.Shared.WebApis.Accounts
{
    public class ValidateAccountRequest : IApiRequest
    {
        // TODO: write validations
        [Required]
        public string UserName { get; protected set; }

        [Required]
        public string Password { get; protected set; }

        public ValidateAccountRequest(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }
}
