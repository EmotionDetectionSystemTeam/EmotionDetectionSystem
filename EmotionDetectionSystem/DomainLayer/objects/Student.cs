namespace EmotionDetectionSystem.DomainLayer.objects;

public class Student : User
{
    public Student(string email, string firstName, string lastName, string password) :
        base(email, firstName, lastName, password)
    {
        Type = "Student";
    }

    public override void JoinLesson(Lesson lesson)
    {
        lesson.AddStudent(this);
    }
}