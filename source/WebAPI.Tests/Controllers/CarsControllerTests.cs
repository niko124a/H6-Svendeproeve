using Common.Entities;
using Common.Helpers;
using DatabaseAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Controllers;
using WebAPI.Models.Car;

namespace WebAPI.Tests.Controllers
{
    public class CarsControllerTests
    {
        private Mock<IUserRepository> mockUserRepo = new Mock<IUserRepository>();
        private Mock<ICarRepository> mockCarRepo = new Mock<ICarRepository>();
        private Mock<CarRegistrationHelper> mockCarRegHelper = new Mock<CarRegistrationHelper>();

        private List<Car> mockCars = new List<Car>();


        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task GetAllCars_ReturnsOk()
        {
            // Arrange
            mockCars.Add(new Car()
            {
                Id = 1,
                User = new User()
                {
                    Id = 1,
                    Username = "mockUsername",
                    PasswordHash = "mockPasswordHash",
                    Salt = new byte[] { },
                    FirstName = "mockFirstname",
                    LastName = "mockLastname",
                    Email = "mock@email.com",
                    PhoneNumber = "mockPhone",
                    Role = new Role() { Id = 1, Name = "mockRolename" }
                },
                Registration = "mockRegistration",
                Vin = "mockVin",
                Status = "mockStatus",
                Kind = "mockKind",
                Usage = "mockUsage",
                Category = "mockCategory",
                ModelYear = 9999,
                Brand = "mockBrand",
                Model = "mockModel",
                Variant = "mockVariant",
                FuelType = "mockFuelType",
                ExtraEquipment = "mockExtra"
            });
            mockCarRepo.Setup(cr => cr.GetAllCarsAsync())
                .ReturnsAsync(mockCars);

            var controller = new CarsController(mockCarRepo.Object, mockUserRepo.Object, mockCarRegHelper.Object);

            // Act
            var response = (OkObjectResult)await controller.GetAllCars();

            // Assert
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(200));
            Assert.That(response.Value, Is.EqualTo(mockCars));
        }

        [Test]
        public async Task GetAllCars_ReturnsNoContent_WhenCarsDatabaseTableIsEmpty()
        {
            // Arrange
            mockCarRepo.Setup(cr => cr.GetAllCarsAsync())
                .ReturnsAsync(mockCars);

            var controller = new CarsController(mockCarRepo.Object, mockUserRepo.Object, mockCarRegHelper.Object);

            // Act
            var response = (NoContentResult)await controller.GetAllCars();

            // Assert
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(204));
        }

        [Test]
        public async Task GetCarsByUserId_ReturnsOk()
        {
            // Arrange
            int userId = 1;
            mockCars.Add(new Car()
            {
                Id = 1,
                User = new User()
                {
                    Id = 1,
                    Username = "mockUsername",
                    PasswordHash = "mockPasswordHash",
                    Salt = new byte[] { },
                    FirstName = "mockFirstname",
                    LastName = "mockLastname",
                    Email = "mock@email.com",
                    PhoneNumber = "mockPhone",
                    Role = new Role() { Id = 1, Name = "mockRolename" }
                },
                Registration = "mockRegistration",
                Vin = "mockVin",
                Status = "mockStatus",
                Kind = "mockKind",
                Usage = "mockUsage",
                Category = "mockCategory",
                ModelYear = 9999,
                Brand = "mockBrand",
                Model = "mockModel",
                Variant = "mockVariant",
                FuelType = "mockFuelType",
                ExtraEquipment = "mockExtra"
            });
            mockCarRepo.Setup(cr => cr.GetCarsByUserIdAsync(userId))
                .ReturnsAsync(mockCars);

            var controller = new CarsController(mockCarRepo.Object, mockUserRepo.Object, mockCarRegHelper.Object);

            // Act
            var response = (OkObjectResult)await controller.GetCarsByUserId(userId);

            // Assert
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(200));
            Assert.That(response.Value, Is.EqualTo(mockCars));
        }

        [Test]
        public async Task GetCarsByUserId_ReturnsBadRequest_WhenIdIsLessThanOne()
        {
            // Arrange
            int userId = 0;

            var controller = new CarsController(mockCarRepo.Object, mockUserRepo.Object, mockCarRegHelper.Object);

            // Act
            var response = (BadRequestObjectResult)await controller.GetCarsByUserId(userId);

            // Assert
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public async Task GetCarsByUserId_ReturnsNoContent_WhenNoCarsWithGivenUserIdExist()
        {
            // Arrange
            int userId = 1;
            mockCarRepo.Setup(cr => cr.GetCarsByUserIdAsync(userId))
                .ReturnsAsync(mockCars);

            var controller = new CarsController(mockCarRepo.Object, mockUserRepo.Object, mockCarRegHelper.Object);

            // Act
            var response = (NoContentResult)await controller.GetCarsByUserId(userId);

            // Assert
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(204));
        }

        [Test]
        public async Task GetCarById_ReturnsOk()
        {
            // Arrange
            int carId = 1;
            var mockCar = new Car()
            {
                Id = 1,
                User = new User()
                {
                    Id = 1,
                    Username = "mockUsername",
                    PasswordHash = "mockPasswordHash",
                    Salt = new byte[] { },
                    FirstName = "mockFirstname",
                    LastName = "mockLastname",
                    Email = "mock@email.com",
                    PhoneNumber = "mockPhone",
                    Role = new Role() { Id = 1, Name = "mockRolename" }
                },
                Registration = "mockRegistration",
                Vin = "mockVin",
                Status = "mockStatus",
                Kind = "mockKind",
                Usage = "mockUsage",
                Category = "mockCategory",
                ModelYear = 9999,
                Brand = "mockBrand",
                Model = "mockModel",
                Variant = "mockVariant",
                FuelType = "mockFuelType",
                ExtraEquipment = "mockExtra"
            };
            mockCarRepo.Setup(cr => cr.GetCarByIdAsync(carId))
                .ReturnsAsync(mockCar);

            var controller = new CarsController(mockCarRepo.Object, mockUserRepo.Object, mockCarRegHelper.Object);

            // Act
            var response = (OkObjectResult)await controller.GetCarById(carId);

            // Assert
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(200));
            Assert.That(response.Value, Is.EqualTo(mockCar));
        }

        [Test]
        public async Task GetCarById_ReturnsBadRequest_WhenIdIsLessThanOne()
        {
            // Arrange
            int carId = 0;

            var controller = new CarsController(mockCarRepo.Object, mockUserRepo.Object, mockCarRegHelper.Object);

            // Act
            var response = (BadRequestObjectResult)await controller.GetCarById(carId);

            // Assert
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public async Task GetCarById_ReturnsNotFound_WhenCarWithGivenIdDoesntExist()
        {
            // Arrange
            int carId = 1;
            var mockCar = new Car();
            mockCarRepo.Setup(cr => cr.GetCarByIdAsync(carId))
                .ReturnsAsync(mockCar = null);

            var controller = new CarsController(mockCarRepo.Object, mockUserRepo.Object, mockCarRegHelper.Object);

            // Act
            var response = (NotFoundResult)await controller.GetCarById(carId);

            // Assert
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public async Task GetCarByRegistrationAsync_ReturnsOk()
        {
            // Arrange
            string carRegistration = "AB12345";
            var mockCar = new Car()
            {
                Id = 1,
                User = new User()
                {
                    Id = 1,
                    Username = "mockUsername",
                    PasswordHash = "mockPasswordHash",
                    Salt = new byte[] { },
                    FirstName = "mockFirstname",
                    LastName = "mockLastname",
                    Email = "mock@email.com",
                    PhoneNumber = "mockPhone",
                    Role = new Role() { Id = 1, Name = "mockRolename" }
                },
                Registration = carRegistration,
                Vin = "mockVin",
                Status = "mockStatus",
                Kind = "mockKind",
                Usage = "mockUsage",
                Category = "mockCategory",
                ModelYear = 9999,
                Brand = "mockBrand",
                Model = "mockModel",
                Variant = "mockVariant",
                FuelType = "mockFuelType",
                ExtraEquipment = "mockExtra"
            };
            mockCarRepo.Setup(cr => cr.GetCarByRegistrationAsync(carRegistration))
                .ReturnsAsync(mockCar);

            var controller = new CarsController(mockCarRepo.Object, mockUserRepo.Object, mockCarRegHelper.Object);

            // Act
            var response = (OkObjectResult)await controller.GetCarByRegistrationAsync(carRegistration);

            // Assert
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(200));
            Assert.That(response.Value, Is.EqualTo(mockCar));
        }

        [Test]
        public async Task GetCarByRegistrationAsync_ReturnsBadRequest_WhenNormalRegistrationIsInvalid()
        {
            // Arrange
            string carRegistration = "ABC2345";
            var mockCar = new Car()
            {
                Id = 1,
                User = new User()
                {
                    Id = 1,
                    Username = "mockUsername",
                    PasswordHash = "mockPasswordHash",
                    Salt = new byte[] { },
                    FirstName = "mockFirstname",
                    LastName = "mockLastname",
                    Email = "mock@email.com",
                    PhoneNumber = "mockPhone",
                    Role = new Role() { Id = 1, Name = "mockRolename" }
                },
                Registration = carRegistration,
                Vin = "mockVin",
                Status = "mockStatus",
                Kind = "mockKind",
                Usage = "mockUsage",
                Category = "mockCategory",
                ModelYear = 9999,
                Brand = "mockBrand",
                Model = "mockModel",
                Variant = "mockVariant",
                FuelType = "mockFuelType",
                ExtraEquipment = "mockExtra"
            };
            mockCarRepo.Setup(cr => cr.GetCarByRegistrationAsync(carRegistration))
                .ReturnsAsync(mockCar);

            var controller = new CarsController(mockCarRepo.Object, mockUserRepo.Object, mockCarRegHelper.Object);

            // Act
            var response = (BadRequestObjectResult)await controller.GetCarByRegistrationAsync(carRegistration);

            // Assert
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public async Task GetCarByRegistrationAsync_ReturnsBadRequest_WhenCustomRegistrationIsInvalid()
        {
            // Arrange
            string carRegistration = "ABCDEFGH";
            var mockCar = new Car()
            {
                Id = 1,
                User = new User()
                {
                    Id = 1,
                    Username = "mockUsername",
                    PasswordHash = "mockPasswordHash",
                    Salt = new byte[] { },
                    FirstName = "mockFirstname",
                    LastName = "mockLastname",
                    Email = "mock@email.com",
                    PhoneNumber = "mockPhone",
                    Role = new Role() { Id = 1, Name = "mockRolename" }
                },
                Registration = carRegistration,
                Vin = "mockVin",
                Status = "mockStatus",
                Kind = "mockKind",
                Usage = "mockUsage",
                Category = "mockCategory",
                ModelYear = 9999,
                Brand = "mockBrand",
                Model = "mockModel",
                Variant = "mockVariant",
                FuelType = "mockFuelType",
                ExtraEquipment = "mockExtra"
            };
            mockCarRepo.Setup(cr => cr.GetCarByRegistrationAsync(carRegistration))
                .ReturnsAsync(mockCar);

            var controller = new CarsController(mockCarRepo.Object, mockUserRepo.Object, mockCarRegHelper.Object);

            // Act
            var response = (BadRequestObjectResult)await controller.GetCarByRegistrationAsync(carRegistration, true);

            // Assert
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public async Task GetCarByRegistrationAsync_ReturnsNotFound_WhenCarWithGivenRegistrationDoesntExist()
        {
            // Arrange
            string carRegistration = "AB12345";
            var mockCar = new Car();
            mockCarRepo.Setup(cr => cr.GetCarByRegistrationAsync(carRegistration))
                .ReturnsAsync(mockCar = null);

            var controller = new CarsController(mockCarRepo.Object, mockUserRepo.Object, mockCarRegHelper.Object);

            // Act
            var response = (NotFoundResult)await controller.GetCarByRegistrationAsync(carRegistration);

            // Assert
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public async Task CreateCar_ReturnsCreatedAt()
        {
            // Arrange
            var mockCarDto = new CarDto()
            {
                UserId = 1,
                Registration = "AB12345",
                Vin = "mockVin",
                Status = "mockStatus",
                Kind = "mockKind",
                Usage = "mockUsage",
                Category = "mockCategory",
                ModelYear = 9999,
                Brand = "mockBrand",
                Model = "mockModel",
                Variant = "mockVariant",
                FuelType = "mockFuelType",
                ExtraEquipment = "mockExtra"
            };
            var mockUser = new User() 
            {
                Id = 1,
                Username = "mockUsername",
                PasswordHash = "mockPasswordHash",
                Salt = new byte[] { },
                FirstName = "mockFirstname",
                LastName = "mockLastname",
                Email = "mock@email.com",
                PhoneNumber = "mockPhone",
                Role = new Role() { Id = 1, Name = "mockRolename" }
            };
            var mockCar = new Car()
            {
                Id = 1,
                User = mockUser,
                Registration = mockCarDto.Registration,
                Vin = "mockVin",
                Status = "mockStatus",
                Kind = "mockKind",
                Usage = "mockUsage",
                Category = "mockCategory",
                ModelYear = 9999,
                Brand = "mockBrand",
                Model = "mockModel",
                Variant = "mockVariant",
                FuelType = "mockFuelType",
                ExtraEquipment = "mockExtra"
            };
            mockCarRepo.Setup(cr => cr.CarExistsAsync(mockCarDto.Registration))
                .ReturnsAsync(false);
            mockCarRepo.Setup(cr => cr.CreateCar(mockCar));
            mockCarRepo.Setup(cr => cr.SaveChangesAsync())
                .ReturnsAsync(true);
            mockUserRepo.Setup(cr => cr.GetUserByIdAsync(mockCarDto.UserId))
                .ReturnsAsync(mockUser);

            var controller = new CarsController(mockCarRepo.Object, mockUserRepo.Object, mockCarRegHelper.Object);

            // Act
            var response = (CreatedAtActionResult)await controller.CreateCar(mockCarDto);

            // Assert
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(201));
        }

        [Test]
        public async Task CreateCar_ReturnsBadRequest_WhenUserIdIsLessThanOne()
        {
            // Arrange
            var mockCarDto = new CarDto()
            {
                UserId = 0,
                Registration = "AB12345",
                Vin = "mockVin",
                Status = "mockStatus",
                Kind = "mockKind",
                Usage = "mockUsage",
                Category = "mockCategory",
                ModelYear = 9999,
                Brand = "mockBrand",
                Model = "mockModel",
                Variant = "mockVariant",
                FuelType = "mockFuelType",
                ExtraEquipment = "mockExtra"
            };

            var controller = new CarsController(mockCarRepo.Object, mockUserRepo.Object, mockCarRegHelper.Object);

            // Act
            var response = (BadRequestObjectResult)await controller.CreateCar(mockCarDto);

            // Assert
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public async Task CreateCar_ReturnsBadRequest_WhenNormalCarRegistrationIsInvalid()
        {
            // Arrange
            var mockCarDto = new CarDto()
            {
                UserId = 1,
                Registration = "ABC2345",
                Vin = "mockVin",
                Status = "mockStatus",
                Kind = "mockKind",
                Usage = "mockUsage",
                Category = "mockCategory",
                ModelYear = 9999,
                Brand = "mockBrand",
                Model = "mockModel",
                Variant = "mockVariant",
                FuelType = "mockFuelType",
                ExtraEquipment = "mockExtra"
            };

            var controller = new CarsController(mockCarRepo.Object, mockUserRepo.Object, mockCarRegHelper.Object);

            // Act
            var response = (BadRequestObjectResult)await controller.CreateCar(mockCarDto);

            // Assert
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public async Task CreateCar_ReturnsBadRequest_WhenCustomCarRegistrationIsInvalid()
        {
            // Arrange
            var mockCarDto = new CarDto()
            {
                UserId = 1,
                Registration = "ABCDEFGH",
                Vin = "mockVin",
                Status = "mockStatus",
                Kind = "mockKind",
                Usage = "mockUsage",
                Category = "mockCategory",
                ModelYear = 9999,
                Brand = "mockBrand",
                Model = "mockModel",
                Variant = "mockVariant",
                FuelType = "mockFuelType",
                ExtraEquipment = "mockExtra"
            };

            var controller = new CarsController(mockCarRepo.Object, mockUserRepo.Object, mockCarRegHelper.Object);

            // Act
            var response = (BadRequestObjectResult)await controller.CreateCar(mockCarDto, true);

            // Assert
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public async Task CreateCar_ReturnsBadRequest_WhenCarAlreadyExist()
        {
            // Arrange
            var mockCarDto = new CarDto()
            {
                UserId = 1,
                Registration = "AB12345",
                Vin = "mockVin",
                Status = "mockStatus",
                Kind = "mockKind",
                Usage = "mockUsage",
                Category = "mockCategory",
                ModelYear = 9999,
                Brand = "mockBrand",
                Model = "mockModel",
                Variant = "mockVariant",
                FuelType = "mockFuelType",
                ExtraEquipment = "mockExtra"
            };
            mockCarRepo.Setup(cr => cr.CarExistsAsync(mockCarDto.Registration))
                .ReturnsAsync(true);

            var controller = new CarsController(mockCarRepo.Object, mockUserRepo.Object, mockCarRegHelper.Object);

            // Act
            var response = (BadRequestObjectResult)await controller.CreateCar(mockCarDto, true);

            // Assert
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public async Task CreateCar_ReturnsNotFound_WhenUserDoesntExist()
        {
            // Arrange
            var mockCarDto = new CarDto()
            {
                UserId = 1,
                Registration = "AB12345",
                Vin = "mockVin",
                Status = "mockStatus",
                Kind = "mockKind",
                Usage = "mockUsage",
                Category = "mockCategory",
                ModelYear = 9999,
                Brand = "mockBrand",
                Model = "mockModel",
                Variant = "mockVariant",
                FuelType = "mockFuelType",
                ExtraEquipment = "mockExtra"
            };
            var mockUser = new User();
            mockCarRepo.Setup(cr => cr.CarExistsAsync(mockCarDto.Registration))
                .ReturnsAsync(false);
            mockUserRepo.Setup(cr => cr.GetUserByIdAsync(mockCarDto.UserId))
                .ReturnsAsync(mockUser = null);

            var controller = new CarsController(mockCarRepo.Object, mockUserRepo.Object, mockCarRegHelper.Object);

            // Act
            var response = (NotFoundObjectResult)await controller.CreateCar(mockCarDto, true);

            // Assert
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public async Task UpdateCar_ReturnsOk()
        {
            // Arrange
            var mockCarDto = new CarDto()
            {
                UserId = 1,
                Registration = "AB12345",
                Vin = "mockVin",
                Status = "mockStatus",
                Kind = "mockKind",
                Usage = "mockUsage",
                Category = "mockCategory",
                ModelYear = 9999,
                Brand = "mockBrand",
                Model = "mockModel",
                Variant = "mockVariant",
                FuelType = "mockFuelType",
                ExtraEquipment = "mockExtra"
            };
            var mockUser = new User()
            {
                Id = 1,
                Username = "mockUsername",
                PasswordHash = "mockPasswordHash",
                Salt = new byte[] { },
                FirstName = "mockFirstname",
                LastName = "mockLastname",
                Email = "mock@email.com",
                PhoneNumber = "mockPhone",
                Role = new Role() { Id = 1, Name = "mockRolename" }
            };
            var mockCar = new Car()
            {
                Id = 1,
                User = mockUser,
                Registration = mockCarDto.Registration,
                Vin = "mockVin",
                Status = "mockStatus",
                Kind = "mockKind",
                Usage = "mockUsage",
                Category = "mockCategory",
                ModelYear = 9999,
                Brand = "mockBrand",
                Model = "mockModel",
                Variant = "mockVariant",
                FuelType = "mockFuelType",
                ExtraEquipment = "mockExtra"
            };
            mockCarRepo.Setup(cr => cr.GetCarByIdAsync(mockCar.Id))
                .ReturnsAsync(mockCar);
            mockCarRepo.Setup(cr => cr.SaveChangesAsync())
                .ReturnsAsync(true);
            mockUserRepo.Setup(cr => cr.GetUserByIdAsync(mockCarDto.UserId))
                .ReturnsAsync(mockUser);

            var controller = new CarsController(mockCarRepo.Object, mockUserRepo.Object, mockCarRegHelper.Object);

            // Act
            var response = (OkResult)await controller.UpdateCar(mockCar.Id, mockCarDto);

            // Assert
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task UpdateCar_ReturnsBadRequest_WhenCarIdIsLessThanOne()
        {
            // Arrange
            var mockCarDto = new CarDto()
            {
                UserId = 1,
                Registration = "AB12345",
                Vin = "mockVin",
                Status = "mockStatus",
                Kind = "mockKind",
                Usage = "mockUsage",
                Category = "mockCategory",
                ModelYear = 9999,
                Brand = "mockBrand",
                Model = "mockModel",
                Variant = "mockVariant",
                FuelType = "mockFuelType",
                ExtraEquipment = "mockExtra"
            };
            var mockCar = new Car()
            {
                Id = 0
            };

            var controller = new CarsController(mockCarRepo.Object, mockUserRepo.Object, mockCarRegHelper.Object);

            // Act
            var response = (BadRequestObjectResult)await controller.UpdateCar(mockCar.Id, mockCarDto);

            // Assert
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public async Task UpdateCar_ReturnsNotFound_WhenCarDoesntExist()
        {
            // Arrange
            int carId = 1;
            var mockCarDto = new CarDto()
            {
                UserId = 1,
                Registration = "AB12345",
                Vin = "mockVin",
                Status = "mockStatus",
                Kind = "mockKind",
                Usage = "mockUsage",
                Category = "mockCategory",
                ModelYear = 9999,
                Brand = "mockBrand",
                Model = "mockModel",
                Variant = "mockVariant",
                FuelType = "mockFuelType",
                ExtraEquipment = "mockExtra"
            };
            var mockCar = new Car();
            mockCarRepo.Setup(cr => cr.GetCarByIdAsync(carId))
                .ReturnsAsync(mockCar = null);

            var controller = new CarsController(mockCarRepo.Object, mockUserRepo.Object, mockCarRegHelper.Object);

            // Act
            var response = (NotFoundObjectResult)await controller.UpdateCar(carId, mockCarDto);

            // Assert
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public async Task UpdateCar_ReturnsNotFound_WhenUserDoesntExist()
        {
            // Arrange
            var mockCarDto = new CarDto()
            {
                UserId = 1,
                Registration = "AB12345",
                Vin = "mockVin",
                Status = "mockStatus",
                Kind = "mockKind",
                Usage = "mockUsage",
                Category = "mockCategory",
                ModelYear = 9999,
                Brand = "mockBrand",
                Model = "mockModel",
                Variant = "mockVariant",
                FuelType = "mockFuelType",
                ExtraEquipment = "mockExtra"
            };
            var mockCar = new Car()
            {
                Id = 1,
                User = new User()
                {
                    Id = 1,
                    Username = "mockUsername",
                    PasswordHash = "mockPasswordHash",
                    Salt = new byte[] { },
                    FirstName = "mockFirstname",
                    LastName = "mockLastname",
                    Email = "mock@email.com",
                    PhoneNumber = "mockPhone",
                    Role = new Role() { Id = 1, Name = "mockRolename" }
                },
                Registration = mockCarDto.Registration,
                Vin = "mockVin",
                Status = "mockStatus",
                Kind = "mockKind",
                Usage = "mockUsage",
                Category = "mockCategory",
                ModelYear = 9999,
                Brand = "mockBrand",
                Model = "mockModel",
                Variant = "mockVariant",
                FuelType = "mockFuelType",
                ExtraEquipment = "mockExtra"
            };
            var mockUser = new User();
            mockCarRepo.Setup(cr => cr.GetCarByIdAsync(mockCar.Id))
                .ReturnsAsync(mockCar);
            mockUserRepo.Setup(cr => cr.GetUserByIdAsync(mockCarDto.UserId))
                .ReturnsAsync(mockUser = null);

            var controller = new CarsController(mockCarRepo.Object, mockUserRepo.Object, mockCarRegHelper.Object);

            // Act
            var response = (NotFoundObjectResult)await controller.UpdateCar(mockCar.Id, mockCarDto);

            // Assert
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public async Task UpdateCar_ReturnsBadRequest_WhenNormalRegistrationIsInvalid()
        {
            // Arrange
            var mockCarDto = new CarDto()
            {
                UserId = 1,
                Registration = "ABC2345",
                Vin = "mockVin",
                Status = "mockStatus",
                Kind = "mockKind",
                Usage = "mockUsage",
                Category = "mockCategory",
                ModelYear = 9999,
                Brand = "mockBrand",
                Model = "mockModel",
                Variant = "mockVariant",
                FuelType = "mockFuelType",
                ExtraEquipment = "mockExtra"
            };
            var mockUser = new User()
            {
                Id = 1,
                Username = "mockUsername",
                PasswordHash = "mockPasswordHash",
                Salt = new byte[] { },
                FirstName = "mockFirstname",
                LastName = "mockLastname",
                Email = "mock@email.com",
                PhoneNumber = "mockPhone",
                Role = new Role() { Id = 1, Name = "mockRolename" }
            };
            var mockCar = new Car()
            {
                Id = 1,
                User = mockUser,
                Registration = mockCarDto.Registration,
                Vin = "mockVin",
                Status = "mockStatus",
                Kind = "mockKind",
                Usage = "mockUsage",
                Category = "mockCategory",
                ModelYear = 9999,
                Brand = "mockBrand",
                Model = "mockModel",
                Variant = "mockVariant",
                FuelType = "mockFuelType",
                ExtraEquipment = "mockExtra"
            };
            mockCarRepo.Setup(cr => cr.GetCarByIdAsync(mockCar.Id))
                .ReturnsAsync(mockCar);
            mockUserRepo.Setup(cr => cr.GetUserByIdAsync(mockCarDto.UserId))
                .ReturnsAsync(mockUser);

            var controller = new CarsController(mockCarRepo.Object, mockUserRepo.Object, mockCarRegHelper.Object);

            // Act
            var response = (BadRequestObjectResult)await controller.UpdateCar(mockCar.Id, mockCarDto);

            // Assert
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public async Task UpdateCar_ReturnsBadRequest_WhenCustomRegistrationIsInvalid()
        {
            // Arrange
            var mockCarDto = new CarDto()
            {
                UserId = 1,
                Registration = "ABCDEFGH",
                Vin = "mockVin",
                Status = "mockStatus",
                Kind = "mockKind",
                Usage = "mockUsage",
                Category = "mockCategory",
                ModelYear = 9999,
                Brand = "mockBrand",
                Model = "mockModel",
                Variant = "mockVariant",
                FuelType = "mockFuelType",
                ExtraEquipment = "mockExtra"
            };
            var mockUser = new User()
            {
                Id = 1,
                Username = "mockUsername",
                PasswordHash = "mockPasswordHash",
                Salt = new byte[] { },
                FirstName = "mockFirstname",
                LastName = "mockLastname",
                Email = "mock@email.com",
                PhoneNumber = "mockPhone",
                Role = new Role() { Id = 1, Name = "mockRolename" }
            };
            var mockCar = new Car()
            {
                Id = 1,
                User = mockUser,
                Registration = mockCarDto.Registration,
                Vin = "mockVin",
                Status = "mockStatus",
                Kind = "mockKind",
                Usage = "mockUsage",
                Category = "mockCategory",
                ModelYear = 9999,
                Brand = "mockBrand",
                Model = "mockModel",
                Variant = "mockVariant",
                FuelType = "mockFuelType",
                ExtraEquipment = "mockExtra"
            };
            mockCarRepo.Setup(cr => cr.GetCarByIdAsync(mockCar.Id))
                .ReturnsAsync(mockCar);
            mockUserRepo.Setup(cr => cr.GetUserByIdAsync(mockCarDto.UserId))
                .ReturnsAsync(mockUser);

            var controller = new CarsController(mockCarRepo.Object, mockUserRepo.Object, mockCarRegHelper.Object);

            // Act
            var response = (BadRequestObjectResult)await controller.UpdateCar(mockCar.Id, mockCarDto, true);

            // Assert
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public async Task DeleteCar_ReturnsOk()
        {
            // Arrange
            var mockUser = new User()
            {
                Id = 1,
                Username = "mockUsername",
                PasswordHash = "mockPasswordHash",
                Salt = new byte[] { },
                FirstName = "mockFirstname",
                LastName = "mockLastname",
                Email = "mock@email.com",
                PhoneNumber = "mockPhone",
                Role = new Role() { Id = 1, Name = "mockRolename" }
            };
            var mockCar = new Car()
            {
                Id = 1,
                User = mockUser,
                Registration = "AB12345",
                Vin = "mockVin",
                Status = "mockStatus",
                Kind = "mockKind",
                Usage = "mockUsage",
                Category = "mockCategory",
                ModelYear = 9999,
                Brand = "mockBrand",
                Model = "mockModel",
                Variant = "mockVariant",
                FuelType = "mockFuelType",
                ExtraEquipment = "mockExtra"
            };
            mockCarRepo.Setup(cr => cr.GetCarByIdAsync(mockCar.Id))
                .ReturnsAsync(mockCar);
            mockCarRepo.Setup(cr => cr.DeleteCar(mockCar));
            mockCarRepo.Setup(cr => cr.SaveChangesAsync())
                .ReturnsAsync(true);

            var controller = new CarsController(mockCarRepo.Object, mockUserRepo.Object, mockCarRegHelper.Object);

            // Act
            var response = (OkResult)await controller.DeleteCar(mockCar.Id);

            // Assert
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task DeleteCar_ReturnsBadRequest_WhenCarIdIsLessThanOne()
        {
            // Arrange
            var mockCar = new Car()
            {
                Id = 0
            };

            var controller = new CarsController(mockCarRepo.Object, mockUserRepo.Object, mockCarRegHelper.Object);

            // Act
            var response = (BadRequestObjectResult)await controller.DeleteCar(mockCar.Id);

            // Assert
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public async Task DeleteCar_ReturnsNotFound_WhenCarDoesntExist()
        {
            // Arrange
            int carId = 1;
            var mockCar = new Car();
            mockCarRepo.Setup(cr => cr.GetCarByIdAsync(carId))
                .ReturnsAsync(mockCar = null);

            var controller = new CarsController(mockCarRepo.Object, mockUserRepo.Object, mockCarRegHelper.Object);

            // Act
            var response = (NotFoundObjectResult)await controller.DeleteCar(carId);

            // Assert
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(404));
        }
    }
}
