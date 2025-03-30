using Newtonsoft.Json;

namespace hotel_booking_dto
{
    public class Response<T>
    {
        public T Result { get; set; }
        public bool success { get; set; }
        public string Message { get; set; }
        public int Code { get; set; }

        public Response(int statusCode,bool success,string msg, T data)
        {
            Result = data;
            this.success = success;
            Code = statusCode;
            Message = msg;
        }
        public Response()
        {
        }
        /// <summary>
        /// Sets the data to the appropriate response
        /// at run time
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static Response<T> Fail(string errorMessage, int statusCode = 404)
        {
            return new Response<T> { success = false, Message = errorMessage, Code = statusCode };
        }
        public static Response<T> Success(string successMessage,T data, int statusCode = 200)
        {
            return new Response<T> { success = true, Message = successMessage, Result = data, Code = statusCode};
        }

        // Phương thức tĩnh tạo phản hồi từ Exception
        public static Response<T> Fail(Exception ex, string message = "Lỗi xảy ra")
        {
            return new Response<T>(500, false, $"{message}: {ex.InnerException?.Message ?? ex.Message}",default);
        }
        public override string ToString() => JsonConvert.SerializeObject(this);       
    }
}
