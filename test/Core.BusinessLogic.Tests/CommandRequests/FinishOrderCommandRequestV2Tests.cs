using System;
using System.Diagnostics.CodeAnalysis;
using Core.BusinessLogic.CommandRequests;
using Xunit;

namespace Core.BusinessLogic.Tests.CommandRequests
{
    [ExcludeFromCodeCoverage]
    [Trait("Category", "Unit")]
    public class FinishOrderCommandRequestV2Tests
    {
        [Fact]
        public void Ctor__FinishDateTimeIsNotNull()
        {
            // Arrange
            // Act
            var request = new FinishOrderCommandRequestV2();
            // Assert
            Assert.NotNull(request.FinishDateTime);
        }

        [Fact]
        public void Ctor__FinishDateTimeIsNotDefault()
        {
            // Arrange
            var request = new FinishOrderCommandRequestV2();
            // Act
            var finishDateTime = request.FinishDateTime;
            // Assert
            Assert.NotEqual(default(DateTime), finishDateTime);
        }
    }
}