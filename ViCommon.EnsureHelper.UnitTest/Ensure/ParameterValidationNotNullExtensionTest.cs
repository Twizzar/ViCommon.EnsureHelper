using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViCommon.EnsureHelper.ArgumentHelpers.Extensions;
using ViCommon.EnsureHelper.UnitTest.Ensure.Mocks;

namespace ViCommon.EnsureHelper.UnitTest.Ensure
{
    [TestClass]
    public class ParameterValidationNotNullExtensionTest
    {
        [TestMethod]
        public void When_methods_called_with_null_as_validator_throw_ArgumentException()
        {
            // Act
            Action actionIsNotNull = () => ParameterValidationNotNullExtension.IsNotNull<string>(null);
            Action actionThrowWhenNull = () => ParameterValidationNotNullExtension.ThrowWhenNull<string>(null);

            // Assert
            actionIsNotNull.Should().Throw<ArgumentException>();
            actionThrowWhenNull.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void IsNotNull_with_success_validator_does_not_throw()
        {
            // Arrange
            var validator = new ParameterValidatorBuilder().BuildSuccessValidator();

            // Assert
            Action action = () => validator.IsNotNull();

            //Act
            action.Should().NotThrow();
        }

        [TestMethod]
        public void IsNotNull_with_failure_validator_does_not_throw_when_ThrowOnFailure_is_not_called()
        {
            // Arrange
            var validator = new ParameterValidatorBuilder().BuildFailureValidator();

            // Assert
            Action action = () => validator.IsNotNull();

            //Act
            action.Should().NotThrow();
        }

        [TestMethod]
        public void IsNotNull_with_failure_validator_throws_when_ThrowOnFailure_is_called()
        {
            // Arrange
            var validator = new ParameterValidatorBuilder().WithException(new IndexOutOfRangeException()).BuildFailureValidator();

            // Assert
            Action action = () => validator.IsNotNull(s => new IndexOutOfRangeException()).ThrowOnFailure();

            //Act
            action.Should().Throw<IndexOutOfRangeException>();
        }
    }
}
