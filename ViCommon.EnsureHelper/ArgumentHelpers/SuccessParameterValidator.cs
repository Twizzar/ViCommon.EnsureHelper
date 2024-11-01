using System;
using System.Runtime.CompilerServices;
using ViCommon.EnsureHelper.ResultHelpers;
using static ViCommon.EnsureHelper.IEnsureHelper;

namespace ViCommon.EnsureHelper.ArgumentHelpers
{
    /// <inheritdoc />
    public class SuccessParameterValidator<TParam> : IParameterValidator<TParam>
    {
        private readonly TParam _parameter;
        private readonly string _parameterName;
        private readonly EnsureHelperFailCallback _onFailure;
        private readonly EnsureHelperThrowCallback _onThrow;

        /// <summary>
        /// Initializes a new instance of the <see cref="SuccessParameterValidator{TParam}"/> class.
        /// </summary>
        /// <param name="parameter">The parameter value.</param>
        /// <param name="parameterName">The argument name.</param>
        /// <param name="onFailure">Action to execute before throwing the exception.</param>
        /// <param name="onThrow">Action to execute when a exception is thrown.</param>
        protected internal SuccessParameterValidator(TParam parameter, string parameterName, EnsureHelperFailCallback onFailure = null, EnsureHelperThrowCallback onThrow = null)
        {
            this._parameter = parameter;
            this._parameterName = parameterName;
            this._onFailure = onFailure;
            this._onThrow = onThrow;
        }

        /// <inheritdoc />
        public IParameterValidator<TParam> IsTrue(Predicate<TParam> predicate, string failureMessage) =>
            this.IsTrue(predicate, argumentName => new ArgumentException(failureMessage, argumentName));

        /// <inheritdoc />
        public IParameterValidator<TParam> IsTrue(Predicate<TParam> predicate, Func<string, Exception> customExceptionBuilder)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            if (predicate(this._parameter))
            {
                return this;
            }
            else
            {
                var exp = customExceptionBuilder(this._parameterName);
                this._onFailure?.Invoke(exp);
                return new FailureParameterValidator<TParam>(exp, this._onThrow);
            }
        }

        /// <inheritdoc />
        public IParameterValidator<TParam> IsFalse(Predicate<TParam> predicate, string failureMessage) =>
            this.IsFalse(predicate, argumentName => new ArgumentException(failureMessage, argumentName));

        /// <inheritdoc />
        public IParameterValidator<TParam> IsFalse(Predicate<TParam> predicate, Func<string, Exception> customExceptionBuilder)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return this.IsTrue(arg => !predicate(arg), customExceptionBuilder);
        }

        /// <inheritdoc />
        public void ThrowOnFailure([CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            // Do Nothing
        }

        /// <inheritdoc />
        public IResult ToResult() =>
            Result.Success();
    }
}
