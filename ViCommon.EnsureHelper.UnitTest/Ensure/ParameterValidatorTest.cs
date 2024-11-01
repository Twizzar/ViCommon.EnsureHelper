using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ViCommon.EnsureHelper.ArgumentHelpers;

namespace ViCommon.EnsureHelper.UnitTest.Ensure
{
    [TestClass]
    public class ParameterValidatorTest
    {
        [TestMethod]
        public void When_is_true_predicate_returns_false_throw_ArgumentException()
        {
            // arrange
            var sut = new SuccessParameterValidatorDecorator<string>(null, Guid.NewGuid().ToString());

            // act
            Action action = () => sut
                .IsTrue(_ => false, Guid.NewGuid().ToString())
                .ThrowOnFailure();

            // assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void When_is_false_predicate_returns_true_throw_ArgumentException()
        {
            // arrange
            var sut = new SuccessParameterValidatorDecorator<string>(null, Guid.NewGuid().ToString());

            // act
            Action action = () => sut
                .IsFalse(_ => true, Guid.NewGuid().ToString())
                .ThrowOnFailure();

            // assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void OnFailure_is_called_before_exception_is_thrown()
        {
            // arrange
            Exception exp = null;
            var sut = new SuccessParameterValidatorDecorator<string>(null, Guid.NewGuid().ToString(), exception => exp = exception);

            // act
            Action action = () => sut
                .IsTrue(_ => false, Guid.NewGuid().ToString())
                .ThrowOnFailure();

            // assert
            action.Should().Throw<ArgumentException>();
            exp.Should().BeOfType<ArgumentException>();
        }

        [TestMethod]
        public void CustomException_is_thrown_when_customExceptionBuilder_is_defined()
        {
            // arrange
            var sut = new SuccessParameterValidatorDecorator<string>(null, Guid.NewGuid().ToString());

            // act
            Action actionIsTrue = () =>
                sut
                    .IsTrue(_ => false, parameterName => new AccessViolationException())
                    .ThrowOnFailure();
            Action actionIsFalse = () =>
                sut
                    .IsFalse(_ => true, parameterName => new AccessViolationException())
                    .ThrowOnFailure();

            // assert
            actionIsTrue.Should().Throw<AccessViolationException>();
            actionIsFalse.Should().Throw<AccessViolationException>();
        }

        [TestMethod]
        public void CustomExceptionBuilder_parameter_is_the_parameter_name()
        {
            // arrange
            var parameterName = Guid.NewGuid().ToString();
            var isTrueCustomExceptionBuilderMock = new Mock<Func<string, Exception>>();
            var isFalseCustomExceptionBuilderMock = new Mock<Func<string, Exception>>();
            isTrueCustomExceptionBuilderMock.Setup(_ => _(It.IsAny<string>())).Returns(new ArgumentException(""));
            isFalseCustomExceptionBuilderMock.Setup(_ => _(It.IsAny<string>())).Returns(new ArgumentException(""));

            var sut = new SuccessParameterValidatorDecorator<string>(null, parameterName);

            // act
            sut.IsTrue(_ => false, isTrueCustomExceptionBuilderMock.Object);
            sut.IsFalse(_ => true, isFalseCustomExceptionBuilderMock.Object);

            // assert
            isTrueCustomExceptionBuilderMock.Verify(func => func(parameterName), Times.Once);
            isFalseCustomExceptionBuilderMock.Verify(func => func(parameterName), Times.Once);
        }

        private class SuccessParameterValidatorDecorator<T> : SuccessParameterValidator<T>
        {
            protected internal SuccessParameterValidatorDecorator(T parameter, string parameterName, IEnsureHelper.EnsureHelperFailCallback onFailure = null) : base(parameter, parameterName, onFailure)
            {

            }
        }
    }
}
