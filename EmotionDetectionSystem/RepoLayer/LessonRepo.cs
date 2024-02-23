using EmotionDetectionSystem.DomainLayer.objects;

namespace EmotionDetectionSystem.RepoLayer;

public class LessonRepo : IRepo<Lesson>
{
    //Dictionary<teacherEmail, List<Lesson>>
    Dictionary<string,List<Lesson> > _lessons = new Dictionary<string, List<Lesson>>();
    public List<Lesson> GetAll()
    {
        return _lessons.Values.SelectMany(x => x).ToList();
    }

    public Lesson GetById(string id)
    {
        throw new NotImplementedException();
    }

    public void Add(Lesson item)
    {
        throw new NotImplementedException();
    }

    public void Update(Lesson item)
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
}