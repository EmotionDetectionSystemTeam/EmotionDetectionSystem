using System.Text;
using EmotionDetectionSystem.DomainLayer.objects;
using EmotionDetectionSystem.RepoLayer;
using EmotionDetectionSystem.Service;
using EmotionDetectionSystem.ServiceLayer;

namespace EmotionDetectionSystem.DomainLayer.Managers;

public class LessonManager
{
    private LessonRepo _lessonRepo;

    public LessonManager()
    {
        _lessonRepo = new LessonRepo();
    }

    public Lesson CreateLesson(Teacher teacher, string title, string description, string[] tags)
    {
        if (HasActiveLesson(teacher.Email))
        {
            throw new Exception("Teacher already has an active lesson");
        }

        var newLesson = new Lesson(teacher, title, description, GenerateEntryCode(),
                                   tags.ToList());
        _lessonRepo.Add(newLesson);
        return newLesson;
    }

    private bool HasActiveLesson(string email)
    {
        var lessons = _lessonRepo.GetByTeacherEmail(email);
        return lessons.Any(l => l.IsActive);
    }

    public void EndLesson(string email)
    {
        var lessons = _lessonRepo.GetByTeacherEmail(email);
        var lesson  = lessons.FirstOrDefault(l => l.IsActive);
        if (lesson == null)
        {
            throw new Exception("No active lesson found");
        }

        lesson.EndLesson();
        _lessonRepo.Update(lesson);
    }

    public Lesson JoinLesson(User user, string entryCode)
    {
        var lessons = _lessonRepo.GetByEntryCode(entryCode);
        var lesson  = lessons.FirstOrDefault(l => l.IsActive);
        if (lesson == null)
        {
            throw new Exception("Invalid entry code");
        }
        if(user.Type == UserType.Student.GetStringValue())
        {
            var student = (Student) user;
            if (lesson.ContainStudent(student))
            {
                throw new Exception("Student is already in the lesson");
            }
        }
        else
        {
            throw new Exception("Only students can join lessons");
        }
        lesson.AddUser(user);
        _lessonRepo.Update(lesson);
        return lesson;
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