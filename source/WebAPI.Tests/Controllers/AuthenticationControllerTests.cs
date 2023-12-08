using Common.Entities;
using Common.Helpers;
using DatabaseAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Controllers;
using WebAPI.Models.Authentication;

namespace WebAPI.Tests.Controllers
{
    public class AuthenticationControllerTests
    {

        private Mock<IUserRepository> mockUserRepo = new Mock<IUserRepository>();
        private Mock<PasswordHelper> mockPasswordHelper = new Mock<PasswordHelper>();
        private Mock<IConfiguration> mockConfiguration = new Mock<IConfiguration>();

        private User? testUser = new User()
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

        private AuthenticationRequestBody authenticationRequestBody = new AuthenticationRequestBody()
        {
            Username = "mockUsername",
            Password = "mockPasswordHash"
        };


        [SetUp]
        public void Setup()
        {
            mockConfiguration.Setup(c => c["Authentication:SecretForKey"])
                .Returns("873C606F-4653-4722-9A5A-D75A4D8E93B4");
            mockConfiguration.Setup(c => c["Authentication:Issuer"])
                .Returns("mockIssuer");
            mockConfiguration.Setup(c => c["Authentication:Audience"])
                .Returns("mockAudience");
        }

        [Test]
        public async Task Authenticate_ReturnsOk()
        {
            // Arrange
            mockUserRepo.Setup(repo => repo.GetUserByUsernameAsync(testUser.Username))
                .ReturnsAsync(testUser);
            mockPasswordHelper.Setup(pwh => pwh.VerifyPassword(testUser.PasswordHash, testUser.PasswordHash, testUser.Salt))
                .Returns(true);

            var controller = new AuthenticationController(mockUserRepo.Object, mockPasswordHelper.Object, mockConfiguration.Object);


            // Act
            var response = (OkObjectResult)await controller.Authenticate(authenticationRequestBody);

            // Assert
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(200));
            Assert.That(response.Value, Is.TypeOf<string>());
        }

        [Test]
        public async Task Authenticate_ReturnsUnauthorized_WhenUserDoesntExist()
        {
            // Arrange
            mockUserRepo.Setup(repo => repo.GetUserByUsernameAsync(testUser.Username))
                .ReturnsAsync(testUser = null);
            mockPasswordHelper.Setup(pwh => pwh.VerifyPassword("mockPasswordHash", "mockPasswordHash", new byte[] { }))
                .Returns(true);

            var controller = new AuthenticationController(mockUserRepo.Object, mockPasswordHelper.Object, mockConfiguration.Object);


            // Act
            var response = (UnauthorizedResult)await controller.Authenticate(authenticationRequestBody);

            // Assert
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(401));
        }

        [Test]
        public async Task Authenticate_ReturnsUnauthorized_WhenGivenPasswordDoesntMatchDatabasePassword()
        {
            // Arrange
            mockUserRepo.Setup(repo => repo.GetUserByUsernameAsync(testUser.Username))
                .ReturnsAsync(testUser);
            mockPasswordHelper.Setup(pwh => pwh.VerifyPassword("wrongPassword", "correctPassword", new byte[] { }))
                .Returns(false);
            

            var controller = new AuthenticationController(mockUserRepo.Object, mockPasswordHelper.Object, mockConfiguration.Object);


            // Act
            var response = (UnauthorizedResult)await controller.Authenticate(authenticationRequestBody);

            // Assert
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(401));
        }
    }
}
