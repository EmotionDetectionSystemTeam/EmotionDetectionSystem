using System;
using System.Linq;
using EmotionDetectionSystem.DomainLayer.Managers;
using EmotionDetectionSystem.DomainLayer.objects;
using EmotionDetectionSystem.ServiceLayer;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EmotionDetectionSystem.Tests.DomainLayer.Managers;

[TestClass]
[TestSubject(typeof(EdsManager))]
public class EdsManagerTest
{
    private EdsManager _edsManager;
    private string     _sessionId;

    [TestInitialize]
    public void SetUp()
    {
        _edsManager = new EdsManager();
        _sessionId  = "1";
    }

    [TestMethod]
    public void Register_GoodDetais_Success()
    {
        const string email     = "test@example.com";
        const string firstName = "John";
        const string lastName  = "Doe";
        const string password  = "1q2w3eAS!";
        const int    userType  = 1;
        _edsManager.Register(email, firstName, lastName, password, userType);
    }

    [TestMethod]
    public void Register_BadEmail_Fail()
    {
        const string email     = "testExample.com";
        const string firstName = "John";
        const string lastName  = "Doe";
        const string password  = "1q2w3eAS!";
        const int    userType  = 1;
        var exception =
            Assert.ThrowsException<Exception>(
                () => _edsManager.Register(email, firstName, lastName, password, userType));
        Assert.AreEqual("Email is not valid", exception.Message);
    }

    [TestMethod]
    public void Register_BadPassword_Fail()
    {
        const string email     = "test@example.com";
        const string firstName = "John";
        const string lastName  = "Doe";
        const string password  = "123456789";
        const int    userType  = 1;
        var exception =
            Assert.ThrowsException<Exception>(
                () => _edsManager.Register(email, firstName, lastName, password, userType));
        Assert.AreEqual("Password is not valid", exception.Message);
    }

    [TestMethod]
    public void Login_Success()
    {
        const string email     = "test@example.com";
        const string firstName = "John";
        const string lastName  = "Doe";
        const string password  = "1q2w3eAS!";
        const int    userType  = 1;
        _edsManager.Register(email, firstName, lastName, password, userType);
        _edsManager.Login(_sessionId, email, password);
        Assert.IsTrue(_edsManager.IsLoggedIn(_sessionId, email));
    }

    [TestMethod]
    public void Login_BadPassword_Fail()
    {
        const string email       = "test@example.com";
        const string firstName   = "John";
        const string lastName    = "Doe";
        const string password    = "1q2w3eAS!";
        const string badPassword = "123456789";
        const int    userType    = 1;
        _edsManager.Register(email, firstName, lastName, password, userType);
        var e = Assert.ThrowsException<Exception>(() => _edsManager.Login(_sessionId, email, badPassword));
        Assert.AreEqual("Password or username are incorrect", e.Message);
    }

    [TestMethod]
    public void Logout_Success()
    {
        const string email     = "test@example.com";
        const string firstName = "John";
        const string lastName  = "Doe";
        const string password  = "1q2w3eAS!";
        const int    userType  = 1;
        _edsManager.Register(email, firstName, lastName, password, userType);
        _edsManager.Login(_sessionId, email, password);
        _edsManager.Logout(_sessionId, email);
        Assert.IsFalse(_edsManager.IsLoggedIn(_sessionId, email));
    }

    [TestMethod]
    public void Logout_Fail()
    {
        const string email     = "test@example.com";
        const string firstName = "John";
        const string lastName  = "Doe";
        const string password  = "1q2w3eAS!";
        const int    userType  = 1;
        _edsManager.Register(email, firstName, lastName, password, userType);
        var e = Assert.ThrowsException<Exception>(() => _edsManager.Logout(_sessionId, email));
        Assert.AreEqual($"Session: {_sessionId} is not valid", e.Message);
    }

    [TestMethod]
    public void CreateLesson_Teacher_Success()
    {
        const string email     = "test@example.com";
        const string firstName = "John";
        const string lastName  = "Doe";
        const string password  = "1q2w3eAS!";
        const int    userType  = 1;
        _edsManager.Register(email, firstName, lastName, password, userType);
        _edsManager.Login(_sessionId, email, password);
        var lesson = _edsManager.CreateLesson(_sessionId, email, "test", "test", new[] { "test" });
        Assert.IsNotNull(lesson);
        Assert.AreEqual("test", lesson.LessonName);
        Assert.AreEqual("test", lesson.Description);
        Assert.AreEqual("test", lesson.Tags[0]);
    }

    [TestMethod]
    public void CreateLesson_Student_Fail()
    {
        const string email     = "test@example.com";
        const string firstName = "John";
        const string lastName  = "Doe";
        const string password  = "1q2w3eAS!";
        const int    userType  = 0;
        _edsManager.Register(email, firstName, lastName, password, userType);
        _edsManager.Login(_sessionId, email, password);
        var e = Assert.ThrowsException<Exception>(
            () => _edsManager.CreateLesson(_sessionId, email, "test", "test", new[] { "test" }));
        Assert.AreEqual("User is not a teacher", e.Message);
    }

    [TestMethod]
    public void EndLesson_Success()
    {
        const string email     = "test@example.com";
        const string firstName = "John";
        const string lastName  = "Doe";
        const string password  = "1q2w3eAS!";
        const int    userType  = 1;
        _edsManager.Register(email, firstName, lastName, password, userType);
        _edsManager.Login(_sessionId, email, password);
        var lesson = _edsManager.CreateLesson(_sessionId, email, "test", "test", new[] { "test" });
        Assert.IsTrue(lesson.IsActive);
        _edsManager.EndLesson(_sessionId, email);
        Assert.IsFalse(lesson.IsActive);
    }

    [TestMethod]
    public void EndLesson_NotCreatorTeacher_Fail()
    {
        const string email      = "test@example.com";
        const string email2     = "test2@example.com";
        const string firstName  = "John";
        const string lastName   = "Doe";
        const string password   = "1q2w3eAS!";
        const int    userType   = 1;
        const string sessionId2 = "2";
        _edsManager.Register(email,  firstName, lastName, password, userType);
        _edsManager.Register(email2, firstName, lastName, password, userType);
        _edsManager.Login(_sessionId, email,  password);
        _edsManager.Login(sessionId2, email2, password);
        var lesson = _edsManager.CreateLesson(_sessionId, email, "test", "test", new[] { "test" });
        Assert.IsTrue(lesson.IsActive);
        Assert.ThrowsException<Exception>(() => _edsManager.EndLesson(sessionId2, email));
    }

    [TestMethod]
    public void JoinLesson_Viewer_Success()
    {
        const string email      = "test@example.com";
        const string email2     = "test2@example.com";
        const string firstName  = "John";
        const string lastName   = "Doe";
        const string password   = "1q2w3eAS!";
        const int    userType   = 1;
        const string sessionId2 = "2";
        _edsManager.Register(email,  firstName, lastName, password, userType);
        _edsManager.Register(email2, firstName, lastName, password, userType);
        _edsManager.Login(_sessionId, email,  password);
        _edsManager.Login(sessionId2, email2, password);
        var user2  = (Teacher)_edsManager.GetUser(email2);
        var lesson = _edsManager.CreateLesson(_sessionId, email, "test", "test", new[] { "test" });
        _edsManager.JoinLesson(sessionId2, email2, lesson.EntryCode);
        Assert.IsTrue(lesson.Viewers.Contains(user2));
    }

    [TestMethod]
    public void JoinLesson_Student_Success()
    {
        const string email      = "test@example.com";
        const string email2     = "test2@example.com";
        const string firstName  = "John";
        const string lastName   = "Doe";
        const string password   = "1q2w3eAS!";
        const int    userType   = 1;
        const string sessionId2 = "2";
        _edsManager.Register(email,  firstName, lastName, password, userType);
        _edsManager.Register(email2, firstName, lastName, password, 0);
        _edsManager.Login(_sessionId, email,  password);
        _edsManager.Login(sessionId2, email2, password);
        var user2  = _edsManager.GetUser(email2);
        var lesson = _edsManager.CreateLesson(_sessionId, email, "test", "test", new[] { "test" });
        _edsManager.JoinLesson(sessionId2, email2, lesson.EntryCode);
        Assert.IsTrue(lesson.ContainStudent((Student)user2));
    }

    [TestMethod]
    public void ViewStudentsDuringLesson_Success()
    {
        const string email      = "test@example.com";
        const string email2     = "test2@example.com";
        const string firstName  = "John";
        const string lastName   = "Doe";
        const string password   = "1q2w3eAS!";
        const int    userType   = 1;
        const string sessionId2 = "2";
        _edsManager.Register(email,  firstName, lastName, password, userType);
        _edsManager.Register(email2, firstName, lastName, password, 0);
        _edsManager.Login(_sessionId, email,  password);
        _edsManager.Login(sessionId2, email2, password);
        var lesson = _edsManager.CreateLesson(_sessionId, email, "test", "test", new[] { "test" });
        _edsManager.JoinLesson(sessionId2, email2, lesson.EntryCode);
        var students = _edsManager.ViewStudentsDuringLesson(_sessionId, email, lesson.LessonId);
        Assert.IsNotNull(students);
        Assert.AreEqual(1,      students.Count());
        Assert.AreEqual(email2, students.First().Student.Email);
    }

    [TestMethod]
    public void ViewLessonDashboard_Success()
    {
        const string email     = "test@example.com";
        const string firstName = "John";
        const string lastName  = "Doe";
        const string password  = "1q2w3eAS!";
        const int    userType  = 1;
        _edsManager.Register(email, firstName, lastName, password, userType);
        _edsManager.Login(_sessionId, email, password);
        var lesson = _edsManager.CreateLesson(_sessionId, email, "test", "test", new[] { "test" });
        _edsManager.EndLesson(_sessionId, email);
        var lesson2 = _edsManager.CreateLesson(_sessionId, email, "test2", "test2", new[] { "test2" });
        var lessons = _edsManager.ViewTeacherDashboard(_sessionId, email);
        Assert.IsNotNull(lessons);
        Assert.AreEqual(2,                lessons.Count());
        Assert.AreEqual(lesson.LessonId,  lessons.First().LessonId);
        Assert.AreEqual(lesson2.LessonId, lessons.Last().LessonId);
    }

    [TestMethod]
    public void PushEmotionData()
    {
        // Register teacher
        string teacherEmail     = "teacher@example.com";
        string teacherFirstName = "John";
        string teacherLastName  = "Doe";
        string teacherPassword  = "1q2w3eAS!";
        int    teacherUserType  = 1;
        _edsManager.Register(teacherEmail, teacherFirstName, teacherLastName, teacherPassword, teacherUserType);
        _edsManager.Login("0", teacherEmail, teacherPassword);

        var lesson = _edsManager.CreateLesson("0", teacherEmail, "test", "test", new[] { "test" });

        int studentUserType = 0;
        for (int i = 1; i <= 50; i++)
        {
            string studentEmail = $"student{i}@example.com";
            _edsManager.Register(studentEmail, teacherFirstName, teacherLastName, teacherPassword, studentUserType);
            _edsManager.Login(i.ToString(), studentEmail, teacherPassword);
            _edsManager.JoinLesson(i.ToString(), studentEmail, lesson.EntryCode);
        }

        for (int i = 1; i <= 5000000; i++)
        {
            string studentEmail = $"student{(i % 50) + 1}@example.com"; // Corrected index
            _edsManager.PushEmotionData((i % 50 + 1).ToString(), studentEmail, lesson.LessonId, new EmotionData(
                                            DateTime.Now, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0));
        }
        while (_edsManager.IsProcessingTasks)
        {
        }
        
        Assert.AreEqual(50, lesson.GetEnrollmentSummaries().Count);
        int expectedEmotionDataCount = 50 * 100000;
        Assert.AreEqual(expectedEmotionDataCount, lesson.GetEmotionDataEntries().Count());
    }

}