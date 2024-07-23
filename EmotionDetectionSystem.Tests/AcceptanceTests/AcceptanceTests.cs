using EmotionDetectionServer;
using EmotionDetectionSystem.ServiceLayer;
using EmotionDetectionSystem.ServiceLayer.objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

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
            _emotionData = new ServiceEmotionData(Emotions.SURPRISED, DateTime.Now);
        }
        [TestCleanup]
        public void Cleanup()
        {
            _edsService.Dispose();
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
        
        [TestMethod]
        public void CreateUsers_CreateClass_LogInAndSendEmotionData()
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

            // Register 5 Students
            for (int i = 0; i < 5; i++)
            {
                var studentEmail = $"student{i + 1}@example.com";
                var registerStudentResponse = _edsService.Register(studentEmail, $"Student{i + 1}", $"User{i + 1}", StudentPassword.Replace("#Pass2024", $"{i + 1}#Pass2024"), StudentPassword.Replace("#Pass2024", $"{i + 1}#Pass2024"), 0);
                Assert.IsFalse(registerStudentResponse.ErrorOccured, registerStudentResponse.ErrorMessage);
            }

            // Students Login
            for (int i = 0; i < 5; i++)
            {
                var studentEmail = $"student{i + 1}@example.com";
                var loginStudentResponse = _edsService.Login((i + 2).ToString(), studentEmail, StudentPassword.Replace("#Pass2024", $"{i + 1}#Pass2024"));
                Assert.IsFalse(loginStudentResponse.ErrorOccured, loginStudentResponse.ErrorMessage);
            }

            // Students Join Lesson
            var lessonEntryCode = createLessonResponse.Value.EntryCode;
            for (int i = 0; i < 5; i++)
            {
                var studentEmail = $"student{i + 1}@example.com";
                var joinLessonResponse = _edsService.JoinLesson((i + 2).ToString(), studentEmail, lessonEntryCode);
                Assert.IsFalse(joinLessonResponse.ErrorOccured, joinLessonResponse.ErrorMessage);
            }

            // Push Emotion Data for each student
            var lessonId = createLessonResponse.Value.LessonId;
            foreach (var studentEmail in StudentEmails.Take(5))
            {
                var pushEmotionResponse = _edsService.PushEmotionData("2", studentEmail, lessonId, _emotionData);
                Assert.IsFalse(pushEmotionResponse.ErrorOccured, pushEmotionResponse.ErrorMessage);
            }
        } 
        
        [TestMethod]
        public async Task CreateUsers_Concurrently_CreateClass_LogInAndSendEmotionData()
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

            // Concurrently register 5 students
            var registerStudentTasks = Enumerable.Range(0, 5).Select(i =>
            {
                var studentEmail = $"student{i + 1}@example.com";
                return Task.Run(() =>
                {
                    var registerStudentResponse = _edsService.Register(studentEmail, $"Student{i + 1}", $"User{i + 1}", StudentPassword.Replace("#Pass2024", $"{i + 1}#Pass2024"), StudentPassword.Replace("#Pass2024", $"{i + 1}#Pass2024"), 0);
                    Assert.IsFalse(registerStudentResponse.ErrorOccured, registerStudentResponse.ErrorMessage);
                });
            }).ToArray();

            await Task.WhenAll(registerStudentTasks);

            // Concurrently log in 5 students
            var loginStudentTasks = Enumerable.Range(0, 5).Select(i =>
            {
                var studentEmail = $"student{i + 1}@example.com";
                return Task.Run(() =>
                {
                    var loginStudentResponse = _edsService.Login((i + 2).ToString(), studentEmail, StudentPassword.Replace("#Pass2024", $"{i + 1}#Pass2024"));
                    Assert.IsFalse(loginStudentResponse.ErrorOccured, loginStudentResponse.ErrorMessage);
                });
            }).ToArray();

            await Task.WhenAll(loginStudentTasks);

            // Concurrently join 5 students to the lesson
            var lessonEntryCode = createLessonResponse.Value.EntryCode;
            var joinLessonTasks = Enumerable.Range(0, 5).Select(i =>
            {
                var studentEmail = $"student{i + 1}@example.com";
                return Task.Run(() =>
                {
                    var joinLessonResponse = _edsService.JoinLesson((i + 2).ToString(), studentEmail, lessonEntryCode);
                    Assert.IsFalse(joinLessonResponse.ErrorOccured, joinLessonResponse.ErrorMessage);
                });
            }).ToArray();

            await Task.WhenAll(joinLessonTasks);

            // Concurrently push emotion data for each student
            var lessonId = createLessonResponse.Value.LessonId;
            var pushEmotionTasks = StudentEmails.Take(5).Select(studentEmail =>
            {
                return Task.Run(() =>
                {
                    var pushEmotionResponse = _edsService.PushEmotionData("2", studentEmail, lessonId, _emotionData);
                    Assert.IsFalse(pushEmotionResponse.ErrorOccured, pushEmotionResponse.ErrorMessage);
                });
            }).ToArray();

            await Task.WhenAll(pushEmotionTasks);
        }
        [TestMethod]
        public void PushEmotionData_ShouldSucceed_WhenValidData()
        {
            _edsService.Register(TeacherEmail, "John", "Doe", TeacherPassword, TeacherPassword, 1);
            _edsService.Login(SessionId, TeacherEmail, TeacherPassword);
            var createLessonResponse = _edsService.CreateLesson(SessionId, TeacherEmail, LessonTitle, LessonDescription, new[] { "tag1", "tag2" });
            Assert.IsFalse(createLessonResponse.ErrorOccured, createLessonResponse.ErrorMessage);
            var lessonId = createLessonResponse.Value.LessonId;
            var emotionData = new ServiceEmotionData(Emotions.FEARFUL, DateTime.Now);
            var response = _edsService.PushEmotionData(SessionId, TeacherEmail, lessonId, emotionData);
            Assert.IsFalse(response.ErrorOccured, response.ErrorMessage);
        }

        [TestMethod]
        public void GetLesson_ShouldReturnActiveLesson_WhenValidLessonId()
        {
            _edsService.Register(TeacherEmail, "John", "Doe", TeacherPassword, TeacherPassword, 1);
            _edsService.Login(SessionId, TeacherEmail, TeacherPassword);
            var createLessonResponse = _edsService.CreateLesson(SessionId, TeacherEmail, LessonTitle, LessonDescription, new[] { "tag1", "tag2" });
            Assert.IsFalse(createLessonResponse.ErrorOccured, createLessonResponse.ErrorMessage);
            var lessonId = createLessonResponse.Value.LessonId;
            var response = _edsService.GetLesson(SessionId, TeacherEmail, lessonId);
            Assert.IsFalse(response.ErrorOccured, response.ErrorMessage);
            Assert.IsNotNull(response.Value);
        }


    }
}
