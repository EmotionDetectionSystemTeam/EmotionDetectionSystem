using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEDS.Logic.RequestsObj
{
    [Serializable]
    public class ServiceEmotionData
    {
        public DateTime Time { get; set; }
        public string WinningEmotion { get; set; }

        public ServiceEmotionData(string winningEmotion, DateTime time)
        {
            Time = time;
            WinningEmotion = winningEmotion;
        }
    
    }

}
