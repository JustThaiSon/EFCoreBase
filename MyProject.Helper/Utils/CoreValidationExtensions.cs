using FluentValidation;
using MyProject.Helper.Constants.Globals;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Text.RegularExpressions;

namespace MyProject.Helper.Utils
{
    public static class CoreValidationExtensions
    {
        private static string _langCode;

        public static void Initialize(IConfiguration configuration)
        {
            _langCode = configuration["ProjectSettings:LanguageCode"] ?? "vi";
        }

        public static IRuleBuilderOptionsConditions<T, R> NotNullOrEmpty<T, R>(this IRuleBuilder<T, R> ruleBuilder)
        {
            string msg = ResourceUtil.GetMessage((int)ResponseCodeEnum.VLD_REQUIRED, _langCode);

            return ruleBuilder.Custom((x, y) =>
            {
                if (x == null || string.IsNullOrEmpty(x.ToString()))
                    y.AddFailure(y.DisplayName, string.Format(msg, y.DisplayName));
            });
        }

        public static IRuleBuilderOptionsConditions<T, R> MaxLength<T, R>(this IRuleBuilder<T, R> ruleBuilder, int maxLength)
        {
            string msg = ResourceUtil.GetMessage((int)ResponseCodeEnum.VLD_MAX_LENGTH, _langCode);

            return ruleBuilder.Custom((x, y) =>
            {
                if (x != null && !string.IsNullOrEmpty(x.ToString()) && x.ToString().Length > maxLength)
                    y.AddFailure(y.DisplayName, string.Format(msg, maxLength));
            });
        }

        public static IRuleBuilderOptionsConditions<T, R> MinLength<T, R>(this IRuleBuilder<T, R> ruleBuilder, int minLength)
        {
            string msg = ResourceUtil.GetMessage((int)ResponseCodeEnum.VLD_MIN_LENGTH, _langCode);

            return ruleBuilder.Custom((x, y) =>
            {
                if (x != null && !string.IsNullOrEmpty(x.ToString()) && x.ToString().Length < minLength)
                    y.AddFailure(y.DisplayName, string.Format(msg, minLength));
            });
        }

        public static IRuleBuilderOptionsConditions<T, R> RangeLength<T, R>(this IRuleBuilder<T, R> ruleBuilder, int minLength, int maxLength)
        {
            string msg = ResourceUtil.GetMessage((int)ResponseCodeEnum.VLD_RANGE_LENGTH, _langCode);

            return ruleBuilder.Custom((x, y) =>
            {
                if (x != null && !string.IsNullOrEmpty(x.ToString()) && x.ToString().Length > minLength && x.ToString().Length < maxLength)
                    y.AddFailure(y.DisplayName, string.Format(msg, maxLength));
            });
        }

        public static IRuleBuilderOptionsConditions<T, R> NotNegative<T, R>(this IRuleBuilder<T, R> ruleBuilder)
        {
            string msg = ResourceUtil.GetMessage((int)ResponseCodeEnum.VLD_NOT_NEGATIVE, _langCode);

            return ruleBuilder.Custom((x, y) =>
            {
                if (x != null && (!int.TryParse(x.ToString(), out int val) || val < 0))
                {
                    y.AddFailure(y.DisplayName, msg);
                }
            });
        }
        public static IRuleBuilderOptionsConditions<T, R> GreaterThanZero<T, R>(this IRuleBuilder<T, R> ruleBuilder)
        {
            string msg = ResourceUtil.GetMessage((int)ResponseCodeEnum.VLD_GREATER_THAN_ZERO, _langCode);

            return ruleBuilder.Custom((value, context) =>
            {
                if (value != null && (!int.TryParse(value.ToString(), out int val) || val <= 0))
                {
                    context.AddFailure(context.DisplayName, msg);
                }
            });
        }
        public static IRuleBuilderOptionsConditions<T, R> MustBeInteger<T, R>(this IRuleBuilder<T, R> ruleBuilder)
        {
            string msg = ResourceUtil.GetMessage((int)ResponseCodeEnum.VLD_MUST_BE_INTEGER, _langCode);

            return ruleBuilder.Custom((value, context) =>
            {
                if (value == null || !int.TryParse(value.ToString(), out _))
                {
                    context.AddFailure(context.DisplayName, msg);
                }
            });
        }
        public static IRuleBuilderOptionsConditions<T, R> MustBeValidTimeFormat<T, R>(this IRuleBuilder<T, R> ruleBuilder)
        {
            string msg = ResourceUtil.GetMessage((int)ResponseCodeEnum.VLD_MUST_BE_VALID_TIME_FORMAT, _langCode);

            return ruleBuilder.Custom((value, context) =>
            {
                if (value == null || !Regex.IsMatch(value.ToString(), @"^(0[0-9]|1[0-9]|2[0-3]):([0-5][0-9])$"))
                {
                    context.AddFailure(context.DisplayName, msg);
                }
            });
        }
    }
}
