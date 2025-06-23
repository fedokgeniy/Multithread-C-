using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using PhoneInheritanceDemo.Data;
using PhoneInheritanceDemo.Models;
using PhoneInheritanceDemo.Services;

namespace PhoneInheritanceDemo.Tests.Services
{
    /// <summary>
    /// Unit tests for TpcPhoneService (Table-Per-Concrete-Type strategy).
    /// Tests specific behavior for complete tables per concrete type with unions.
    /// </summary>
    [TestFixture]
    public class TpcPhoneServiceTests
    {
        private Mock<TpcPhoneContext> _mockContext;
        private TpcPhoneService _service;
        /// <summary>
        /// Sets up test fixtures before each test method.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _mockContext = new Mock<TpcPhoneContext>();
            _service = new TpcPhoneService(_mockContext.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _service?.Dispose();
        }

        /// <summary>
        /// Tests that StrategyName returns correct value for TPC.
        /// </summary>
        [Test]
        public void StrategyName_Should_ReturnCorrectValue()
        {
            _service.StrategyName.Should().Be("Table-Per-Concrete-Type (TPC)");
        }

        /// <summary>
        /// Tests constructor validation for null context.
        /// </summary>
        [Test]
        public void Constructor_Should_ThrowArgumentNullException_WhenContextIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new TpcPhoneService(null!));
            exception!.ParamName.Should().Be("context");
        }

        /// <summary>
        /// Tests the dispose functionality of the service.
        /// </summary>
        [Test]
        public void Dispose_Should_DisposeContext()
        {
            _service.Dispose();

            _mockContext.Verify(c => c.Dispose(), Times.Once);
        }
    }
}