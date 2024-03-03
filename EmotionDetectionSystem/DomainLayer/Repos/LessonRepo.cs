using EmotionDetectionSystem.DomainLayer.objects;

namespace EmotionDetectionSystem.DomainLayer.Repos;

public class LessonRepo : IRepo<Lesson>
{
    //Dictionary<teacherEmail, List<Lesson>>
    private Dictionary<string, List<Lesson>> _lessons = new Dictionary<string, List<Lesson>>();

    public List<Lesson> GetAll()
    {
        return _lessons.Values.SelectMany(x => x).ToList();
    }

    public List<Lesson> GetByEntryCode(string entryCode)
    {
        return _lessons.Values.SelectMany(x => x).Where(x => x.EntryCode.Equals(entryCode)).ToList();
    }

    public Lesson GetById(string id)
    {
        return _lessons.Values.SelectMany(x => x).FirstOrDefault(x => x.LessonId.Equals(id)) ??
               throw new Exception("Lesson not found");
    }

    public void Add(Lesson item)
    {
        if (_lessons.TryGetValue(item.Teacher.Email, out var value))
        {
            value.Add(item);
        }
        else
        {
            _lessons.Add(item.Teacher.Email, new List<Lesson> { item });
        }
    }

    public void Update(Lesson item)
    {
    }

    public void Delete(string id)
    {
        throw new NotImplementedException();
    }

    public bool ContainsID(string id)
    {
        throw new NotImplementedException();
    }

    public bool ContainsValue(Lesson item)
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

    public List<Lesson> GetByTeacherEmail(string email)
    {
        return _lessons.TryGetValue(email, out var value) ? value : new List<Lesson>();
    }
}