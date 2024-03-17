using EmotionDetectionSystem.DomainLayer.objects;

namespace EmotionDetectionSystem.ServiceLayer.objects
{
    [Serializable]
    public class SActiveLesson
    {
        public string   LessonId         { get; set; }
        public string   LessonName       { get; set; }
        public string   Teacher          { get; set; }
        public DateTime Date             { get; set; }
        public bool     IsActive         { get; set; }
        public string   EntryCode        { get; set; }
        public int      studentsQuantity { get; set; }
        public List<ServiceRealTimeUser>   StudentsEmotions { get; set; }


        public SActiveLesson(Lesson lesson)
        {
            StudentsEmotions = lesson.GetStudentsEmotions()
                .Where(entry => entry.Value.PeekFirstNotSeenEmotionData()?.GetWinningEmotion() != null)
                .Select(entry => 
                            new ServiceRealTimeUser(
                                entry.Key, 
                                entry.Value.PeekFirstNotSeenEmotionData().GetWinningEmotion(),
                                entry.Value.getPreviousEmotionData()
                            )
                )
                .ToList();

            LessonId = lesson.LessonId;
            LessonName = lesson.LessonName;
            Teacher = lesson.Teacher.FirstName + " " + lesson.Teacher.LastName;
            Date = lesson.Date;
            IsActive = lesson.IsActive;
            EntryCode = lesson.EntryCode;
            studentsQuantity = lesson.Viewers.Count();
        }

    }
}
