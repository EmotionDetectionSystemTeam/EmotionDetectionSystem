

namespace EmotionDetectionSystem.ServiceLayer.Responses
{
    [Serializable]
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
