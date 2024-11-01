using System;
using System.Runtime.CompilerServices;
using ViCommon.EnsureHelper.ResultHelpers;

namespace ViCommon.EnsureHelper.UnitTest.Ensure.Mocks
{
    public class FailureParameterValidatorMock<T> : AbstractParameterValidatorMock<T>
    {
        protected readonly Exception _exception;
        private readonly T _parameter;
        private readonly string _parameterName;

        public FailureParameterValidatorMock(Exception exception, T parameter, string parameterName)
        {
            this._exception = exception;
            this._parameter = parameter;
            this._parameterName = parameterName;
        }

        protected override void DoForAll(Predicate<T> predicate, Func<string, Exception> customExceptionBuilder)
        {
            predicate(this._parameter);
            customExceptionBuilder(this._parameterName);
        }

        /// <inheritdoc />
        public override void ThrowOnFailure(string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) =>
            throw this._exception;

        public override IResult ToResult()
            => Result.Failure(this._exception);
    }
}
