using System;
using System.Linq;
using EmotionDetectionSystem.DomainLayer.Managers;
using EmotionDetectionSystem.DomainLayer.objects;
using EmotionDetectionSystem.ServiceLayer;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EmotionDetectionSystem.Tests.AcceptanceTests
{
    [TestClass]
    public class AcceptanceTest
    {
        private EdsService _edsService;

        private const string TestEmail = "test@example.com";
        private const string TestFirstName = "John";
        private const string TestLastName = "Doe";
        private const string TestPassword = "Password123!";
        private const string TestConfirmPassword = "Password123!";
        private const int TestUserType = 1;
        private const string TestSessionId = "session123";
        private const string TestTitle = "New Lesson";
        private const string TestDescription = "Lesson Description";
        private const string TestErrorMessage = "Operation failed";
        private static readonly string[] TestTags = { "Tag1", "Tag2" };

        [TestInitialize]
        public void TestInitialize()
        {
            _edsService = new EdsService();
        }

        [TestMethod]
        public void RegisterUser_WithValidData_ShouldSucceed()
        {
            // Act
            var response = _edsService.Register(TestEmail, TestFirstName, TestLastName, TestPassword, TestConfirmPassword, TestUserType);

            // Assert
            Assert.IsFalse(response.ErrorOccured, "User registration should succeed with valid data.");
        }

        [TestMethod]
        public void RegisterUser_WithInvalidData_ShouldFail()
        {
            // Act
            var response = _edsService.Register(TestEmail, TestFirstName, TestLastName, TestPassword, "DifferentPassword", TestUserType);

            // Assert
            Assert.IsTrue(response.ErrorOccured, "User registration should fail with invalid data.");
        }

        [TestMethod]
        public void LoginUser_WithValidCredentials_ShouldSucceed()
        {
            // First, register the user to ensure it exists
            var registerResponse = _edsService.Register(TestEmail, TestFirstName, TestLastName, TestPassword, TestConfirmPassword, TestUserType);
            Assert.IsFalse(registerResponse.ErrorOccured, "User registration should succeed for login test.");

            // Act
            var response = _edsService.Login(TestSessionId, TestEmail, TestPassword);

            // Assert
            Assert.IsFalse(response.ErrorOccured, "User login should succeed with valid credentials.");
            Assert.AreEqual(TestEmail, response.Value.Email, "Logged in user's email should match the provided email.");
        }

        [TestMethod]
        public void LoginUser_WithInvalidCredentials_ShouldFail()
        {
            // First, register the user to ensure it exists
            var registerResponse = _edsService.Register(TestEmail, TestFirstName, TestLastName, TestPassword, TestConfirmPassword, TestUserType);
            Assert.IsFalse(registerResponse.ErrorOccured, "User registration should succeed for login test.");

            // Act
            var response = _edsService.Login(TestSessionId, TestEmail, "WrongPassword");

            // Assert
            Assert.IsTrue(response.ErrorOccured, "User login should fail with invalid credentials.");
        }

        [TestMethod]
        public void LogoutUser_WithValidSession_ShouldSucceed()
        {
            // First, register and log in the user to ensure it exists and is logged in
            var registerResponse = _edsService.Register(TestEmail, TestFirstName, TestLastName, TestPassword, TestConfirmPassword, TestUserType);
            Assert.IsFalse(registerResponse.ErrorOccured, "User registration should succeed for logout test.");

            var loginResponse = _edsService.Login(TestSessionId, TestEmail, TestPassword);
            Assert.IsFalse(loginResponse.ErrorOccured, "User login should succeed for logout test.");

            // Act
            var response = _edsService.Logout(TestSessionId, TestEmail);

            // Assert
            Assert.IsFalse(response.ErrorOccured, "User logout should succeed with a valid session.");
        }

        [TestMethod]
        public void LogoutUser_WithInvalidSession_ShouldFail()
        {
            // Act
            var response = _edsService.Logout("InvalidSessionId", TestEmail);

            // Assert
            Assert.IsTrue(response.ErrorOccured, "User logout should fail with an invalid session.");
        }

        [TestMethod]
        public void CreateLesson_WithValidData_ShouldSucceed()
        {
            // First, register and log in the user to ensure it exists and is logged in
            var registerResponse = _edsService.Register(TestEmail, TestFirstName, TestLastName, TestPassword, TestConfirmPassword, TestUserType);
            Assert.IsFalse(registerResponse.ErrorOccured, "User registration should succeed for lesson creation test.");

            var loginResponse = _edsService.Login(TestSessionId, TestEmail, TestPassword);
            Assert.IsFalse(loginResponse.ErrorOccured, "User login should succeed for lesson creation test.");

            // Act
            var response = _edsService.CreateLesson(TestSessionId, TestEmail, TestTitle, TestDescription, TestTags);

            // Assert
            Assert.IsFalse(response.ErrorOccured, "Lesson creation should succeed with valid data.");
            Assert.AreEqual(TestTitle, response.Value.ToString(), "Created lesson's title should match the provided title.");
        }

        [TestMethod]
        public void CreateLesson_WithInvalidData_ShouldFail()
        {
            // First, register and log in the user to ensure it exists and is logged in
            var registerResponse = _edsService.Register(TestEmail, TestFirstName, TestLastName, TestPassword, TestConfirmPassword, TestUserType);
            Assert.IsFalse(registerResponse.ErrorOccured, "User registration should succeed for lesson creation test.");

            var loginResponse = _edsService.Login(TestSessionId, TestEmail, TestPassword);
            Assert.IsFalse(loginResponse.ErrorOccured, "User login should succeed for lesson creation test.");

            // Act
            var response = _edsService.CreateLesson(TestSessionId, TestEmail, "", TestDescription, TestTags);

            // Assert
            Assert.IsTrue(response.ErrorOccured, "Lesson creation should fail with invalid data.");
        }

        [TestMethod]
        public void EndLesson_WithValidSession_ShouldSucceed()
        {
            // First, register and log in the user, then create a lesson to ensure the lesson exists and is active
            var registerResponse = _edsService.Register(TestEmail, TestFirstName, TestLastName, TestPassword, TestConfirmPassword, TestUserType);
            Assert.IsFalse(registerResponse.ErrorOccured, "User registration should succeed for lesson end test.");

            var loginResponse = _edsService.Login(TestSessionId, TestEmail, TestPassword);
            Assert.IsFalse(loginResponse.ErrorOccured, "User login should succeed for lesson end test.");

            var createLessonResponse = _edsService.CreateLesson(TestSessionId, TestEmail, TestTitle, TestDescription, TestTags);
            Assert.IsFalse(createLessonResponse.ErrorOccured, "Lesson creation should succeed for lesson end test.");

            // Act
            var response = _edsService.EndLesson(TestSessionId, TestEmail);

            // Assert
            Assert.IsFalse(response.ErrorOccured, "Ending the lesson should succeed with a valid session.");
        }

        [TestMethod]
        public void EndLesson_WithInvalidSession_ShouldFail()
        {
            // Act
            var response = _edsService.EndLesson("InvalidSessionId", TestEmail);

            // Assert
            Assert.IsTrue(response.ErrorOccured, "Ending the lesson should fail with an invalid session.");
        }
    }
}
