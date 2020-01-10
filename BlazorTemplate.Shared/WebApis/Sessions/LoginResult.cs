using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace BlazorTemplate.Shared.WebApis.Sessions
{
    public class LoginResult
    {
        public string Token { get; set; }
    }
}
