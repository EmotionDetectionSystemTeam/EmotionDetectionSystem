namespace EmotionDetectionSystem.ServiceLayer.Responses
{
    [Serializable]
    public class Response<T> : Response
    {
        public readonly T Value;
        private Response(T value, string msg) : base(msg)
        {
            this.Value = value;
        }

        public static Response<T> FromValue(T value)
        {
            return new Response<T>(value, null);
        }

        public static Response<T> FromError(string msg)
        {
            return new Response<T>(default(T), msg);
        }
    }
}

