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
    public class PhoneServiceTests
    {
        private Mock<IPhoneRepository> _phoneRepoMock;
        private Mock<IManufacturerRepository> _manufacturerRepoMock;
        private PhoneService _service;

        [SetUp]
        public void SetUp()
        {
            _phoneRepoMock = new Mock<IPhoneRepository>();
            _manufacturerRepoMock = new Mock<IManufacturerRepository>();
            _service = new PhoneService(_phoneRepoMock.Object, _manufacturerRepoMock.Object);
        }

        [Test]
        public async Task GetAllPhonesAsync_ReturnsPhones()
        {
            var phones = new List<Phone> { new Phone { Id = 1, Model = "M", SerialNumber = "SN", PhoneType = "Type", ManufacturerId = 1 } };
            _phoneRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(phones);

            var result = await _service.GetAllPhonesAsync();

            Assert.That(phones, Is.EqualTo(result));
        }

        [Test]
        public void GetPhoneByIdAsync_IdLessThanOrEqualZero_ThrowsArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(() => _service.GetPhoneByIdAsync(0));
        }

        [Test]
        public async Task GetPhoneByIdAsync_ValidId_ReturnsPhone()
        {
            var phone = new Phone { Id = 1, Model = "M", SerialNumber = "SN", PhoneType = "Type", ManufacturerId = 1 };
            _phoneRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(phone);

            var result = await _service.GetPhoneByIdAsync(1);

            Assert.That(phone, Is.EqualTo(result));
        }

        [Test]
        public void GetPhonesByManufacturerIdAsync_IdLessThanOrEqualZero_ThrowsArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(() => _service.GetPhonesByManufacturerIdAsync(0));
        }

        [Test]
        public void GetPhonesByManufacturerIdAsync_ManufacturerNotFound_ThrowsArgumentException()
        {
            _manufacturerRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Manufacturer)null);

            var ex = Assert.ThrowsAsync<ArgumentException>(() => _service.GetPhonesByManufacturerIdAsync(1));
            Assert.That(ex.Message, Does.Contain("not found"));
        }

        [Test]
        public async Task GetPhonesByManufacturerIdAsync_ValidId_ReturnsPhones()
        {
            var manufacturer = new Manufacturer { Id = 1, Name = "Test", Address = "Addr", IsAChildCompany = false };
            var phones = new List<Phone> { new Phone { Id = 1, Model = "M", SerialNumber = "SN", PhoneType = "Type", ManufacturerId = 1 } };
            _manufacturerRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(manufacturer);
            _phoneRepoMock.Setup(r => r.GetByManufacturerIdAsync(1)).ReturnsAsync(phones);

            var result = await _service.GetPhonesByManufacturerIdAsync(1);

            Assert.That(phones, Is.EqualTo(result));
        }

        [Test]
        public void CreatePhoneAsync_InvalidModel_ThrowsArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(() => _service.CreatePhoneAsync("", "SN", "Type", 1));
        }

        [Test]
        public void CreatePhoneAsync_InvalidSerialNumber_ThrowsArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(() => _service.CreatePhoneAsync("Model", "", "Type", 1));
        }

        [Test]
        public void CreatePhoneAsync_InvalidPhoneType_ThrowsArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(() => _service.CreatePhoneAsync("Model", "SN", "", 1));
        }

        [Test]
        public void CreatePhoneAsync_InvalidManufacturerId_ThrowsArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(() => _service.CreatePhoneAsync("Model", "SN", "Type", 0));
        }

        [Test]
        public void CreatePhoneAsync_ManufacturerNotFound_ThrowsArgumentException()
        {
            _manufacturerRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Manufacturer)null);

            var ex = Assert.ThrowsAsync<ArgumentException>(() => _service.CreatePhoneAsync("Model", "SN", "Type", 1));
            Assert.That(ex.Message, Does.Contain("not found"));
        }

        [Test]
        public async Task CreatePhoneAsync_ValidInput_CallsRepository()
        {
            var manufacturer = new Manufacturer { Id = 1, Name = "Test", Address = "Addr", IsAChildCompany = false };
            _manufacturerRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(manufacturer);
            _phoneRepoMock.Setup(r => r.AddAsync(It.IsAny<Phone>())).ReturnsAsync(1);

            var result = await _service.CreatePhoneAsync("Model", "SN", "Type", 1);

            Assert.That(1, Is.EqualTo(result));
            _phoneRepoMock.Verify(r => r.AddAsync(It.Is<Phone>(p => p.Model == "Model" && p.SerialNumber == "SN" && p.PhoneType == "Type" && p.ManufacturerId == 1)), Times.Once);
        }

        [Test]
        public void UpdatePhoneAsync_IdLessThanOrEqualZero_ThrowsArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(() => _service.UpdatePhoneAsync(0, "Model", "SN", "Type", 1));
        }

        [Test]
        public async Task UpdatePhoneAsync_ValidInput_CallsRepository()
        {
            var manufacturer = new Manufacturer { Id = 1, Name = "Test", Address = "Addr", IsAChildCompany = false };
            _manufacturerRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(manufacturer);
            _phoneRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Phone>())).ReturnsAsync(true);

            var result = await _service.UpdatePhoneAsync(1, "Model", "SN", "Type", 1);

            Assert.That(result, Is.True);
            _phoneRepoMock.Verify(r => r.UpdateAsync(It.Is<Phone>(p => p.Id == 1)), Times.Once);
        }

        [Test]
        public void DeletePhoneAsync_IdLessThanOrEqualZero_ThrowsArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(() => _service.DeletePhoneAsync(0));
        }

        [Test]
        public async Task DeletePhoneAsync_ValidId_CallsRepository()
        {
            _phoneRepoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _service.DeletePhoneAsync(1);

            Assert.That(result, Is.True);
            _phoneRepoMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }
    }
}
