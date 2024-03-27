using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEDS.Logic.RequestsObj
{
    public class Value
    {
        public string lessonId { get; set; }
        public string lessonName { get; set; }
        public string teacher { get; set; }
        public DateTime date { get; set; }
        public bool isActive { get; set; }
        public string entryCode { get; set; }
        public int studentsQuantity { get; set; }
    }
}
