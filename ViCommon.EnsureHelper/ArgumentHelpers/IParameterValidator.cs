using System;
using System.Runtime.CompilerServices;
using ViCommon.EnsureHelper.ResultHelpers;

namespace ViCommon.EnsureHelper.ArgumentHelpers
{
    /// <summary>
    /// Helper for helping validate method argument.
    /// </summary>
    /// <typeparam name="TParam">The type of the argument.</typeparam>
    public interface IParameterValidator<out TParam>
    {
        /// <summary>
        /// Checks if the predicate is true.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="failureMessage">Message provided to the ArgumentExceptionBuilder on failure.</param>
        /// <returns>On success returns itself on failure throws an <see cref="ArgumentException"/>.</returns>
        IParameterValidator<TParam> IsTrue(Predicate<TParam> predicate, string failureMessage);

        /// <summary>
        /// Checks if the predicate is true.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="customExceptionBuilder">Function which has as input the parameter name and builds a custom <see cref="Exception"/> to throw on failure.</param>
        /// <returns>On success returns itself on failure throws the customException.</returns>ss
        IParameterValidator<TParam> IsTrue(Predicate<TParam> predicate, Func<string, Exception> customExceptionBuilder);

        /// <summary>
        /// Checks if the predicate is false.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="failureMessage">Message provided to the ArgumentExceptionBuilder on failure.</param>
        /// <returns>On success returns itself on failure throws an <see cref="ArgumentException"/>.</returns>
        IParameterValidator<TParam> IsFalse(Predicate<TParam> predicate, string failureMessage);

        /// <summary>
        /// Checks if the predicate is false.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="customExceptionBuilder">Function which has as input the parameter name and builds a custom <see cref="Exception"/> to throw on failure.</param>
        /// <returns>On success returns itself on failure throws the customException.</returns>
        IParameterValidator<TParam> IsFalse(Predicate<TParam> predicate, Func<string, Exception> customExceptionBuilder);

        /// <summary>
        /// When a check fails throw an exception.
        /// </summary>
        /// <param name="callerMemberName">Caller member name set by the compiler.</param>
        /// <param name="callerFilePath">Caller path set by the compiler.</param>
        /// <param name="callerLineNumber">Caller line number set by the compiler.</param>
        void ThrowOnFailure([CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0);

        /// <summary>
        /// Convert the validator to an result.
        /// </summary>
        /// <returns>An <see cref="Result"/>.</returns>
        IResult ToResult();
    }
}
