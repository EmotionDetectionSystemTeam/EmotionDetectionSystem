namespace EmotionDetectionSystem.ServiceLayer;

public class ServiceTeacher : ServiceUser
{
    public override string _type { get; set; }
    public ServiceTeacher(string email, string firstName, string lastName, List<ServiceLesson> lessons)
        : base(email, firstName, lastName, lessons)
    {
        _type = "Teacher";
    }
}