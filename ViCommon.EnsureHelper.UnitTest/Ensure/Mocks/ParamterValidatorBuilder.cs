using System;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace ViCommon.EnsureHelper.UnitTest.Ensure.Mocks
{
    public class ParameterValidatorBuilder
    {
        private string _parameter;
        private string _parameterName;
        private Exception _exception;

        public ParameterValidatorBuilder()
        {
            this._parameter = Guid.NewGuid().ToString();
            this._parameterName = "ParameterName";
            this._exception = new ArgumentException("Some exception");
        }

        public ParameterValidatorBuilder WithParameter(string parameter, string parameterName)
        {
            this._parameter = parameter;
            this._parameterName = parameterName;
            return this;
        }

        public ParameterValidatorBuilder WithException(Exception exception)
        {
            this._exception = exception;
            return this;
        }

        public SuccessParameterValidatorMock<string> BuildSuccessValidator() => 
            new SuccessParameterValidatorMock<string>(this._parameter);

        public FailureParameterValidatorMock<string> BuildFailureValidator() => 
            new FailureParameterValidatorMock<string>(this._exception, this._parameter, this._parameterName);
    }
}
