using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ViCommon.EnsureHelper.ArgumentHelpers;

namespace ViCommon.EnsureHelper.UnitTest.Ensure
{
    [TestClass]
    public class ParameterManyValidatorTest
    {
        [TestMethod]
        public void When_all_validators_are_successful_then_no_exception_is_thrown()
        {
            // arrange
            var validator = Enumerable.Range(0, 10).Select(_ => this.CreateSingleSuccessfulValidator<string>());
            var sut = new ParameterValidatorTestCollectionCollection<string>(validator.ToArray());

            // act
            Action actionIsTrue = () => sut.IsTrue(_ => true, Guid.NewGuid().ToString());
            Action actionIsFalse = () => sut.IsFalse(_ => false, Guid.NewGuid().ToString());

            // assert
            actionIsTrue.Should().NotThrow();
            actionIsFalse.Should().NotThrow();
        }

        [TestMethod]
        public void When_one_validator_is_failing_then_an_exception_is_thrown()
        {
            // arrange
            var validator = Enumerable.Range(0, 10)
                .Select(_ => this.CreateSingleSuccessfulValidator<string>())
                .Append(this.CreateSingleFailingValidator<string>());
            var sut = new ParameterValidatorTestCollectionCollection<string>(validator.ToArray());

            // act
            Action actionIsTrue = () => sut
                .IsTrue(_ => true, Guid.NewGuid().ToString())
                .ThrowOnFailure();
            Action actionIsFalse = () => sut
                .IsFalse(_ => false, Guid.NewGuid().ToString())
                .ThrowOnFailure();

            // assert
            actionIsTrue.Should().Throw<ArgumentException>();
            actionIsFalse.Should().Throw<ArgumentException>();
        }

        private IParameterValidator<T> CreateSingleFailingValidator<T>()
        {
            var validator = new Mock<IParameterValidator<T>>();
            validator
                .Setup(v => v.IsTrue(It.IsAny<Predicate<T>>(), It.IsAny<string>()))
                .Throws<ArgumentException>();
            validator
                .Setup(v => v.IsFalse(It.IsAny<Predicate<T>>(), It.IsAny<string>()))
                .Throws<ArgumentException>();
            return validator.Object;
        }

        private IParameterValidator<T> CreateSingleSuccessfulValidator<T>()
        {
            var validator = new Mock<IParameterValidator<T>>();
            validator
                .Setup(v => v.IsTrue(It.IsAny<Predicate<T>>(), It.IsAny<string>()))
                .Returns(validator.Object);
            validator
                .Setup(v => v.IsTrue(It.IsAny<Predicate<T>>(), It.IsAny<Func<string, Exception>>()))
                .Returns(validator.Object);
            validator
                .Setup(v => v.IsFalse(It.IsAny<Predicate<T>>(), It.IsAny<string>()))
                .Returns(validator.Object);
            validator
                .Setup(v => v.IsFalse(It.IsAny<Predicate<T>>(), It.IsAny<Func<string, Exception>>()))
                .Returns(validator.Object);
            return validator.Object;
        }

        private class ParameterValidatorTestCollectionCollection<T> : ParameterValidatorCollection<T>
        {
            protected internal ParameterValidatorTestCollectionCollection(IEnumerable<IParameterValidator<T>> parameterValidators) : base()
            {
                foreach (var parameterValidator in parameterValidators)
                {
                    Validators.Add(parameterValidator);
                }
            }
        }
    }
}
