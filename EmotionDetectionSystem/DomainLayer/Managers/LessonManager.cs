using EmotionDetectionSystem.DomainLayer.objects;
using EmotionDetectionSystem.Service;
using EmotionDetectionSystem.ServiceLayer;

namespace EmotionDetectionSystem.DomainLayer.Managers;

public class LessonManager
{

    public Lesson CreateLesson(string sessionId, string title, string description, string[] tags)
    {
        throw new NotImplementedException();
    }

    public void EndLesson(string sessionId)
    {
        throw new NotImplementedException();
    }

    public User JoinLesson(string sessionId, string entryCode)
    {
        throw new NotImplementedException();
    }

    public Dictionary<string, EnrollmentSummary> ViewStudentsDuringLesson(string sessionId)
    {
        throw new NotImplementedException();
    }

    public List<Lesson> ViewTeacherDashboard(string sessionId)
    {
        throw new NotImplementedException();
    }

    public Response<ServiceUser> ViewStudent(string sessionId)
    {
        throw new NotImplementedException();
    }
}