namespace EmotionDetectionSystem.ServiceLayer;

public class ServiceStudent : ServiceUser
{
    public override string _type { get; set; }
    public ServiceStudent(string email, string firstName, string lastName, List<ServiceLesson> lessons)
        : base(email, firstName, lastName, lessons)
    {
        _type = "Student";
    }
}