namespace EmotionDetectionSystem.DomainLayer.Repos;

public interface IRepo<T>
{
    List<T> GetAll();
    T       GetById(string    id);
    void    Add(T             item);
    void    Update(T          item);
    void    Delete(string     id);
    Boolean ContainsID(string id);
    Boolean ContainsValue(T item);
    void    ResetDomainData();
    void    Clear();
}