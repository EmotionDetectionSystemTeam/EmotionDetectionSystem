namespace EmotionDetectionSystem.ServiceLayer;

public abstract class ServiceUser
{
    protected ServiceUser(string email, string firstName, string lastName, List<ServiceLesson> lessons)
    {
        _email = email;
        _firstName = firstName;
        _lastName = lastName;
        _lessons = lessons;
    }

    public string _email { get; set; }
    public string _firstName { get; set; }
    public string _lastName { get; set; }
    public List<ServiceLesson> _lessons { get; set; }
    public abstract string _type { get; set; }
}