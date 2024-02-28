namespace EmotionDetectionSystem.DomainLayer.objects;

public abstract class User
{
    protected User(string email, string firstName, string lastName, string password)
    {
        _email = email;
        _firstName = firstName;
        _lastName = lastName;
        _password = password;
    }

    private string _email;
    private string _firstName;
    private string _lastName;
    private string _password;
    protected string _type;
    
    public string Email
    {
        get => _email;
        set => _email = value;
    }
    public string FirstName
    {
        get => _firstName;
        set => _firstName = value;
    }
    public string LastName
    {
        get => _lastName;
        set => _lastName = value;
    } 
    public string Password
    {
        get => _password;
        set => _password = value;
    }
    public string Type
    {
        get => _type;
        set => _type = value;
    }

    public abstract void JoinLesson(Lesson lesson);
    
}