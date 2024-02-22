using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionDetectionSystem.Service
{
    [Serializable]
    ///<summary>Class <c>Response</c> represents the result of a call to a void function. 
    ///If an exception was thrown, <c>ErrorOccured = true</c> and <c>ErrorMessage != null</c>. 
    ///Otherwise, <c>ErrorOccured = false</c> and <c>ErrorMessage = null</c>.</summary>
    public class Response
    {  ///<summary>The error message. If an exception was thrown, <c>ErrorOccured = true</c> and <c>ErrorMessage != null</c>.</summary>
        public readonly string ErrorMessage;
        public bool ErrorOccured { get => ErrorMessage != null; }
        internal Response() { }
        internal Response(string msg)
        {
            ErrorMessage = msg;
        }
    }
}
