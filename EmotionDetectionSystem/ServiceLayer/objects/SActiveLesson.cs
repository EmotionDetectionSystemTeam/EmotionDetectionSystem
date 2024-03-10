﻿using EmotionDetectionSystem.DomainLayer.objects;

namespace EmotionDetectionSystem.ServiceLayer.objects
{
    [Serializable]
    public class SActiveLesson
    {
        public string LessonId { get; set; }
        public string LessonName { get; set; }
        public string Teacher { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
        public string EntryCode { get; set; }
        public int studentsQuantity { get; set; }
        public Dictionary<string, List<string>> StudentsEmotions { get; set; }


        public SActiveLesson(Lesson lesson)
        {
            StudentsEmotions = lesson.GetStudentWiningEmotions();
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
