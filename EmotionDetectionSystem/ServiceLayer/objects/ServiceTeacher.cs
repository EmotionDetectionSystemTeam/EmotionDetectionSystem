using EmotionDetectionSystem.DomainLayer.objects;

namespace EmotionDetectionSystem.ServiceLayer.objects;

public class ServiceTeacher : ServiceUser
{
    public ServiceTeacher(string email, string firstName, string lastName, List<ServiceLesson> lessons)
        : base(email, firstName, lastName,"Teacher")
    {
    }
    
    public ServiceTeacher(Teacher teacher) : base(teacher.Email, teacher.FirstName, teacher.LastName,teacher.Type, teacher.Lessons)
    {
    }
}