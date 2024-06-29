using EmotionDetectionSystem.DomainLayer.objects;

namespace EmotionDetectionSystem.DomainLayer.Events;

public class TeacherApproachEvent : Event
{
    private Student  _student;
    private Teacher  _teacher;
    private DateTime _date;

    public TeacherApproachEvent(Student student, Teacher teacher) : base("TeacherApproachEvent")
    {
        _student = student;
        _teacher = teacher;
        _date    = DateTime.Now;
    }

    public override string GenerateMsg()
    {
        return $"[{_date}]: {_teacher.FirstName} {_teacher.LastName} approached {_student.FirstName} {_student.LastName}";
    }
}