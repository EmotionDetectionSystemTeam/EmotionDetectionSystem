namespace EmotionDetectionSystem.DomainLayer.Events;

public class StudentLeftLessonEvent: Event
{
    public StudentLeftLessonEvent(string name) : base(name)
    {
    }

    public override string GenerateMsg()
    {
        throw new NotImplementedException();
    }
}