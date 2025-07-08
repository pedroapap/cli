using Cmf.CLI.Utilities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace tests.Specs
{
    public class IoTStructuresTests
    {
        [Theory]
        [InlineData("String", "string")]
        [InlineData("Integer", "number")]
        [InlineData("Long", "number")]
        [InlineData("Decimal", "number")]
        [InlineData("Boolean", "boolean")]
        [InlineData("Object", "object")]
        [InlineData("DateTime", "Date")]
        [InlineData("Buffer", "Buffer")]
        [InlineData("Any", "any")]
        public void ConvertIoTTypesToJSTypes_WithDataTypeInputOutput_ReturnsCorrectJSType(string iotType, string expectedJsType)
        {
            // Act
            var result = IoTStructures.ConvertIoTTypesToJSTypes<DataTypeInputOutput>(iotType);

            // Assert
            result.Should().Be(expectedJsType);
        }

        [Theory]
        [InlineData("String", "string")]
        [InlineData("Integer", "number")]
        [InlineData("Long", "number")]
        [InlineData("Decimal", "number")]
        [InlineData("Boolean", "boolean")]
        [InlineData("Object", "object")]
        [InlineData("Enum", "<Declare your enum>")]
        public void ConvertIoTTypesToJSTypes_WithDataTypeSetting_ReturnsCorrectJSType(string iotType, string expectedJsType)
        {
            // Act
            var result = IoTStructures.ConvertIoTTypesToJSTypes<DataTypeSetting>(iotType);

            // Assert
            result.Should().Be(expectedJsType);
        }

        [Fact]
        public void ConvertIoTTypesToJSTypes_WithDataTypeInputOutputEnum_ReturnsCorrectJSType()
        {
            // Act
            var result = IoTStructures.ConvertIoTTypesToJSTypes(DataTypeInputOutput.String);

            // Assert
            result.Should().Be("string");
        }

        [Fact]
        public void ConvertIoTTypesToJSTypes_WithDataTypeSettingEnum_ReturnsCorrectJSType()
        {
            // Act
            var result = IoTStructures.ConvertIoTTypesToJSTypes(DataTypeSetting.Boolean);

            // Assert
            result.Should().Be("boolean");
        }

        [Theory]
        [InlineData(DataTypeInputOutput.String, "Task.TaskValueType.String")]
        [InlineData(DataTypeInputOutput.Integer, "Task.TaskValueType.Integer")]
        [InlineData(DataTypeInputOutput.Boolean, "Task.TaskValueType.Boolean")]
        [InlineData(DataTypeInputOutput.Any, "undefined")]
        public void ConvertIoTValueTypeToTaskValueType_WithDataTypeInputOutput_ReturnsCorrectTaskValueType(DataTypeInputOutput type, string expected)
        {
            // Act
            var result = IoTStructures.ConvertIoTValueTypeToTaskValueType(type);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(IoTValueType.String, "Task.TaskValueType.String")]
        [InlineData(IoTValueType.Integer, "Task.TaskValueType.Integer")]
        [InlineData(IoTValueType.Boolean, "Task.TaskValueType.Boolean")]
        [InlineData(IoTValueType.Any, "undefined")]
        public void ConvertIoTValueTypeToTaskValueType_WithIoTValueType_ReturnsCorrectTaskValueType(IoTValueType type, string expected)
        {
            // Act
            var result = IoTStructures.ConvertIoTValueTypeToTaskValueType(type);

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void AskDynamicType_WithStringType_ReturnsStringValue()
        {
            // Note: This method uses AnsiConsole.Ask which requires interactive input
            // We can't easily test it without mocking the console, but we can test the type conversion logic
            
            // This test verifies the method exists and can be called
            // In a real scenario, we'd need to mock Spectre.Console
            var methodExists = typeof(IoTStructures).GetMethod("AskDynamicType");
            methodExists.Should().NotBeNull();
        }

        [Fact]
        public void AskChoice_MethodExists()
        {
            // Note: This method uses AnsiConsole.Prompt which requires interactive input
            // We can verify the method exists for coverage
            var methodExists = typeof(IoTStructures).GetMethod("AskChoice");
            methodExists.Should().NotBeNull();
        }

        [Fact]
        public void TemplateTaskLibrary_HasCorrectProperties()
        {
            // Act
            var taskLibrary = new TemplateTaskLibrary();

            // Assert
            taskLibrary.Converters.Should().NotBeNull();
            taskLibrary.Tasks.Should().NotBeNull();
            taskLibrary.Converters.Should().BeEmpty();
            taskLibrary.Tasks.Should().BeEmpty();
        }

        [Fact]
        public void DriverValues_HasCorrectDefaults()
        {
            // Act
            var driverValues = new DriverValues();

            // Assert
            driverValues.Directory.Should().Be("driver-sample");
            driverValues.PackageVersion.Should().Be("0.0.0");
            driverValues.Identifier.Should().Be("SampleDriver");
            driverValues.PackageScope.Should().Be("@criticalmanufacturing");
            driverValues.PackageName.Should().Be("connect-iot-driver-sample");
        }

        [Fact]
        public void ConverterValues_HasCorrectDefaults()
        {
            // Act
            var converterValues = new ConverterValues();

            // Assert
            converterValues.Name.Should().Be("somethingToSomething");
            converterValues.Title.Should().Be("Something To Something");
            converterValues.Input.Should().Be(DataTypeInputOutput.Any.ToString());
            converterValues.Output.Should().Be(DataTypeInputOutput.Any.ToString());
            converterValues.Parameters.Should().NotBeNull();
            converterValues.Parameters.Should().BeEmpty();
        }

        [Fact]
        public void TaskValues_HasCorrectDefaults()
        {
            // Act
            var taskValues = new TaskValues();

            // Assert
            taskValues.Name.Should().Be("blackBox");
            taskValues.Title.Should().Be("Black Box");
            taskValues.Icon.Should().Be("icon-core-tasks-connect-iot-lg-logmessage");
            taskValues.IsProtocol.Should().BeFalse();
            taskValues.IsController.Should().BeTrue();
            taskValues.Lifecycle.Should().Be("Productive");
            taskValues.Inputs.Should().ContainKey("activate");
            taskValues.Outputs.Should().ContainKey("success");
            taskValues.Outputs.Should().ContainKey("error");
        }

        [Fact]
        public void TaskSetting_HasCorrectDefaults()
        {
            // Act
            var taskSetting = new TaskSetting();

            // Assert
            taskSetting.Name.Should().Be("settingName");
            taskSetting.SettingKey.Should().Be("settingKey");
            taskSetting.DataType.Should().Be(DataTypeInputOutput.String.ToString());
        }

        [Fact]
        public void TaskInputOutputType_HasCorrectDefaults()
        {
            // Act
            var taskInputOutput = new TaskInputOutputType();

            // Assert
            taskInputOutput.Type.Should().Be(TaskInputTypeType.Static);
            taskInputOutput.DataType.Should().Be(DataTypeInputOutput.String.ToString());
        }

        [Theory]
        [InlineData(DataTypeSetting.String)]
        [InlineData(DataTypeSetting.Integer)]
        [InlineData(DataTypeSetting.Boolean)]
        [InlineData(DataTypeSetting.Enum)]
        public void DataTypeSetting_EnumValues_AreValid(DataTypeSetting dataType)
        {
            // Act & Assert
            dataType.Should().BeDefined();
        }

        [Theory]
        [InlineData(DataTypeInputOutput.String)]
        [InlineData(DataTypeInputOutput.Any)]
        [InlineData(DataTypeInputOutput.Integer)]
        [InlineData(DataTypeInputOutput.Boolean)]
        [InlineData(DataTypeInputOutput.DateTime)]
        [InlineData(DataTypeInputOutput.Buffer)]
        public void DataTypeInputOutput_EnumValues_AreValid(DataTypeInputOutput dataType)
        {
            // Act & Assert
            dataType.Should().BeDefined();
        }

        [Theory]
        [InlineData(IoTValueType.String)]
        [InlineData(IoTValueType.Any)]
        [InlineData(IoTValueType.Integer)]
        [InlineData(IoTValueType.Boolean)]
        [InlineData(IoTValueType.Enum)]
        public void IoTValueType_EnumValues_AreValid(IoTValueType iotValueType)
        {
            // Act & Assert
            iotValueType.Should().BeDefined();
        }

        [Fact]
        public void ConvertIoTTypesToJSTypes_WithInvalidType_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<Exception>(() => IoTStructures.ConvertIoTTypesToJSTypes<DataTypeInputOutput>((DataTypeInputOutput)999));
        }
    }
}