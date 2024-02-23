using EmotionDetectionSystem.DomainLayer.objects;

namespace EmotionDetectionSystem.ServiceLayer;

public class ServiceTeacher : ServiceUser
{
    public List<ServiceLesson> Lessons;
    public ServiceTeacher(string email, string firstName, string lastName, List<ServiceLesson> lessons)
        : base(email, firstName, lastName)
    {
        Type = "Teacher";
        Lessons = lessons;
    }
    
    public ServiceTeacher(Teacher teacher) : base(teacher.Email, teacher.FirstName, teacher.LastName)
    {
        Type = "Teacher";
        Lessons = new List<ServiceLesson>();
        foreach (var lesson in teacher.Lessons)
        {
            Lessons.Add(new ServiceLesson(lesson));
        }
    }
}