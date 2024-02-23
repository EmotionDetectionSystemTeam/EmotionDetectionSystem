using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionDetectionSystem.Service
{
    [Serializable]
    ///<summary>This class extends<c>Response</c> and represents the result of a call to a non-void function.
    ///In addition to the behavior of <c>Response</c>, the class holds the value of the returned value in the variable <c>Value</c>.</summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Market.ServiceLayer.Response" />

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

