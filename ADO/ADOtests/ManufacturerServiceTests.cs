using NUnit.Framework;
using Moq;
using ManufacturerPhoneApp.Services;
using ManufacturerPhoneApp.Interfaces;
using ManufacturerPhoneApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManufacturerPhoneApp.Tests.Services
{
    [TestFixture]
    public class ManufacturerServiceTests
    {
        private Mock<IManufacturerRepository> _repositoryMock;
        private ManufacturerService _service;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IManufacturerRepository>();
            _service = new ManufacturerService(_repositoryMock.Object);
        }

        [Test]
        public async Task GetAllManufacturersAsync_ReturnsManufacturers()
        {
            var manufacturers = new List<Manufacturer> { new Manufacturer { Id = 1, Name = "Test", Address = "Addr", IsAChildCompany = false } };
            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(manufacturers);

            var result = await _service.GetAllManufacturersAsync();

            Assert.That(manufacturers, Is.EqualTo(result));
        }

        [Test]
        public void GetManufacturerByIdAsync_IdLessThanOrEqualZero_ThrowsArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(() => _service.GetManufacturerByIdAsync(0));
            Assert.ThrowsAsync<ArgumentException>(() => _service.GetManufacturerByIdAsync(-1));
        }

        [Test]
        public async Task GetManufacturerByIdAsync_ValidId_ReturnsManufacturer()
        {
            var manufacturer = new Manufacturer { Id = 1, Name = "Test", Address = "Addr", IsAChildCompany = false };
            _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(manufacturer);

            var result = await _service.GetManufacturerByIdAsync(1);

            Assert.That(manufacturer, Is.EqualTo(result));
        }

        [Test]
        public void CreateManufacturerAsync_EmptyName_ThrowsArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(() => _service.CreateManufacturerAsync("", "Addr", false));
        }

        [Test]
        public void CreateManufacturerAsync_EmptyAddress_ThrowsArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(() => _service.CreateManufacturerAsync("Name", "", false));
        }

        [Test]
        public async Task CreateManufacturerAsync_ValidInput_CallsRepository()
        {
            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Manufacturer>())).ReturnsAsync(1);

            var result = await _service.CreateManufacturerAsync("Name", "Address", false);

            Assert.That(1, Is.EqualTo(result));
            _repositoryMock.Verify(r => r.AddAsync(It.Is<Manufacturer>(m => m.Name == "Name" && m.Address == "Address" && m.IsAChildCompany == false)), Times.Once);
        }

        [Test]
        public void UpdateManufacturerAsync_IdLessThanOrEqualZero_ThrowsArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateManufacturerAsync(0, "Name", "Addr", false));
        }

        [Test]
        public async Task UpdateManufacturerAsync_ValidInput_CallsRepository()
        {
            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Manufacturer>())).ReturnsAsync(true);

            var result = await _service.UpdateManufacturerAsync(1, "Name", "Address", true);

            Assert.That(result, Is.True);
            _repositoryMock.Verify(r => r.UpdateAsync(It.Is<Manufacturer>(m => m.Id == 1)), Times.Once);
        }

        [Test]
        public void DeleteManufacturerAsync_IdLessThanOrEqualZero_ThrowsArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(() => _service.DeleteManufacturerAsync(0));
        }

        [Test]
        public async Task DeleteManufacturerAsync_ValidId_CallsRepository()
        {
            _repositoryMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _service.DeleteManufacturerAsync(1);

            Assert.That(result, Is.True);
            _repositoryMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }
    }
}
