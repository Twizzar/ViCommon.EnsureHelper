using System;
using System.Runtime.CompilerServices;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViCommon.EnsureHelper.ArgumentHelpers.Extensions;

namespace ViCommon.EnsureHelper.UnitTest.Ensure
{
    [TestClass]
    public class EnsureHelperTest
    {
        [TestMethod]
        public void Configure_default_test()
        {
            // arrange
            CallerInformation callerInformation = default;

            var expectedCallerInformation = new CallerInformation(
                nameof(this.Configure_default_test),
                this.GetFileName(),
                35);

            var defaultHelper = new EnsureHelper();
            defaultHelper.RegisterOnThrowCallbacks((exp, information) =>
                callerInformation = information);

            EnsureHelper.ConfigureDefault(defaultHelper);

            // act
            try
            {
                EnsureHelper.GetDefault
                    .Parameter(false, "test")
                    .IsTrue(b => b, "Fail")
                    .ThrowOnFailure();
            }
            catch (Exception)
            {
                // assert
                callerInformation.Should().Be(expectedCallerInformation);
                return;
            }

            Assert.Fail("An exception should be thrown");
        }

        [TestMethod]
        public void ThrowWhenNullSetsTheCallerInformationCorrectlyTest()
        {
            // arrange
            CallerInformation callerInformation = default;

            var expectedCallerInformation = new CallerInformation(
                nameof(this.ThrowWhenNullSetsTheCallerInformationCorrectlyTest),
                this.GetFileName(),
                66);

            var sut = new EnsureHelper();
            sut.RegisterOnThrowCallbacks((exception, information) => 
                callerInformation = information);

            // act
            try
            {
                object o = null;
                sut.Parameter(o, nameof(o)).ThrowWhenNull();
            }
            catch (Exception)
            {
                // assert
                callerInformation.Should().Be(expectedCallerInformation);
                return;
            }

            Assert.Fail("An exception should be thrown");
        }

        [TestMethod]
        public void Ensure_with_no_empty_string_IsNotNullAndNotEmpty_does_not_throw()
        {
            // Arrange
            var helper = new EnsureHelperBuilder().Build();

            // Act
            Action action = () => helper.Parameter("test", "Parameter")
                .IsNotNullAndNotEmpty()
                .ThrowOnFailure();

            // Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void Ensure_with_empty_string_IsNotNullAndNotEmpty_throws()
        {
            // Arrange
            var helper = new EnsureHelperBuilder().Build();

            // Act
            Action action = () => helper.Parameter("", "Parameter")
                .IsNotNullAndNotEmpty()
                .ThrowOnFailure();

            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void Ensure_params_with_empty_string_IsNotNullAndNotEmpty_throws()
        {
            // Arrange
            var helper = new EnsureHelperBuilder().Build();

            // Act
            Action action = () => helper.Many<string>()
                    .Parameter("test", "test")
                    .Parameter(null, "null")
                    .IsNotNullAndNotEmpty()
                    .ThrowOnFailure();

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Ensure_with_registred_callbacks_on_failure_but_with_no_throw_should_call_fail_callback()
        {
            // Arrange
            Exception failExp = null;
            Exception throwExp = null;

            var helper = new EnsureHelperBuilder()
                .WithFailureCallback(exception => failExp = exception)
                .WithThrowCallback((exception, _) => throwExp = exception)
                .Build();

            // Act
            Action action = () => helper.Parameter("", "string")
                .IsNotNullAndNotEmpty();

            // Assert
            action.Should().NotThrow();
            failExp.Should().NotBeNull();
            throwExp.Should().BeNull();
        }

        [TestMethod]
        public void Ensure_with_registered_callbacks_on_fail_with_thrown_should_call_fail_and_thrown_callback()
        {
            // Arrange
            Exception failExp = null;
            Exception throwExp = null;

            var helper = new EnsureHelperBuilder()
                .WithFailureCallback(exception => failExp = exception)
                .WithThrowCallback((exception, _) => throwExp = exception)
                .Build();

            // Act
            Action action = () => helper.Parameter("", "string")
                .IsNotNullAndNotEmpty()
                .ThrowOnFailure();

            // Assert
            action.Should().Throw<ArgumentException>();
            failExp.Should().NotBeNull();
            throwExp.Should().NotBeNull();
        }

        private string GetFileName([CallerFilePath] string file = "") => file;

        private class EnsureHelperBuilder
        {
            private readonly EnsureHelper _ensureHelper = new EnsureHelper();

            public EnsureHelperBuilder WithFailureCallback(IEnsureHelper.EnsureHelperFailCallback failCallback)
            {
                this._ensureHelper.RegisterOnFailureCallbacks(failCallback);
                return this;
            }

            public EnsureHelperBuilder WithThrowCallback(IEnsureHelper.EnsureHelperThrowCallback throwCallback)
            {
                this._ensureHelper.RegisterOnThrowCallbacks(throwCallback);
                return this;
            }

            public EnsureHelper Build() => this._ensureHelper;
        }
    }
}