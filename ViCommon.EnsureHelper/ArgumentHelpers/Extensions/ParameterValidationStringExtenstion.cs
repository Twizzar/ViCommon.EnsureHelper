using System;
using static ViCommon.EnsureHelper.ArgumentHelpers.ExceptionBuilders.DefaultExceptionBuilder;

#pragma warning disable CA1303 // Do not pass literals as localized parameters

namespace ViCommon.EnsureHelper.ArgumentHelpers.Extensions
{
    /// <summary>
    /// Extenstion methods for the type IParameterValidator&lt;string&gt;.
    /// </summary>
    public static class ParameterValidationStringExtenstion
    {
        /// <summary>
        /// Checks if the string is empty.
        /// </summary>
        /// <param name="parameterValidator">The argument validator.</param>
        /// <returns>A new <see cref="IParameterValidator{TParam}"/> to chain more checks or react to the validation.</returns>
        public static IParameterValidator<string> IsNotEmpty(this IParameterValidator<string> parameterValidator) =>
            IsNotEmpty(parameterValidator, ArgumentExceptionBuilder("Parameter is empty"));

        /// <summary>
        /// Checks if the string is empty.
        /// </summary>
        /// <param name="parameterValidator">The argument validator.</param>
        /// <param name="customExceptionBuilder">Function which has as input the parameter name and builds a custom <see cref="Exception"/> to throw on failure.</param>
        /// <returns>A new <see cref="IParameterValidator{TParam}"/> to chain  more checks or react to the validation.</returns>
        public static IParameterValidator<string> IsNotEmpty(this IParameterValidator<string> parameterValidator, Func<string, Exception> customExceptionBuilder)
        {
            EnsureHelper.GetDefault.Parameter(parameterValidator, nameof(parameterValidator)).ThrowWhenNull();
            return parameterValidator.IsFalse(IsEmpty, customExceptionBuilder);
        }

        /// <summary>
        /// Checks if the string is empty.
        /// </summary>
        /// <param name="parameterValidator">The argument validator.</param>
        /// <returns>A new <see cref="IParameterValidator{TParam}"/> to chain  more checks or react to the validation.</returns>
        public static IParameterValidator<string> IsNotNullAndNotEmpty(this IParameterValidator<string> parameterValidator) =>
            parameterValidator.IsNotNull().IsNotEmpty();

        private static bool IsEmpty(string parameter)
        {
            EnsureHelper.GetDefault.Parameter(parameter, nameof(parameter)).ThrowWhenNull();
            return parameter.Length == 0;
        }
    }
}
