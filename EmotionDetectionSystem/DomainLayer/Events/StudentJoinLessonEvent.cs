using EmotionDetectionSystem.DomainLayer.objects;

namespace EmotionDetectionSystem.DomainLayer.Events;

public class StudentJoinLessonEvent: Event
{
    private readonly Student _student;
    public StudentJoinLessonEvent(Student student) : base("Student Join Lesson Event")
    {
        _student = student;
    }

    public override string GenerateMsg()
    {
        return $"Student {_student.FirstName} {_student.LastName} joined the lesson.";
    }
}