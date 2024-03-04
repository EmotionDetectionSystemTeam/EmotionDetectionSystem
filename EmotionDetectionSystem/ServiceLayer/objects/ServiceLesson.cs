using EmotionDetectionSystem.DomainLayer.objects;
using EmotionDetectionSystem.ServiceLayer.objects;

namespace EmotionDetectionSystem.ServiceLayer;

public class ServiceLesson
{
    public string                         LessonId;
    public string                         LessonName;
    public ServiceTeacher                 Teacher;
    public DateTime                       Date;
    public bool                           IsActive;
    public string                         EntryCode;
    public List<ServiceEnrollmentSummary> Emotions;
    public List<string>                   Tags;
    
    public ServiceLesson(Lesson lesson)
    {
        LessonId = lesson.LessonId;
        LessonName = lesson.LessonName;
        Teacher = new ServiceTeacher(lesson.Teacher);
        Date = lesson.Date;
        IsActive = lesson.IsActive;
        EntryCode = lesson.EntryCode;
        Emotions = new List<ServiceEnrollmentSummary>();
        Tags = lesson.Tags;
    }
}