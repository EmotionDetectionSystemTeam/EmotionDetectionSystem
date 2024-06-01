using EmotionDetectionSystem.DomainLayer.objects;

namespace EmotionDetectionSystem.ServiceLayer.objects.charts;

public class StudentOverview
{
    public string                              FirstName { get; set; }
    public string                              LastName  { get; set; }
    public string                              Email     { get; set; }
    public IEnumerable<StudentInClassOverview> Lessons   { get; set; }

    public StudentOverview(User student, IEnumerable<EnrollmentSummary> enrollmentSummary)
    {
        FirstName = student.FirstName;
        LastName = student.LastName;
        Email = student.Email;
        Lessons = enrollmentSummary.Select(x => new StudentInClassOverview(x)).ToList();
    }
}