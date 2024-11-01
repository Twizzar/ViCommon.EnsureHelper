using System;
using System.Runtime.CompilerServices;
using static ViCommon.EnsureHelper.ArgumentHelpers.ExceptionBuilders.DefaultExceptionBuilder;

namespace ViCommon.EnsureHelper.ArgumentHelpers.Extensions
{
    /// <summary>
    /// Extension for <see cref="IParameterValidator{TParam}"/> to check for null ref.
    /// </summary>
    public static class ParameterValidationNotNullExtension
    {
        /// <summary>
        /// Check if the argument is not null.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <param name="validator">The validator.</param>
        /// <returns>A new <see cref="IParameterValidator{TParam}"/> to chain  more checks or react to the validation.</returns>
        public static IParameterValidator<TParameter> IsNotNull<TParameter>(
            this IParameterValidator<TParameter> validator)
            where TParameter : class =>
            IsNotNull(validator, ArgumentNullExceptionBuilder());

        /// <summary>
        /// Check if the argument is not null.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <param name="validator">The validator.</param>
        /// <param name="customExceptionBuilder">Function which has as input the parameter name and builds a custom <see cref="Exception"/> to throw on failure.</param>
        /// <returns>A new <see cref="IParameterValidator{TParam}"/> to chain  more checks or react to the validation.</returns>
        public static IParameterValidator<TParameter> IsNotNull<TParameter>(
            this IParameterValidator<TParameter> validator,
            Func<string, Exception> customExceptionBuilder)
            where TParameter : class =>
            validator != null
                ? validator.IsTrue(argument => argument != null, customExceptionBuilder)
                : throw new ArgumentNullException(nameof(validator));

        /// <summary>
        /// Check if the argument is not null and throws <see cref="NullReferenceException"/> when it is null..
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <param name="validator">The validator.</param>
        /// <param name="callerMemberName">Caller member name set by the compiler.</param>
        /// <param name="callerFilePath">Caller file path set by the compiler.</param>
        /// <param name="lineNumber">Caller file number set by the compiler.</param>
        public static void ThrowWhenNull<TParameter>(
            this IParameterValidator<TParameter> validator,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int lineNumber = 0)
            where TParameter : class =>
            IsNotNull(validator, ArgumentNullExceptionBuilder()).ThrowOnFailure(callerMemberName, callerFilePath, lineNumber);
    }
}
