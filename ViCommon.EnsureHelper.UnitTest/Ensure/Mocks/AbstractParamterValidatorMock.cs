using System;
using System.Runtime.CompilerServices;
using ViCommon.EnsureHelper.ArgumentHelpers;
using ViCommon.EnsureHelper.ResultHelpers;

namespace ViCommon.EnsureHelper.UnitTest.Ensure.Mocks
{
    public abstract class AbstractParameterValidatorMock<T> : IParameterValidator<T>
    {
        protected virtual void DoForAll(Predicate<T> predicate, Func<string, Exception> customExceptionBuilder) {}
        protected virtual void DoForTrue(Predicate<T> predicate, Func<string, Exception> customExceptionBuilder) {}
        protected virtual void DoForFalse(Predicate<T> predicate, Func<string, Exception> customExceptionBuilder) {}

        public IParameterValidator<T> IsTrue(Predicate<T> predicate, string failureMessage)
        {
            this.DoForAll(predicate, s => new ArgumentException(s));
            this.DoForTrue(predicate, s => new ArgumentException(s));
            return this;
        }


        public IParameterValidator<T> IsTrue(Predicate<T> predicate, Func<string, Exception> customExceptionBuilder)
        {
            this.DoForAll(predicate, customExceptionBuilder);
            this.DoForTrue(predicate, customExceptionBuilder);
            return this;
        }

        public IParameterValidator<T> IsFalse(Predicate<T> predicate, string failureMessage)
        {
            this.DoForAll(predicate, s => new ArgumentException(s));
            this.DoForFalse(predicate, s => new ArgumentException(s));
            return this;
        }

        public IParameterValidator<T> IsFalse(Predicate<T> predicate, Func<string, Exception> customExceptionBuilder)
        {
            this.DoForAll(predicate, customExceptionBuilder);
            this.DoForFalse(predicate, customExceptionBuilder);
            return this;
        }

        /// <inheritdoc />
        public abstract void ThrowOnFailure(string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0);

        public abstract IResult ToResult();
    }
}
