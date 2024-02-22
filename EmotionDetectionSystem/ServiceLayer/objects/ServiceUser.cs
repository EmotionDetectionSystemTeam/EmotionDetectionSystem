namespace EmotionDetectionSystem.ServiceLayer;

public abstract class ServiceUser
{
    protected ServiceUser(string email, string firstName, string lastName)
    {
        _email = email;
        _firstName = firstName;
        _lastName = lastName;
    }

    private string _email;
    private string _firstName;
    private string _lastName;
    private string _type;
    
    public string Email { get => _email; set => _email = value; }
    public string FirstName { get => _firstName; set => _firstName = value; }
    public string LastName { get => _lastName; set => _lastName = value; }
    public string Type { get => _type; set => _type = value; }
    public List<ServiceLesson> Lessons { get; set; }
}