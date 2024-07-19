using EmotionDetectionSystem.DataLayer;
using EmotionDetectionSystem.DomainLayer.objects;
using Microsoft.IdentityModel.Tokens;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace EmotionDetectionSystem.DomainLayer.Repos;

public class LessonRepo : IRepo<Lesson>
{
    //Dictionary<teacherEmail, List<Lesson>>
    private Dictionary<string, List<Lesson>> _lessonsByEmail = new Dictionary<string, List<Lesson>>();
    private Dictionary<string, Lesson> _lessonsByEntryCode = new Dictionary<string, Lesson>();
    public void ClearCache()
    {
        _lessonsByEmail.Clear();
        _lessonsByEntryCode.Clear();
    }
    private void CacheLesson(Lesson lesson, bool update = false)
    {
        if (!_lessonsByEmail.TryGetValue(lesson.Teacher.Email, out var lessons))
        {
            lessons = new List<Lesson>();
            _lessonsByEmail[lesson.Teacher.Email] = lessons;
        }
        else
        {
            var existingLesson = lessons.FirstOrDefault(l => l.LessonId == lesson.LessonId);
            if (existingLesson != null)
            {
                lessons.Remove(existingLesson);
            }
        }
        lessons.Add(lesson);
        if (_lessonsByEntryCode.ContainsKey(lesson.EntryCode))
        {
            _lessonsByEntryCode.Remove(lesson.EntryCode);
        }
        _lessonsByEntryCode.Add(lesson.EntryCode, lesson);
    }

    public List<Lesson> GetAll()
    {
        var allLessons = DBHandler.Instance.GetAllLessons();
        foreach (var lesson in allLessons)
        {
            CacheLesson(lesson);
        }
        return allLessons;
    }

    public List<Lesson> GetByEntryCode(string entryCode)
    {
        if (_lessonsByEntryCode.TryGetValue(entryCode, out var lesson))
        {
            return new List<Lesson> { lesson };
        }
        return new List<Lesson> { DBHandler.Instance.GetByEntryCode(entryCode), };
    }

    public Lesson GetById(string id)
    {
        Lesson lesson;
        List<Lesson> cached_lessons = _lessonsByEmail.Values.SelectMany(x => x).Where(x => x.LessonId.Equals(id)).ToList();

        if (!cached_lessons.IsNullOrEmpty())
        {
            return cached_lessons[0];
        }
        else
        {
            lesson = DBHandler.Instance.GetLessonById(id);
            CacheLesson(lesson);
        }
        return lesson;
    }

    public void Add(Lesson item)
    {
        DBHandler.Instance.AddLesson(item);
        CacheLesson(item);
    }
    public void Update(Lesson item)
    {
        DBHandler.Instance.UpdateLesson(item);
        CacheLesson(item, update: true);
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
        if (_lessonsByEmail.TryGetValue(email, out var lessons))
        {
            return lessons;
        }

        lessons = DBHandler.Instance.GetByTeacherEmail(email);
        foreach (var lesson in lessons)
        {
            CacheLesson(lesson);
        }
        return lessons;
    }
}