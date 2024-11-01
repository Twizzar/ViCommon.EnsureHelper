using System;
using ViCommon.EnsureHelper.ResultHelpers;

#pragma warning disable S3626 // Jump statements should not be redundant

namespace ViCommon.EnsureHelper.ArgumentHelpers.Extensions
{
    /// <summary>
    /// Decorates the <see cref="IParameterValidator{TParam}"/> with the <see cref="Result"/> methods.
    /// </summary>
    public static class ParameterValidatorResultExtension
    {
        /// <summary>
        /// Checks if the all checks where successful.
        /// </summary>
        /// <typeparam name="TParam">The parameter type.</typeparam>
        /// <param name="validator">The validator.</param>
        /// <returns>True when all checks are successful.</returns>
        public static bool IsSuccess<TParam>(this IParameterValidator<TParam> validator) =>
            EnsureHelper.GetDefault.Parameter(validator, nameof(validator)).IsNotNull()
                .MatchOrThrow(() => validator.ToResult().IsSuccess);

        /// <summary>
        /// Checks if one check has failed.
        /// </summary>
        /// <typeparam name="TParam">The parameter type.</typeparam>
        /// <param name="validator">The validator.</param>
        /// <returns>True when a check failed.</returns>
        public static bool IsFailure<TParam>(this IParameterValidator<TParam> validator) =>
            EnsureHelper.GetDefault.Parameter(validator, nameof(validator)).IsNotNull()
                .MatchOrThrow(() => validator.ToResult().IsFailure);

        /// <summary>
        /// Invokes onSuccess on success or onFailure on failure and returns the output of the function.
        /// </summary>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <typeparam name="TParam">The parameter type.</typeparam>
        /// <param name="validator">The validator.</param>
        /// <param name="onSuccess">Function invoked on success.</param>
        /// <param name="onFailure">Function invoked on failure.</param>
        /// <returns>The result of the function.</returns>
        public static TResult Match<TResult, TParam>(
            this IParameterValidator<TParam> validator,
            Func<TResult> onSuccess,
            Func<Exception, TResult> onFailure) =>
            EnsureHelper.GetDefault.Parameter(validator, nameof(validator)).IsNotNull()
                .MatchOrThrow(() => validator.ToResult().Match(onSuccess, onFailure));

        /// <summary>
        /// Invoked onSuccess on success or throws an exception.
        /// </summary>
        /// <typeparam name="TResult">Type of the returned value.</typeparam>
        /// <typeparam name="TParam">The parameter type.</typeparam>
        /// <param name="validator">The validator.</param>
        /// <param name="onSuccess">Function invoked on success.</param>
        /// <returns>The returned value of onSuccess or throws an exception on failure.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Helper class is used to check paramters.")]
        public static TResult MatchOrThrow<TResult, TParam>(
            this IParameterValidator<TParam> validator,
            Func<TResult> onSuccess)
        {
            EnsureHelper.GetDefault.Parameter(validator, nameof(validator)).ThrowWhenNull();
            return validator.ToResult().Match(onSuccess, exp => throw exp);
        }
    }
}
