using EmotionDetectionSystem.DomainLayer.objects;

namespace EmotionDetectionSystem.DomainLayer.Events;

public class LessonEndedEvent: Event
{
    private Lesson _lesson;
    public LessonEndedEvent(Lesson lesson) : base("Lesson Finished Event")
    {
        _lesson = lesson;
    }

    public override string GenerateMsg()
    {
        return $"Lesson {_lesson.LessonName} has ended";
    }
}