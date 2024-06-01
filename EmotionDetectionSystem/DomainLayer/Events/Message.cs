namespace EmotionDetectionSystem.DomainLayer;

public class Message
{
    public Message(string comment)
    {
        Comment = comment;
    }
    public Message(string comment, bool seen)
    {
        Comment = comment;
    }
    
    public string Comment { get; set; }
}