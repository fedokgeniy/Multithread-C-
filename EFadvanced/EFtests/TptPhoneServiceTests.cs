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
    /// Unit tests for TptPhoneService (Table-Per-Type strategy).
    /// Tests specific behavior for separate tables per type with joins.
    /// </summary>
    [TestFixture]
    public class TptPhoneServiceTests
    {
        private Mock<TptPhoneContext> _mockContext;
        private TptPhoneService _service;

        /// <summary>
        /// Sets up test fixtures before each test method.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _mockContext = new Mock<TptPhoneContext>();
            _service = new TptPhoneService(_mockContext.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _service?.Dispose();
        }

        /// <summary>
        /// Tests that StrategyName returns correct value for TPT.
        /// </summary>
        [Test]
        public void StrategyName_Should_ReturnCorrectValue()
        {
            _service.StrategyName.Should().Be("Table-Per-Type (TPT)");
        }

        /// <summary>
        /// Tests constructor validation for null context.
        /// </summary>
        [Test]
        public void Constructor_Should_ThrowArgumentNullException_WhenContextIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new TptPhoneService(null!));
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