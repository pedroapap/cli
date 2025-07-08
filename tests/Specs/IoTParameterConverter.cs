using Cmf.CLI.Utilities;
using FluentAssertions;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;

namespace tests.Specs
{
    public class IoTParameterConverterTests
    {
        private readonly IoTParametersConverter _converter;
        private readonly JsonSerializer _serializer;

        public IoTParameterConverterTests()
        {
            _converter = new IoTParametersConverter();
            _serializer = new JsonSerializer();
        }

        [Fact]
        public void WriteJson_WithStringValueType_WritesStringValue()
        {
            // Arrange
            var parameters = new Dictionary<string, IoTValueType>
            {
                ["param1"] = IoTValueType.String
            };

            // Act
            var json = JsonConvert.SerializeObject(parameters, _converter);

            // Assert
            json.Should().Contain("\"param1\":\"String\"");
        }

        [Fact]
        public void WriteJson_WithIntegerValueType_WritesIntegerValue()
        {
            // Arrange
            var parameters = new Dictionary<string, IoTValueType>
            {
                ["param1"] = IoTValueType.Integer
            };

            // Act
            var json = JsonConvert.SerializeObject(parameters, _converter);

            // Assert
            json.Should().Contain("\"param1\":\"Integer\"");
        }

        [Fact]
        public void WriteJson_WithBooleanValueType_WritesBooleanValue()
        {
            // Arrange
            var parameters = new Dictionary<string, IoTValueType>
            {
                ["param1"] = IoTValueType.Boolean
            };

            // Act
            var json = JsonConvert.SerializeObject(parameters, _converter);

            // Assert
            json.Should().Contain("\"param1\":\"Boolean\"");
        }

        [Fact]
        public void WriteJson_WithDecimalValueType_WritesDecimalValue()
        {
            // Arrange
            var parameters = new Dictionary<string, IoTValueType>
            {
                ["param1"] = IoTValueType.Decimal
            };

            // Act
            var json = JsonConvert.SerializeObject(parameters, _converter);

            // Assert
            json.Should().Contain("\"param1\":\"Decimal\"");
        }

        [Fact]
        public void WriteJson_WithEnumValueType_WritesEnumObject()
        {
            // Arrange
            var parameters = new Dictionary<string, IoTValueType>
            {
                ["param1"] = IoTValueType.Enum
            };

            // Act
            var json = JsonConvert.SerializeObject(parameters, _converter);

            // Assert
            json.Should().Contain("\"param1\":");
            json.Should().Contain("\"dataType\":\"Enum\"");
            json.Should().Contain("\"enumValues\":");
            json.Should().Contain("\"First Option\"");
            json.Should().Contain("\"Second Option\"");
            json.Should().Contain("\"etc\"");
        }

        [Fact]
        public void WriteJson_WithMultipleParameters_WritesAllParameters()
        {
            // Arrange
            var parameters = new Dictionary<string, IoTValueType>
            {
                ["stringParam"] = IoTValueType.String,
                ["intParam"] = IoTValueType.Integer,
                ["enumParam"] = IoTValueType.Enum
            };

            // Act
            var json = JsonConvert.SerializeObject(parameters, _converter);

            // Assert
            json.Should().Contain("\"stringParam\":\"String\"");
            json.Should().Contain("\"intParam\":\"Integer\"");
            json.Should().Contain("\"enumParam\":");
            json.Should().Contain("\"dataType\":\"Enum\"");
        }

        [Fact]
        public void ReadJson_WithStringValue_ReturnsStringValueType()
        {
            // Arrange
            var json = "{\"param1\":\"String\"}";

            // Act
            var result = JsonConvert.DeserializeObject<Dictionary<string, IoTValueType>>(json, _converter);

            // Assert
            result.Should().ContainKey("param1");
            result["param1"].Should().Be(IoTValueType.String);
        }

        [Fact]
        public void ReadJson_WithIntegerValue_ReturnsIntegerValueType()
        {
            // Arrange
            var json = "{\"param1\":\"Integer\"}";

            // Act
            var result = JsonConvert.DeserializeObject<Dictionary<string, IoTValueType>>(json, _converter);

            // Assert
            result.Should().ContainKey("param1");
            result["param1"].Should().Be(IoTValueType.Integer);
        }

        [Fact]
        public void ReadJson_WithBooleanValue_ReturnsBooleanValueType()
        {
            // Arrange
            var json = "{\"param1\":\"Boolean\"}";

            // Act
            var result = JsonConvert.DeserializeObject<Dictionary<string, IoTValueType>>(json, _converter);

            // Assert
            result.Should().ContainKey("param1");
            result["param1"].Should().Be(IoTValueType.Boolean);
        }

        [Fact]
        public void ReadJson_WithDecimalValue_ReturnsDecimalValueType()
        {
            // Arrange
            var json = "{\"param1\":\"Decimal\"}";

            // Act
            var result = JsonConvert.DeserializeObject<Dictionary<string, IoTValueType>>(json, _converter);

            // Assert
            result.Should().ContainKey("param1");
            result["param1"].Should().Be(IoTValueType.Decimal);
        }

        [Fact]
        public void ReadJson_WithEnumObject_ReturnsEnumValueType()
        {
            // Arrange
            var json = "{\"param1\":{\"dataType\":\"Enum\",\"enumValues\":[\"Option1\",\"Option2\"]}}";

            // Act
            var result = JsonConvert.DeserializeObject<Dictionary<string, IoTValueType>>(json, _converter);

            // Assert
            result.Should().ContainKey("param1");
            result["param1"].Should().Be(IoTValueType.Enum);
        }

        [Fact]
        public void ReadJson_WithMultipleParameters_ReturnsAllParameters()
        {
            // Arrange
            var json = "{\"stringParam\":\"String\",\"intParam\":\"Integer\",\"enumParam\":{\"dataType\":\"Enum\",\"enumValues\":[\"A\",\"B\"]}}";

            // Act
            var result = JsonConvert.DeserializeObject<Dictionary<string, IoTValueType>>(json, _converter);

            // Assert
            result.Should().HaveCount(3);
            result["stringParam"].Should().Be(IoTValueType.String);
            result["intParam"].Should().Be(IoTValueType.Integer);
            result["enumParam"].Should().Be(IoTValueType.Enum);
        }

        [Fact]
        public void ReadJson_WithEmptyObject_ReturnsEmptyDictionary()
        {
            // Arrange
            var json = "{}";

            // Act
            var result = JsonConvert.DeserializeObject<Dictionary<string, IoTValueType>>(json, _converter);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void RoundTrip_SerializeAndDeserialize_PreservesOriginalData()
        {
            // Arrange
            var original = new Dictionary<string, IoTValueType>
            {
                ["stringParam"] = IoTValueType.String,
                ["intParam"] = IoTValueType.Integer,
                ["boolParam"] = IoTValueType.Boolean,
                ["decimalParam"] = IoTValueType.Decimal,
                ["enumParam"] = IoTValueType.Enum
            };

            // Act
            var json = JsonConvert.SerializeObject(original, _converter);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, IoTValueType>>(json, _converter);

            // Assert
            deserialized.Should().BeEquivalentTo(original);
        }
    }
}