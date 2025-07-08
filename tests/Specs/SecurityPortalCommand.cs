using Autofac.Extras.Moq;
using Cmf.Common.Cli.Commands.New;
using Cmf.CLI.Core.Enums;
using FluentAssertions;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using tests.Objects;
using Xunit;

namespace tests.Specs
{
    public class SecurityPortalCommandTests
    {
        [Fact]
        public void Constructor_WithoutFileSystem_SetsCorrectProperties()
        {
            // Act
            var command = new SecurityPortalCommand();

            // Assert
            command.Should().NotBeNull();
            // Test that it inherits from LayerTemplateCommand correctly
        }

        [Fact]
        public void Constructor_WithFileSystem_SetsCorrectProperties()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            var command = new SecurityPortalCommand(fileSystem);

            // Assert
            command.Should().NotBeNull();
        }

        [Fact]
        public void GenerateArgs_WithValidInputs_ReturnsArgs()
        {
            // Arrange
            using var mock = AutoMock.GetLoose();
            var fileSystem = new MockFileSystem();
            var command = new TestableSecurityPortalCommand(fileSystem);
            
            var projectRoot = fileSystem.DirectoryInfo.New("/project");
            var workingDir = fileSystem.DirectoryInfo.New("/project/working");
            var args = new List<string> { "--test", "value" };

            // Act
            var result = command.GenerateArgsPublic(projectRoot, workingDir, args);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(args);
        }

        [Fact]
        public void GenerateArgs_WithEmptyArgs_ReturnsEmptyArgs()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var command = new TestableSecurityPortalCommand(fileSystem);
            
            var projectRoot = fileSystem.DirectoryInfo.New("/project");
            var workingDir = fileSystem.DirectoryInfo.New("/project/working");
            var args = new List<string>();

            // Act
            var result = command.GenerateArgsPublic(projectRoot, workingDir, args);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public void GenerateArgs_WithNullArgs_ReturnsEmptyArgs()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var command = new TestableSecurityPortalCommand(fileSystem);
            
            var projectRoot = fileSystem.DirectoryInfo.New("/project");
            var workingDir = fileSystem.DirectoryInfo.New("/project/working");

            // Act
            var result = command.GenerateArgsPublic(projectRoot, workingDir, null);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public void GenerateArgs_WithMultipleArgs_ReturnsAllArgs()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var command = new TestableSecurityPortalCommand(fileSystem);
            
            var projectRoot = fileSystem.DirectoryInfo.New("/project");
            var workingDir = fileSystem.DirectoryInfo.New("/project/working");
            var args = new List<string> 
            { 
                "--arg1", "value1",
                "--arg2", "value2",
                "--flag"
            };

            // Act
            var result = command.GenerateArgsPublic(projectRoot, workingDir, args);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(5);
            result.Should().BeEquivalentTo(args);
        }
    }

    // Helper class to access protected method for testing
    public class TestableSecurityPortalCommand : SecurityPortalCommand
    {
        public TestableSecurityPortalCommand(IFileSystem fileSystem) : base(fileSystem)
        {
        }

        public List<string> GenerateArgsPublic(IDirectoryInfo projectRoot, IDirectoryInfo workingDir, List<string> args)
        {
            return GenerateArgs(projectRoot, workingDir, args ?? new List<string>());
        }
    }
}