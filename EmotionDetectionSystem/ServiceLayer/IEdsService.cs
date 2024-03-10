using EmotionDetectionSystem.ServiceLayer.objects;
using EmotionDetectionSystem.ServiceLayer.Responses;

namespace EmotionDetectionSystem.ServiceLayer
{
    public interface IEdsService
    {
        Response Register(string email, string firstName, string lastName, string password, string confirmPassword,
                          int    userType);

        Response<ServiceUser> Login(string  sessionId, string email, string password);
        Response              Logout(string sessionId, string email);

        Response<SActiveLesson> CreateLesson(string   sessionId, string email, string title, string description,
                                             string[] tags);

        Response                EndLesson(string  sessionId, string email);
        Response<SActiveLesson> JoinLesson(string sessionId, string email, string entryCode);

        Response<List<ServiceEnrollmentSummary>> ViewStudentsDuringLesson(
            string sessionId, string email, string lessonId);

        Response<List<ServiceLesson>> ViewLessonDashboard(string sessionId, string email);
        Response<ServiceUser> ViewStudent(string sessionId, string email, string studentEmail);
        Response PushEmotionData(string sessionId, string email, string lessonId, ServiceEmotionData emotionData);
        Response EnterAsGuest(string session);
        Response<List<ServiceRealTimeUser>> GetLastEmotionsData(string sessionId, string email, string lessonId);
        Response<SActiveLesson> GetLesson(string sessionId, string email, string lessonId);
    }
}