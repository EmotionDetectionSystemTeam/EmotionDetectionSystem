using EmotionDetectionSystem.DomainLayer.Managers;
using EmotionDetectionSystem.Service;
using EmotionDetectionSystem.ServiceLayer;

namespace EmotionDetectionSystem.ServiceLayer;

public class EdsService : IEdsService
{
    private EdsManager _edsManager;
    public Response Register(string email, string firstName, string lastName, string password, string confirmPassword,
        bool isStudent)
    {
        throw new NotImplementedException();
    }

    public Response<ServiceUser> Login(string email, string password)
    {
        throw new NotImplementedException();
    }

    public Response Logout(string sessionId)
    {
        throw new NotImplementedException();
    }

    public Response<string> CreateLesson(string sessionId, string title, string description, string[] tags)
    {
        throw new NotImplementedException();
    }

    public Response EndLesson(string sessionId)
    {
        throw new NotImplementedException();
    }

    public Response<ServiceUser> JoinLesson(string sessionId, string entryCode)
    {
        throw new NotImplementedException();
    }

    public Response<Dictionary<int, ServiceEnrollmentSummary>> ViewStudentsDuringLesson(string sessionId)
    {
        throw new NotImplementedException();
    }

    public Response<List<ServiceLesson>> ViewLessonDashboard(string sessionId)
    {
        throw new NotImplementedException();
    }

    public Response<ServiceUser> ViewStudent(string sessionId)
    {
        throw new NotImplementedException();
    }
}