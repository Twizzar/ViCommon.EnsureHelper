using System;
using System.Runtime.CompilerServices;
using ViCommon.EnsureHelper.ResultHelpers;
using static ViCommon.EnsureHelper.IEnsureHelper;

namespace ViCommon.EnsureHelper.ArgumentHelpers
{
    /// <inheritdoc />
    public class FailureParameterValidator<TParam> : IParameterValidator<TParam>
    {
        #region fields

        private readonly Exception _exception;
        private readonly EnsureHelperThrowCallback _onThrow;

        #endregion

        #region ctors

        /// <summary>
        /// Initializes a new instance of the <see cref="FailureParameterValidator{TParam}"/> class.
        /// </summary>
        /// <param name="exception">The failure exception.</param>
        /// <param name="onThrow">Action to execute before throwing the exception.</param>
        protected internal FailureParameterValidator(Exception exception, EnsureHelperThrowCallback onThrow = null)
        {
            this._exception = exception;
            this._onThrow = onThrow;
        }

        #endregion

        #region members

        /// <inheritdoc />
        public IParameterValidator<TParam> IsTrue(Predicate<TParam> predicate, string failureMessage) =>
            this.IsTrue(predicate, argumentName => new ArgumentException(failureMessage, argumentName));

        /// <inheritdoc />
        public IParameterValidator<TParam> IsTrue(
            Predicate<TParam> predicate,
            Func<string, Exception> customExceptionBuilder) =>
            this;

        /// <inheritdoc />
        public IParameterValidator<TParam> IsFalse(Predicate<TParam> predicate, string failureMessage) =>
            this.IsFalse(predicate, argumentName => new ArgumentException(failureMessage, argumentName));

        /// <inheritdoc />
        public IParameterValidator<TParam> IsFalse(
            Predicate<TParam> predicate,
            Func<string, Exception> customExceptionBuilder) =>
            this.IsTrue(arg => !predicate(arg), customExceptionBuilder);

        /// <inheritdoc />
        public void ThrowOnFailure([CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            this._onThrow?.Invoke(this._exception, new CallerInformation(callerMemberName, callerFilePath, callerLineNumber));

            throw this._exception;
        }

        /// <inheritdoc />
        public IResult ToResult() => Result.Failure(this._exception);

        #endregion
    }
}