using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorTemplate.Commons.WebApis.Sessions
{
    public class LoginResult
    {
        public List<string> Errors { get; set; }

        public LoginResult()
        {
            Errors = new List<string>();
        }

        public LoginResult(List<string> errors = null)
        {
            Errors = errors;
        }
    }
}
