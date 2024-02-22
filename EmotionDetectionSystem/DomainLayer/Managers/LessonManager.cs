using System.Text;
using EmotionDetectionSystem.DomainLayer.objects;
using EmotionDetectionSystem.RepoLayer;
using EmotionDetectionSystem.Service;
using EmotionDetectionSystem.ServiceLayer;

namespace EmotionDetectionSystem.DomainLayer.Managers;

public class LessonManager
{
    private IRepo<Lesson> _lessonRepo;

    public LessonManager()
    {
        _lessonRepo = new LessonRepo();
    }

    public Lesson CreateLesson(Teacher teacher, string title, string description, string[] tags)
    {
        Lesson newLesson = new Lesson(teacher, title, description, GenerateEntryCode(),
                                      tags.ToList());
        _lessonRepo.Add(newLesson);
        return newLesson;
    }

    public void EndLesson(string email)
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

    private static string GenerateEntryCode(int length = 15)
    {
        var random = new Random();

        const string allowedChars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

        var code = new StringBuilder();

        for (var i = 0; i < length; i++)
        {
            var index = random.Next(0, allowedChars.Length);
            code.Append(allowedChars[index]);
        }

        return code.ToString();
    }
}