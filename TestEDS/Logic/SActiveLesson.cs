using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TestEDS.ServerTests.MultipleStudentsSendsEmotionToServer;

namespace TestEDS.Logic
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
        public List<ServiceRealTimeUser> StudentsEmotions { get; set; }


    }

}
