namespace EmotionDetectionSystem.DomainLayer.objects;

public class Admin : User
{
    public Admin(string email, string firstName, string lastName, string password) : base(email, firstName, lastName, password)
    {
    }
}