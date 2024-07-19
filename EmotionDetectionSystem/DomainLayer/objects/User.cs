using EmotionDetectionSystem.DomainLayer.Managers;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmotionDetectionSystem.DomainLayer.objects;
[Table("User")]
public abstract class User
{
    protected NotificationManager _notificationManager = NotificationManager.GetInstance();
    protected User(string email, string firstName, string lastName, string password)
    {
        _email = email;
        _firstName = firstName;
        _lastName = lastName;
        _password = password;
    }
    public User() { }
    private string _id;
    private string _email;
    private string _firstName;
    private string _lastName;
    private string _password;
    protected string _type;

    public string Id
    {
        get => _id;
        set => _id = value;
    }
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
    public string GetFullName()
    {
        return _firstName + " " + _lastName;
    }

    public abstract void Leave(Lesson lesson);
    
    public void Notify(string msg)
    {
        _notificationManager.SendNotification(msg, Email);
    }
}