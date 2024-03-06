namespace EmotionDetectionServer.API
{
    public class ServerResponse<T>
    {
        public T value { get; set; }
        public string errorMessage { get; set; }
        public static ServerResponse<T> sendOkResponse(T val)
        {
            var response = new ServerResponse<T>
            {
                value = val,
            };
            return response;
        }
        public static ServerResponse<T> sendBadResponse(string msg)
        {
            var response = new ServerResponse<T>
            {
                errorMessage = msg,
            };
            return response;
        }
        // Add other properties as needed for more complex objects
    }
}