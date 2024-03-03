using EmotionDetectionSystem.DomainLayer.objects;
using EmotionDetectionSystem.ServiceLayer.objects;

namespace EmotionDetectionSystem.ServiceLayer;

public class ServiceStudent : ServiceUser
{
    public ServiceStudent(string email, string firstName, string lastName)
        : base(email, firstName, lastName,"Student")
    {
    }
    
    public ServiceStudent(Student student) : base(student.Email, student.FirstName, student.LastName, student.Type)
    {
    }
}