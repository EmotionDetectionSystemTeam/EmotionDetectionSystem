namespace EmotionDetectionSystem.DomainLayer.objects;

public class Teacher: User
{
    private List<Lesson> _lessons;
    public List<Lesson> Lessons { get => _lessons; set => _lessons = value; }
    

    public Teacher(string email, string firstName, string lastName, string password, List<Lesson> lessons)
        : base(email, firstName, lastName, password)
    {
        _lessons = lessons;
        _type = "Teacher";
    }
    
    public Teacher(string email, string firstName, string lastName, string password)
        : base(email, firstName, lastName, password)
    {
        _lessons = new List<Lesson>();
        _type = "Teacher";
    }

}