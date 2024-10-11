using System.Net;

namespace ToDo_API.Responses
{
    public class Response<T>
    {
        public bool Success { get; set; } 
        public HttpStatusCode StatusCode { get; set; } 
        public string Message { get; set; } 
        public T Data { get; set; } // Holds the data in case of success
        public List<string> Errors { get; set; } // Holds validation errors or error details

        
        public static Response<T> CreateSuccess(T data, string message = "Operation was successful", HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new Response<T>
            {
                Success = true,
                Data = data,
                StatusCode = statusCode,
                Message = message,
                Errors = null // No errors on success
            };
        }

   
        public static Response<T> Failure(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest, List<string> errors = null)
        {
            return new Response<T>
            {
                Success = false,
                Data = default,
                StatusCode = statusCode,
                Message = message,
                Errors = errors // Optional error details or validation errors
            };
        }

      
        public static Response<T> ValidationFailure(List<string> errors)
        {
            return new Response<T>
            {
                Success = false,
                Data = default,
                StatusCode = HttpStatusCode.BadRequest,
                Message = "Validation Failed",
                Errors = errors // Validation errors list
            };
        }
    }
}
