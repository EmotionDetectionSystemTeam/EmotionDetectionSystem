namespace EmotionDetectionServer.API
{
    public class ServerResponse<T>
    {
        public T value { get; set; }
        public string errorMessage { get; set; }
    }
}