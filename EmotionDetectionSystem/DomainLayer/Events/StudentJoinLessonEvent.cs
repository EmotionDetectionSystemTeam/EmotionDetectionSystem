namespace EmotionDetectionSystem.DomainLayer.Events;

public class StudentJoinLessonEvent: Event
{
    public StudentJoinLessonEvent(string name) : base(name)
    {
    }

    public override string GenerateMsg()
    {
        throw new NotImplementedException();
    }
}