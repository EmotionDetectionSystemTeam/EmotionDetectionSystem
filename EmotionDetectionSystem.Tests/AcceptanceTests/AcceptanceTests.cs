using EmotionDetectionServer;
using EmotionDetectionSystem.ServiceLayer;
using EmotionDetectionSystem.ServiceLayer.objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace EmotionDetectionSystem.Tests.AcceptanceTests
{
    [TestClass]
    public class AcceptanceTests
    {
        private EdsService _edsService;

        private const string SessionId = "15";
        private const string Email = "test@example.com";
        private const string Password = "ABCab123sdABC#";
        private const string FirstName = "Test";
        private const string LastName = "User";
        private const int UserType = 1;
        private const string LessonTitle = "Sample Lesson";
        private const string LessonDescription = "This is a sample lesson.";
        private const string EntryCode = "entryCode";
        private const string TeacherEmail = "teacher@example.com";
        private const string TeacherPassword = "TeacherPass#2024";
        private const string LessonEntryCode = "MathLessonCode";
        private readonly string[] StudentEmails =
        {
            "student1@example.com",
            "student2@example.com",
            "student3@example.com",
            "student4@example.com",
            "student5@example.com",
            "student6@example.com",
            "student7@example.com",
            "student8@example.com",
            "student9@example.com",
            "student10@example.com"
        };
        private const string StudentPassword = "Student#Pass2024";
        private ServiceEmotionData _emotionData;
        private string _lessonEntryCode;

        [TestInitialize]
        public void Setup()
        {
            _edsService = new EdsService();
            //new ConfigurationFileHandler(_edsService).Parse(); //init a class with 10 students.
            Random rand = new Random();
            _emotionData = new ServiceEmotionData( rand.NextDouble(), rand.NextDouble(), rand.NextDouble(), rand.NextDouble(),
                rand.NextDouble(), rand.NextDouble(), rand.NextDouble());
        }

        [TestMethod]
        public void Register_ShouldReturnSuccess_WhenValidInput()
        {
            var response = _edsService.Register(Email, FirstName, LastName, Password, Password, UserType);
            Assert.IsFalse(response.ErrorOccured, response.ErrorMessage);
        }

        [TestMethod]
        public void Login_ShouldReturnServiceUser_WhenValidCredentials()
        {
            _edsService.Register(Email, FirstName, LastName, Password, Password, UserType);
            var response = _edsService.Login(SessionId, Email, Password);
            Assert.IsFalse(response.ErrorOccured, response.ErrorMessage);
            Assert.IsNotNull(response.Value);
        }

        [TestMethod]
        public void Logout_ShouldReturnSuccess_WhenValidSession()
        {
            _edsService.Register(Email, FirstName, LastName, Password, Password, UserType);
            _edsService.Login(SessionId, Email, Password);
            var response = _edsService.Logout(SessionId, Email);
            Assert.IsFalse(response.ErrorOccured, response.ErrorMessage);
        }

        [TestMethod]
        public void CreateLesson_ShouldReturnSActiveLesson_WhenValidInput()
        {
            _edsService.Register(Email, FirstName, LastName, Password, Password, UserType);
            _edsService.Login(SessionId, Email, Password);
            var response = _edsService.CreateLesson(SessionId, Email, LessonTitle, LessonDescription, new[] { "tag1", "tag2" });
            Assert.IsFalse(response.ErrorOccured, response.ErrorMessage);
            Assert.IsNotNull(response.Value);
        }

        [TestMethod]
        public void EndLesson_ShouldReturnSuccess_WhenValidSession()
        {
            _edsService.Register(Email, FirstName, LastName, Password, Password, UserType);
            _edsService.Login(SessionId, Email, Password);
            _edsService.CreateLesson(SessionId, Email, LessonTitle, LessonDescription, new[] { "tag1", "tag2" });
            var response = _edsService.EndLesson(SessionId, Email);
            Assert.IsFalse(response.ErrorOccured, response.ErrorMessage);
        }

        [TestMethod]
        public void ViewStudentsDuringLesson_ShouldReturnStudentList_WhenValidLessonId()
        {
            _edsService.Register(Email, FirstName, LastName, Password, Password, UserType);
            _edsService.Login(SessionId, Email, Password);
            var lessonResponse = _edsService.CreateLesson(SessionId, Email, LessonTitle, LessonDescription, new[] { "tag1", "tag2" });
            var lessonId = lessonResponse.Value.LessonId;
            var response = _edsService.ViewStudentsDuringLesson(SessionId, Email, lessonId);
            Assert.IsFalse(response.ErrorOccured, response.ErrorMessage);
            Assert.IsNotNull(response.Value);
        }

        [TestMethod]
        public void ViewLessonDashboard_ShouldReturnLessonList_WhenValidSession()
        {
            _edsService.Register(Email, FirstName, LastName, Password, Password, UserType);
            _edsService.Login(SessionId, Email, Password);
            var response = _edsService.ViewLessonDashboard(SessionId, Email);
            Assert.IsFalse(response.ErrorOccured, response.ErrorMessage);
            Assert.IsNotNull(response.Value);
        }

        [TestMethod]
        public void PushEmotionData_ShouldReturnSuccess_WhenValidData()
        {
            _edsService.Register(Email, FirstName, LastName, Password, Password, UserType);
            _edsService.Login(SessionId, Email, Password);
            var lessonResponse = _edsService.CreateLesson(SessionId, Email, LessonTitle, LessonDescription, new[] { "tag1", "tag2" });
            var lessonId = lessonResponse.Value.LessonId;
            _edsService.JoinLesson(SessionId, Email, lessonId);
            var response = _edsService.PushEmotionData(SessionId, Email, lessonId, _emotionData);
            Assert.IsFalse(response.ErrorOccured, response.ErrorMessage);
        }

        [TestMethod]
        public void EnterAsGuest_ShouldReturnSuccess_WhenCalled()
        {
            var response = _edsService.EnterAsGuest(SessionId);
            Assert.IsFalse(response.ErrorOccured, response.ErrorMessage);
        }

        [TestMethod]
        public void GetLastEmotionsData_ShouldReturnEmotionDataList_WhenValidLessonId()
        {
            _edsService.Register(Email, FirstName, LastName, Password, Password, UserType);
            _edsService.Login(SessionId, Email, Password);
            var lessonResponse = _edsService.CreateLesson(SessionId, Email, LessonTitle, LessonDescription, new[] { "tag1", "tag2" });
            var lessonId = lessonResponse.Value.LessonId;
            var response = _edsService.GetLastEmotionsData(SessionId, Email, lessonId);
            Assert.IsFalse(response.ErrorOccured, response.ErrorMessage);
            Assert.IsNotNull(response.Value);
        }

        [TestMethod]
        public void GetLesson_ShouldReturnSActiveLesson_WhenValidLessonId()
        {
            _edsService.Register(Email, FirstName, LastName, Password, Password, UserType);
            _edsService.Login(SessionId, Email, Password);
            var lessonResponse = _edsService.CreateLesson(SessionId, Email, LessonTitle, LessonDescription, new[] { "tag1", "tag2" });
            var lessonId = lessonResponse.Value.LessonId;
            var response = _edsService.GetLesson(SessionId, Email, lessonId);
            Assert.IsFalse(response.ErrorOccured, response.ErrorMessage);
            Assert.IsNotNull(response.Value);
        }

        [TestMethod]
        public void GetEnrolledLessons_ShouldReturnLessonOverviewList_WhenValidTeacherEmail()
        {
            _edsService.Register(Email, FirstName, LastName, Password, Password, UserType);
            _edsService.Login(SessionId, Email, Password);
            var response = _edsService.GetEnrolledLessons(SessionId, Email);
            Assert.IsFalse(response.ErrorOccured, response.ErrorMessage);
            Assert.IsNotNull(response.Value);
        }

        [TestMethod]
        public void GetStudentDataByLesson_ShouldReturnStudentInClassOverviewList_WhenValidLessonId()
        {
            _edsService.Register(Email, FirstName, LastName, Password, Password, UserType);
            _edsService.Login(SessionId, Email, Password);
            var lessonResponse = _edsService.CreateLesson(SessionId, Email, LessonTitle, LessonDescription, new[] { "tag1", "tag2" });
            var lessonId = lessonResponse.Value.LessonId;
            var response = _edsService.GetStudentDataByLesson(SessionId, Email, lessonId);
            Assert.IsFalse(response.ErrorOccured, response.ErrorMessage);
            Assert.IsNotNull(response.Value);
        }

        [TestMethod]
        public void GetStudentData_ShouldReturnStudentOverviewList_WhenValidTeacherEmail()
        {
            _edsService.Register(Email, FirstName, LastName, Password, Password, UserType);
            _edsService.Login(SessionId, Email, Password);
            var response = _edsService.GetAllStudentsData(SessionId, Email);
            Assert.IsFalse(response.ErrorOccured, response.ErrorMessage);
            Assert.IsNotNull(response.Value);
        }

        [TestMethod]
        public void StartingAClassWith10Students()
        {
            // Register Teacher
            var registerTeacherResponse = _edsService.Register(TeacherEmail, "John", "Doe", TeacherPassword, TeacherPassword, 1);
            Assert.IsFalse(registerTeacherResponse.ErrorOccured, registerTeacherResponse.ErrorMessage);

            // Teacher Login
            var loginTeacherResponse = _edsService.Login("1", TeacherEmail, TeacherPassword);
            Assert.IsFalse(loginTeacherResponse.ErrorOccured, loginTeacherResponse.ErrorMessage);

            // Create Lesson
            var createLessonResponse = _edsService.CreateLesson("1", TeacherEmail, LessonTitle, LessonDescription, new[] { "Math", "Algebra" });
            Assert.IsFalse(createLessonResponse.ErrorOccured, createLessonResponse.ErrorMessage);

            // Assume that the entry code for the lesson is returned in the lesson object
            var activeLesson = createLessonResponse.Value;
            _lessonEntryCode = activeLesson.EntryCode; // You might need to adjust this according to your actual data structure

            // Register Students
            for (int i = 0; i < StudentEmails.Length; i++)
            {
                var studentEmail = StudentEmails[i];
                var registerStudentResponse = _edsService.Register(studentEmail, $"Student{i + 1}", $"User{i + 1}", StudentPassword.Replace("#Pass2024", $"{i + 1}#Pass2024"), StudentPassword.Replace("#Pass2024", $"{i + 1}#Pass2024"), 0);
                Assert.IsFalse(registerStudentResponse.ErrorOccured, registerStudentResponse.ErrorMessage);
            }

            // Students Login
            for (int i = 0; i < StudentEmails.Length; i++)
            {
                var studentEmail = StudentEmails[i];
                var loginStudentResponse = _edsService.Login((i + 2).ToString(), studentEmail, StudentPassword.Replace("#Pass2024", $"{i + 1}#Pass2024"));
                Assert.IsFalse(loginStudentResponse.ErrorOccured, loginStudentResponse.ErrorMessage);
            }

            // Students Join Lesson
            for (int i = 0; i < StudentEmails.Length; i++)
            {
                var studentEmail = StudentEmails[i];
                var joinLessonResponse = _edsService.JoinLesson((i + 2).ToString(), studentEmail, _lessonEntryCode);
                Assert.IsFalse(joinLessonResponse.ErrorOccured, joinLessonResponse.ErrorMessage);
            }
        }

        [TestMethod]
        public void StudentShouldBeLoggedInForEnrollment()
        {
            // Register Student
            var registerStudentResponse = _edsService.Register(StudentEmails[0], "Student", "User", StudentPassword, StudentPassword, 0);
            Assert.IsFalse(registerStudentResponse.ErrorOccured, registerStudentResponse.ErrorMessage);

            // Student Login
            var loginStudentResponse = _edsService.Login("1", StudentEmails[0], StudentPassword);
            Assert.IsFalse(loginStudentResponse.ErrorOccured, loginStudentResponse.ErrorMessage);

            // Ensure that the student is logged in
            Assert.IsNotNull(loginStudentResponse.Value, "Student login failed.");
        }
        [TestMethod]
        public void TeacherShouldAccessEmotionalData()
        {
            // Register Teacher
            _edsService.Register(TeacherEmail, "John", "Doe", TeacherPassword, TeacherPassword, 1);

            // Teacher Login
            _edsService.Login("1", TeacherEmail, TeacherPassword);

            // Create Lesson
            var createLessonResponse = _edsService.CreateLesson("1", TeacherEmail, LessonTitle, LessonDescription, new[] { "Math", "Algebra" });
            // Assume that the entry code for the lesson is returned in the lesson object
            var activeLesson = createLessonResponse.Value;
            _lessonEntryCode = activeLesson.EntryCode; // You might need to adjust this according to your actual data structure

            // Access Emotional Data
            var emotionalDataResponse = _edsService.GetLastEmotionsData("1", TeacherEmail, activeLesson.LessonId);
            Assert.IsFalse(emotionalDataResponse.ErrorOccured, emotionalDataResponse.ErrorMessage);
            Assert.IsNotNull(emotionalDataResponse.Value, "Teacher should be able to access emotional data.");
        }

        [TestMethod]
        public void StudentShouldNotAccessEmotionalData()
        {
            // Register Teacher
            _edsService.Register(TeacherEmail, "John", "Doe", TeacherPassword, TeacherPassword, 1);

            // Teacher Login
            _edsService.Login("1", TeacherEmail, TeacherPassword);

            // Create Lesson
            var createLessonResponse = _edsService.CreateLesson("1", TeacherEmail, LessonTitle, LessonDescription, new[] { "Math", "Algebra" });

            // Register Student
            _edsService.Register(StudentEmails[0], "Student", "User", StudentPassword, StudentPassword, 0);

            // Student Login
            _edsService.Login("2", StudentEmails[0], StudentPassword);
            var activeLesson = createLessonResponse.Value;
            _lessonEntryCode = activeLesson.EntryCode; // You might need to adjust this according to your actual data structure
            // Student Joins Lesson
            _edsService.JoinLesson("2", StudentEmails[0], _lessonEntryCode);

            // Access Emotional Data
            var emotionalDataResponse = _edsService.GetLastEmotionsData("2", StudentEmails[0], LessonTitle);
            Assert.IsTrue(emotionalDataResponse.ErrorOccured, "Student should not be able to access emotional data.");
        }

    }
}
