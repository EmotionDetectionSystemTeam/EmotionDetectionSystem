using EmotionDetectionSystem.DomainLayer.objects;

namespace EmotionDetectionSystem.DomainLayer;

public class SurprisedEvent: Event
{
    private Student _student;
    public SurprisedEvent(string name, Student student) : base(name)
    {
        _student = student;
    }

    public override string GenerateMsg()
    {
        return _student.Email;
    }
}