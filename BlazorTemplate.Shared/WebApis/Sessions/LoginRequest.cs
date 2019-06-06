using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace BlazorTemplate.Shared.WebApis.Sessions
{
    public class LoginRequest
    {
        [Display(Name = "ユーザー名")]
        [Required]
        [MinLength(4)]
        [MaxLength(16)]
        [CustomValidation(typeof(LoginRequest), "Validate_Name001")]
        [DataType(DataType.Text)]
        public string UserName { get; set; } = null;

        /// <summary>
        /// LoginCommandのプロパティNameに使用できない文字が含まれていないかを検証します。
        /// </summary>
        public static ValidationResult Validate_Name001(string value, ValidationContext context)
        {
            var model = (LoginRequest)context.ObjectInstance;

            if (model.UserName == null) return ValidationResult.Success;

            var isValid = Regex.IsMatch(model.UserName, @"^[0-9a-zA-Z_]*$");

            if (isValid) return ValidationResult.Success;
            return new ValidationResult("使用できない文字が含まれています。");
        }

        [Display(Name = "パスワード")]
        [Required]
        [StringLength(32)]
        [DataType(DataType.Text)]
        public string Password { get; set; }
    }
}
