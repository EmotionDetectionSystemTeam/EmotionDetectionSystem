using EmotionDetectionSystem.DomainLayer.objects;

namespace EmotionDetectionSystem.ServiceLayer;

public class ServiceStudent : ServiceUser
{
    public ServiceStudent(string email, string firstName, string lastName)
        : base(email, firstName, lastName)
    {
        Type = "Student";
    }
    
    public ServiceStudent(Student student) : base(student.Email, student.FirstName, student.LastName)
    {
        Type = "Student";
    }
}