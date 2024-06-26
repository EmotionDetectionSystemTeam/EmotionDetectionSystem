namespace EmotionDetectionSystem.DomainLayer.objects;

public class Admin : User, Viewer
{
    public Admin(string email, string firstName, string lastName, string password) : base(
        email, firstName, lastName, password)
    {
    }

    public override void JoinLesson(Lesson lesson)
    {
        lesson.AddViewer(this);
    }

    public override void Leave(Lesson lesson)
    {
        // leave as viewer
        lesson.Leave(this as Viewer);
    }
}