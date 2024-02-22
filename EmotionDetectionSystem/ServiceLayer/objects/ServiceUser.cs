using EmotionDetectionSystem.DomainLayer.objects;

namespace EmotionDetectionSystem.ServiceLayer;

public class ServiceUser
{
    protected ServiceUser(string email, string firstName, string lastName)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }
    public ServiceUser(Teacher teacher)
    {
        Email = teacher.Email;
        FirstName = teacher.FirstName;
        LastName = teacher.LastName;
        Type = teacher.Type;
        Lessons = new List<ServiceLesson>();
        foreach (var lesson in teacher.Lessons)
        {
            Lessons.Add(new ServiceLesson(lesson));
        }
    }
    public ServiceUser(User user)
    {
        Email     = user.Email;
        FirstName = user.FirstName;
        LastName  = user.LastName;
        Type      = user.Type;
        Lessons   = new List<ServiceLesson>();

        if (user is not Teacher teacher) return;
        foreach (var lesson in teacher.Lessons)
        {
            Lessons.Add(new ServiceLesson(lesson));
        }
    }
    
    public ServiceUser(Student student)
    {
        Email     = student.Email;
        FirstName = student.FirstName;
        LastName  = student.LastName;
        Type      = student.Type;
        Lessons   = new List<ServiceLesson>();
    }

    public string              Email;
    public string              FirstName;
    public string              LastName;
    public string              Type;
    public List<ServiceLesson> Lessons;
    
}