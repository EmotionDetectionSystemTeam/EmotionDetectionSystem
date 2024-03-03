using System.Text;
using EmotionDetectionSystem.DomainLayer.objects;
using EmotionDetectionSystem.DomainLayer.Repos;
using EmotionDetectionSystem.ServiceLayer;
using EmotionDetectionSystem.ServiceLayer.objects;
using EmotionDetectionSystem.ServiceLayer.Responses;
using log4net;

namespace EmotionDetectionSystem.DomainLayer.Managers;

public class LessonManager
{
    private LessonRepo _lessonRepo;
    private long _lessonIdFactory = 1;
    private static readonly ILog Log = LogManager.GetLogger(typeof(LessonManager));

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

        var newLesson = new Lesson(_lessonIdFactory++.ToString(),teacher, title, description, GenerateEntryCode(),
                                   tags.ToList());
        _lessonRepo.Add(newLesson);
        teacher.AddLesson(newLesson);
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
        user.JoinLesson(lesson);
        return lesson;
    }

    public List<EnrollmentSummary> ViewStudentsDuringLesson(Viewer viewer, string lessonId)
    {
        var lesson = _lessonRepo.GetById(lessonId);
        if (!lesson.IsAllowedToViewStudentsData(viewer))
        {
            throw new Exception("Viewer is not allowed to view students data");
        }

        return lesson.GetEnrollmentSummaries();
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

    public Lesson GetLesson(string lessonId)
    {
        return _lessonRepo.GetById(lessonId);
    }
    
    public void AddViewer(string lessonId, Viewer viewer)
    {
        var lesson = _lessonRepo.GetById(lessonId);
        lesson.AddViewer(viewer);
        _lessonRepo.Update(lesson);
    }
}