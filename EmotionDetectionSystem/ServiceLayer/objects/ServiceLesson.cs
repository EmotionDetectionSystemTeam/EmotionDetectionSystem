using EmotionDetectionSystem.DomainLayer.objects;

namespace EmotionDetectionSystem.ServiceLayer;

public class ServiceLesson
{
    public string                         LessonName;
    public ServiceTeacher                 Teacher;
    public DateTime                       Date;
    public bool                           IsActive;
    public string                         EntryCode;
    public List<ServiceEnrollmentSummary> Emotions;
    public List<string>                   Tags;
    
    public ServiceLesson(string lessonName, ServiceTeacher teacher, DateTime date, bool isActive, string entryCode, List<ServiceEnrollmentSummary> emotions, List<string> tags)
    {
        LessonName = lessonName;
        Teacher = teacher;
        Date = date;
        IsActive = isActive;
        EntryCode = entryCode;
        Emotions = emotions;
        this.Tags = tags;
    }
    
    public ServiceLesson(Lesson lesson)
    {
        LessonName = lesson.LessonName;
        Teacher = new ServiceTeacher(lesson.Teacher);
        Date = lesson.Date;
        IsActive = lesson.IsActive;
        EntryCode = lesson.EntryCode;
        Emotions = new List<ServiceEnrollmentSummary>();
        Tags = lesson.Tags;
    }
}