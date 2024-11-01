using System;
using System.Runtime.CompilerServices;
using ViCommon.EnsureHelper.ResultHelpers;

namespace ViCommon.EnsureHelper.UnitTest.Ensure.Mocks
{
    public class SuccessParameterValidatorMock<T> : AbstractParameterValidatorMock<T>
    {
        private readonly T _parameter;

        public SuccessParameterValidatorMock(T parameter)
        {
            this._parameter = parameter;
        }

        protected override void DoForAll(Predicate<T> predicate, Func<string, Exception> customExceptionBuilder)
        {
            predicate?.Invoke(this._parameter);
        }

        /// <inheritdoc />
        public override void ThrowOnFailure(string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
        }

        public override IResult ToResult() =>
            Result.Success();
    }
}
