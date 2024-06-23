using EmotionDetectionSystem.DomainLayer.objects;

namespace EmotionDetectionSystem.DomainLayer.Events;

public class StudentLeftLessonEvent: Event
{
    private Student student;
    public StudentLeftLessonEvent(Student student) : base("Student Left Lesson Event")
    {
        this.student = student;
    }

    public override string GenerateMsg()
    {
        return $"Student {student.FirstName} {student.LastName} left the lesson.";
    }
}