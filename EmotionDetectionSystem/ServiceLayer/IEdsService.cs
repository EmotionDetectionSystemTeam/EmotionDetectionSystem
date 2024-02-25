using System;
using EmotionDetectionSystem.ServiceLayer;

namespace EmotionDetectionSystem.Service
{
    public interface IEdsService
    {
        Response Register(string email, string firstName, string lastName, string password, string confirmPassword,
                          int    userType);

        Response<ServiceUser> Login(string sessionId, string email, string password);
        Response Logout(string sessionId, string email);
        Response<string> CreateLesson(string sessionId, string email, string title, string description, string[] tags);
        Response EndLesson(string sessionId, string email);
        Response<ServiceLesson> JoinLesson(string sessionId, string email, string entryCode);
        Response<Dictionary<int, ServiceEnrollmentSummary>> ViewStudentsDuringLesson(string sessionId);
        Response<List<ServiceLesson>> ViewLessonDashboard(string sessionId);
        Response<ServiceUser> ViewStudent(string sessionId);
    }
}