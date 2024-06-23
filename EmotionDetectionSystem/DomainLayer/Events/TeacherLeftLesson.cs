using EmotionDetectionSystem.DomainLayer.objects;

namespace EmotionDetectionSystem.DomainLayer.Events;

public class TeacherLeftLesson: Event
{
    private Teacher _teacher;
    public TeacherLeftLesson(Teacher teacher) : base("Teacher Left Lesson Event")
    {
        _teacher = teacher;
    }

    public override string GenerateMsg()
    {
        return $"Teacher {_teacher.FirstName} {_teacher.LastName} left the lesson.";
    }
}