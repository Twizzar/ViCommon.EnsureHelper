using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft;
using ViCommon.EnsureHelper.ResultHelpers;
using static ViCommon.EnsureHelper.IEnsureHelper;

namespace ViCommon.EnsureHelper.ArgumentHelpers
{
    /// <inheritdoc />
    public class ParameterValidatorCollection<TParam> : IParameterValidator<TParam>
    {
        private readonly EnsureHelperFailCallback _onFailure;
        private readonly EnsureHelperThrowCallback _onThrow;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterValidatorCollection{TParam}"/> class.
        /// </summary>
        /// <param name="onFailure">Action to execute before throwing the exception.</param>
        /// <param name="onThrow">Action to execute when a exception is thrown.</param>
        protected internal ParameterValidatorCollection(EnsureHelperFailCallback onFailure = null, EnsureHelperThrowCallback onThrow = null)
        {
            this.Validators = new List<IParameterValidator<TParam>>();
            this._onFailure = onFailure;
            this._onThrow = onThrow;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterValidatorCollection{TParam}"/> class.
        /// </summary>
        /// <param name="validators">A sequence of validators.</param>
        /// <param name="onFailure">Action to execute before throwing the exception.</param>
        /// <param name="onThrow">Action to execute when a exception is thrown.</param>
        protected internal ParameterValidatorCollection(IEnumerable<IParameterValidator<TParam>> validators, EnsureHelperFailCallback onFailure = null, EnsureHelperThrowCallback onThrow = null)
        {
            this.Validators = validators.ToList();
            this._onFailure = onFailure;
            this._onThrow = onThrow;
        }

        /// <summary>
        /// Gets a List of all validators.
        /// </summary>
        protected List<IParameterValidator<TParam>> Validators { get; }

        /// <summary>
        /// Add a parameter to check.
        /// </summary>
        /// <param name="param">The parameter value.</param>
        /// <param name="parameterName">The parameter name.</param>
        /// <returns>Its self.</returns>
        public ParameterValidatorCollection<TParam> Parameter([ValidatedNotNull] TParam param, string parameterName)
        {
            this.Validators.Add(new SuccessParameterValidator<TParam>(param, parameterName, this._onFailure, this._onThrow));
            return this;
        }

        /// <inheritdoc />
        public IParameterValidator<TParam> IsTrue(Predicate<TParam> predicate, string failureMessage) =>
            this.IsTrue(predicate, paramName => new ArgumentException(failureMessage, paramName));

        /// <inheritdoc />
        public IParameterValidator<TParam> IsTrue(Predicate<TParam> predicate, Func<string, Exception> customExceptionBuilder) =>
            new ParameterValidatorCollection<TParam>(
                this.Validators.Select(validator => validator.IsTrue(predicate, customExceptionBuilder)),
                this._onFailure,
                this._onThrow);

        /// <inheritdoc />
        public IParameterValidator<TParam> IsFalse(Predicate<TParam> predicate, string failureMessage) =>
            this.IsTrue(predicate, paramName => new ArgumentException(failureMessage, paramName));

        /// <inheritdoc />
        public IParameterValidator<TParam> IsFalse(Predicate<TParam> predicate, Func<string, Exception> customExceptionBuilder) =>
            this.IsTrue(param => !predicate(param), customExceptionBuilder);

        /// <inheritdoc />
        public void ThrowOnFailure([CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) =>
            this.ToResult().OnFailure(e => this.InvokeAndThrow(e, new CallerInformation(callerMemberName, callerFilePath, callerLineNumber)));

        /// <inheritdoc />
        public IResult ToResult() =>
            this.Validators.Select(validator => validator.ToResult())
                .Aggregate(Result.Success(), Result.Combine);

        private void InvokeAndThrow(Exception exp, CallerInformation callerInformation)
        {
            this._onThrow?.Invoke(exp, callerInformation);
            throw exp;
        }
    }
}
