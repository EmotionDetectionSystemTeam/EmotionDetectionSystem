using EmotionDetectionSystem.DomainLayer.objects;

namespace EmotionDetectionSystem.ServiceLayer.objects;

public class ServiceRealTimeUser
{
    protected ServiceRealTimeUser(string email, string firstName, string lastName, string winingEmotion)
    {
        Email         = email;
        FirstName     = firstName;
        LastName      = lastName;
        WiningEmotion = winingEmotion;
    }

    public ServiceRealTimeUser(User user, string winingEmotion)
    {
        Email         = user.Email;
        FirstName     = user.FirstName;
        LastName      = user.LastName;
        WiningEmotion = winingEmotion;
    }

    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string WiningEmotion { get; set; }
}