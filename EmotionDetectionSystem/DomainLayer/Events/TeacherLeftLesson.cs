namespace EmotionDetectionSystem.DomainLayer.Events;

public class TeacherLeftLesson: Event
{
    private string _firstName;
    private string _lastName;
    public TeacherLeftLesson(string firstName, string lastName) : base("Teacher Left Lesson Event")
    {
        _firstName = firstName;
        _lastName = lastName;
    }

    public override string GenerateMsg()
    {
        return $"Teacher {Name} left the lesson.";
    }
}