using FluentValidation.TestHelper;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Models.Entity;
using InnoGotchi_frontend.Controllers;
using InnoGotchi_frontend.Models.Validators;
using InnoGotchi_frontend.Services.Abstract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MoqTestsForApp.Client_side_tests
{
    public class UserValidationTests
    {
        private readonly UserValidator _validator;

        public UserValidationTests()
        {
            _validator = new UserValidator();
        }

        [Fact]
        public void UserValidatorUserNameIsNull()
        {
            // Arrange
            var user = new UserDto { UserName = null };

            // Act
            var result = _validator.TestValidate(user);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserName);
        }

        [Fact]
        public void UserValidatorFirstNameIsNull()
        {
            // Arrange
            var user = new UserDto { FirstName = null };

            // Act
            var result = _validator.TestValidate(user);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        public void UserValidatorLastNameIsNull()
        {
            // Arrange
            var user = new UserDto { LastName = null };

            // Act
            var result = _validator.TestValidate(user);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.LastName);
        }

        [Fact]
        public void UserValidatorEmailIsEmpty()
        {
            // Arrange
            var user = new UserDto { Email = string.Empty };

            // Act
            var result = _validator.TestValidate(user);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void UserValidatorPasswordIsEmpty()
        {
            // Arrange
            var user = new UserDto { Password = string.Empty };

            // Act
            var result = _validator.TestValidate(user);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void UserValidatorValidUser()
        {
            // Arrange
            var user = new UserDto
            {
                UserName = "Sir_Alexio",
                FirstName = "Alexio",
                LastName = "Sir",
                Email = "alexio@gmail.com",
                Password = "12345"
            };

            // Act
            var result = _validator.TestValidate(user);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
