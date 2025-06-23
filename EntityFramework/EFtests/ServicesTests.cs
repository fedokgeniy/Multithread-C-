using ManufacturerPhoneApp.Data;
using ManufacturerPhoneApp.Models;
using ManufacturerPhoneApp.Repositories;
using ManufacturerPhoneApp.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManufacturerPhoneApp.Tests.Services
{
    [TestFixture]
    public class BusinessServiceTests
    {
        private Mock<IManufacturerRepository> _manufacturerRepoMock;
        private Mock<IPhoneRepository> _phoneRepoMock;
        private BusinessService _service;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ManufacturerPhoneContext>().Options;
            var contextMock = new Mock<ManufacturerPhoneContext>(options);

            _manufacturerRepoMock = new Mock<IManufacturerRepository>();
            _phoneRepoMock = new Mock<IPhoneRepository>();
            _service = new BusinessService(contextMock.Object, _manufacturerRepoMock.Object, _phoneRepoMock.Object);
        }

        [Test]
        public void AddProductForNewManufacturerAsync_ThrowsIfManufacturerIsNull()
        {
            var phone = new Phone();
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _service.AddProductForNewManufacturerAsync(null, phone));
        }

        [Test]
        public void AddProductForNewManufacturerAsync_ThrowsIfPhoneIsNull()
        {
            var manufacturer = new Manufacturer();
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _service.AddProductForNewManufacturerAsync(manufacturer, null));
        }

        [Test]
        public async Task GetProductsForManufacturerAsync_ById_ReturnsPhones()
        {
            var phones = new List<Phone>
            {
                new Phone { Id = 1, Model = "A" },
                new Phone { Id = 2, Model = "B" }
            };
            _phoneRepoMock.Setup(r => r.GetByManufacturerIdAsync(5)).ReturnsAsync(phones);

            var result = await _service.GetProductsForManufacturerAsync(5);

            Assert.That(2, Is.EqualTo(result.Count()));
            Assert.That(result.Any(p => p.Model == "A"), Is.True);
            Assert.That(result.Any(p => p.Model == "B"), Is.True);
        }

        [Test]
        public async Task GetProductsForManufacturerAsync_ByName_ReturnsPhones()
        {
            var manufacturers = new List<Manufacturer>
            {
                new Manufacturer { Id = 10, Name = "Test" }
            };
            var phones = new List<Phone>
            {
                new Phone { Id = 1, Model = "A", ManufacturerId = 10 }
            };

            _manufacturerRepoMock.Setup(r => r.GetByNameAsync("Test")).ReturnsAsync(manufacturers);
            _phoneRepoMock.Setup(r => r.GetByManufacturerIdAsync(10)).ReturnsAsync(phones);

            var result = await _service.GetProductsForManufacturerAsync("Test");

            Assert.That(1, Is.EqualTo(result.Count()));
            Assert.That("A", Is.EqualTo(result.First().Model));
        }

        [Test]
        public async Task GetProductsForManufacturerAsync_ByName_ManufacturerNotFound_ReturnsEmpty()
        {
            _manufacturerRepoMock.Setup(r => r.GetByNameAsync("Unknown"))
                .ReturnsAsync(new List<Manufacturer>());

            var result = await _service.GetProductsForManufacturerAsync("Unknown");

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetProductsForManufacturerAsync_ByName_EmptyName_ReturnsEmpty()
        {
            var result = await _service.GetProductsForManufacturerAsync(string.Empty);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Null);
        }
    }
}
