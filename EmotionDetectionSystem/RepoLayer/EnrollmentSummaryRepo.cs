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
        _enrollmentSummaries.Add(item);
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
        return _enrollmentSummaries.Exists(x => x.Student.Email.Equals(student.Email));
    }

    public void PutEmotionData(string userEmail, EmotionData emotionData)
    {
        var enrollmentSummary = _enrollmentSummaries.Find(x => x.Student.Email.Equals(userEmail));
        if (enrollmentSummary == null)
        {
            throw new Exception($" Student with email {userEmail} not found in lesson");
        }
        enrollmentSummary.AddEmotionData(emotionData);
    }

    public IEnumerable<EmotionData> GetEmotionDataEntries()
    {
        var emotionsData = new List<EmotionData>();
        return emotionsData.Concat(_enrollmentSummaries.SelectMany(x => x.EmotionData));
    }
}