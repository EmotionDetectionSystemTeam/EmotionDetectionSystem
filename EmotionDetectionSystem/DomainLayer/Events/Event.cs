using EmotionDetectionSystem.DomainLayer.objects;

namespace EmotionDetectionSystem.DomainLayer.Events;

public abstract class Event
{
    protected Event(string name)
    {
        Name = name;
    }

    public string Name { get; set; }

    public void Update(Teacher user)
    {
        user.Notify(GenerateMsg());
    }
    public abstract string GenerateMsg();
}