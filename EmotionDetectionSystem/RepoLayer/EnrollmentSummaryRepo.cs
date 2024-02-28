using EmotionDetectionSystem.DomainLayer.objects;

namespace EmotionDetectionSystem.RepoLayer;

public class EnrollmentSummaryRepo : IRepo<EnrollmentSummary>
{
    private string _lessonId;
    private List<EnrollmentSummary> _enrollmentSummaries;
    

    public EnrollmentSummaryRepo(string lessonId)
    {
        _lessonId = lessonId;
        _enrollmentSummaries = new List<EnrollmentSummary>();
    }
    
    public List<EnrollmentSummary> GetAll()
    {
        return _enrollmentSummaries;
    }

    public EnrollmentSummary GetById(string id)
    {
        throw new NotImplementedException();
    }

    public void Add(EnrollmentSummary item)
    {
        throw new NotImplementedException();
    }

    public void Update(EnrollmentSummary item)
    {
        throw new NotImplementedException();
    }

    public void Delete(string id)
    {
        throw new NotImplementedException();
    }

    public bool ContainsID(string id)
    {
        throw new NotImplementedException();
    }

    public bool ContainsValue(EnrollmentSummary item)
    {
        throw new NotImplementedException();
    }

    public void ResetDomainData()
    {
        throw new NotImplementedException();
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public bool ContainStudent(Student student)
    {
        throw new NotImplementedException();
    }

    public void PutEmotionData(string userEmail, EmotionData emotionData)
    {
        var enrollmentSummary = _enrollmentSummaries.Find(x => x.Student.Email.Equals(userEmail))!;
        enrollmentSummary.AddEmotionData(emotionData);
    }
}