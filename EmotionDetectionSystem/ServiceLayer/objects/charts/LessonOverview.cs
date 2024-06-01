using EmotionDetectionSystem.DomainLayer.objects;

namespace EmotionDetectionSystem.ServiceLayer.objects.charts;

public class LessonOverview
{
    public string   LessonId    { get; set; }
    public string   LessonName  { get; set; }
    public string   Description { get; set; }
    public DateTime Date        { get; set; }
    public LessonOverview(Lesson lesson)
    {
        LessonId = lesson.LessonId;
        LessonName = lesson.LessonName;
        Description = lesson.Description;
        Date = lesson.Date;
    }
}