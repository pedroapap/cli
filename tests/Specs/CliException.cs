using Cmf.CLI.Core.Enums;
using Cmf.CLI.Core.Utilities;
using Cmf.CLI.Utilities;
using FluentAssertions;
using System;
using System.Reflection;
using Xunit;

namespace tests.Specs
{
    public class CliExceptionTests
    {
        [Fact]
        public void Constructor_Default_SetsDefaultErrorCode()
        {
            // Act
            var exception = new CliException();

            // Assert
            exception.ErrorCode.Should().Be(ErrorCode.Default);
            exception.Message.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void Constructor_WithErrorCode_SetsErrorCode()
        {
            // Arrange
            var errorCode = ErrorCode.InvalidArgument;

            // Act
            var exception = new CliException(errorCode);

            // Assert
            exception.ErrorCode.Should().Be(errorCode);
            exception.Message.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void Constructor_WithMessage_SetsMessage()
        {
            // Arrange
            var message = "Test error message";

            // Act
            var exception = new CliException(message);

            // Assert
            exception.Message.Should().Be(message);
            exception.ErrorCode.Should().Be(ErrorCode.Default);
        }

        [Fact]
        public void Constructor_WithMessageAndErrorCode_SetsBothProperties()
        {
            // Arrange
            var message = "Test error message";
            var errorCode = ErrorCode.InvalidArgument;

            // Act
            var exception = new CliException(message, errorCode);

            // Assert
            exception.Message.Should().Be(message);
            exception.ErrorCode.Should().Be(errorCode);
        }

        [Fact]
        public void Constructor_WithMessageAndInnerException_SetsBothProperties()
        {
            // Arrange
            var message = "Test error message";
            var innerException = new InvalidOperationException("Inner exception");

            // Act
            var exception = new CliException(message, innerException);

            // Assert
            exception.Message.Should().Be(message);
            exception.InnerException.Should().Be(innerException);
            exception.ErrorCode.Should().Be(ErrorCode.Default);
        }

        [Fact]
        public void Constructor_WithMessageAndInnerExceptionAndErrorCode_SetsAllProperties()
        {
            // Arrange
            var message = "Test error message";
            var innerException = new InvalidOperationException("Inner exception");
            var errorCode = ErrorCode.InvalidArgument;

            // Act
            var exception = new CliException(message, innerException, errorCode);

            // Assert
            exception.Message.Should().Be(message);
            exception.InnerException.Should().Be(innerException);
            exception.ErrorCode.Should().Be(errorCode);
        }

        [Fact]
        public void Handler_WithCliException_RethrowsDirectly()
        {
            // Arrange
            var originalException = new CliException("Test message", ErrorCode.InvalidArgument);

            // Act & Assert
            var thrownException = Assert.Throws<CliException>(() => CliException.Handler(originalException));
            thrownException.Should().BeSameAs(originalException);
        }

        [Fact]
        public void Handler_WithTargetInvocationExceptionContainingCliException_RethrowsInnerException()
        {
            // Arrange
            var cliException = new CliException("Test message", ErrorCode.InvalidArgument);
            var targetInvocationException = new TargetInvocationException(cliException);

            // Act & Assert
            var thrownException = Assert.Throws<CliException>(() => CliException.Handler(targetInvocationException));
            thrownException.Should().BeSameAs(cliException);
        }

        [Fact]
        public void Handler_WithRegularException_WrapsAndRethrows()
        {
            // Arrange
            var originalException = new InvalidOperationException("Test message");

            // Act & Assert
            var thrownException = Assert.Throws<WrappedException>(() => CliException.Handler(originalException));
            
            // Should be wrapped (specific behavior depends on WrappedException.Wrap implementation)
            thrownException.Should().NotBeSameAs(originalException);
        }

        [Fact]
        public void Handler_WithTargetInvocationExceptionContainingRegularException_WrapsAndRethrows()
        {
            // Arrange
            var innerException = new InvalidOperationException("Test message");
            var targetInvocationException = new TargetInvocationException(innerException);

            // Act & Assert
            var thrownException = Assert.Throws<WrappedException>(() => CliException.Handler(targetInvocationException));
            
            // Should be wrapped (specific behavior depends on WrappedException.Wrap implementation)
            thrownException.Should().NotBeSameAs(targetInvocationException);
        }

        [Theory]
        [InlineData(ErrorCode.Default)]
        [InlineData(ErrorCode.Success)]
        [InlineData(ErrorCode.InvalidArgument)]
        public void Constructor_WithDifferentErrorCodes_SetsCorrectErrorCode(ErrorCode errorCode)
        {
            // Act
            var exception = new CliException("Test message", errorCode);

            // Assert
            exception.ErrorCode.Should().Be(errorCode);
        }

        [Fact]
        public void CliException_IsSerializable()
        {
            // Arrange
            var exception = new CliException("Test message", ErrorCode.InvalidArgument);

            // Assert
            exception.GetType().Should().BeDecoratedWith<SerializableAttribute>();
        }

        [Fact]
        public void CliException_InheritsFromException()
        {
            // Arrange
            var exception = new CliException();

            // Assert
            exception.Should().BeAssignableTo<Exception>();
        }
    }
}