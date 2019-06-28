using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BlazorTemplate.Shared.WebApis.Accounts
{
    public class CreateUserRequest
    {
        [Display(Name = "同意")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "利用規約への同意が必要です。")]
        [CustomValidation(typeof(CreateUserRequest), "Validate_Agreed001")]
        public bool Agreed { get; set; } = false;

        /// <summary>
        /// 利用規約への同意を検証します。
        /// </summary>
        public static ValidationResult Validate_Agreed001(string value, ValidationContext context)
        {
            var model = (CreateUserRequest)context.ObjectInstance;
            if (!model.Agreed) return new ValidationResult("利用規約への同意が必要です。");
            return ValidationResult.Success;
        }

        [Display(Name = "ユーザー名")]
        [Required]
        [MinLength(4)]
        [MaxLength(16)]
        [CustomValidation(typeof(CreateUserRequest), "Validate_Name001")]
        [DataType(DataType.Text)]
        public string UserName { get; set; } = null;

        /// <summary>
        /// CreateUserRequestのプロパティUserNameに使用できない文字が含まれていないかを検証します。
        /// </summary>
        public static ValidationResult Validate_Name001(string value, ValidationContext context)
        {
            var model = (CreateUserRequest)context.ObjectInstance;

            if (model.UserName == null) return ValidationResult.Success;

            var isValid = Regex.IsMatch(model.UserName, @"^[0-9a-zA-Z_]*$");

            if (isValid) return ValidationResult.Success;
            return new ValidationResult("使用できない文字が含まれています。");
        }
    }
}
