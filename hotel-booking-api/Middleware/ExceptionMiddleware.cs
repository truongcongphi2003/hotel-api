using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace thda
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;


        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); 
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex); 
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {

            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            var traceId = context.TraceIdentifier;
            _logger.LogError(exception, "An error occurred while processing the request. TraceId: {TraceId}", context.TraceIdentifier);
            var errorDetails = new ErrorDetails
            {
                ErrorCode = statusCode,
                ErrorMessage = exception.Message,
                ErrorType = "Fail",
                TraceId = traceId
            };

            switch (exception)
            {
                case BadRequestException:
                    statusCode = HttpStatusCode.BadRequest;
                    errorDetails.ErrorType = "BadRequest";
                    break;

                case ValidationException validationException:
                    statusCode = HttpStatusCode.BadRequest;
                    errorDetails.ErrorType = "Validation";
                    errorDetails.ErrorMessage = validationException.Message;
                    // Thêm lỗi chi tiết nếu có
                    errorDetails.Errors = validationException.Data as IDictionary<string, string[]>;
                    break;

                case NotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    errorDetails.ErrorType = "NotFound";
                    errorDetails.ErrorMessage = notFoundException.Message;
                    break;

                case UnauthorizedAccessException unauthorizedException:
                    statusCode = HttpStatusCode.Unauthorized;
                    errorDetails.ErrorType = "Unauthorized";
                    errorDetails.ErrorMessage = unauthorizedException.Message;
                    break;

                default:
                    // Xử lý các lỗi không mong đợi
                    errorDetails.ErrorMessage = "An unexpected error occurred.";
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsync(JsonSerializer.Serialize(errorDetails));
        }

       
    }

    public class ErrorDetails
    {
        public HttpStatusCode? ErrorCode { get; set; }
        public string? ErrorType { get; set; }
        public string? ErrorMessage { get; set; }
        public IDictionary<string, string[]> Errors { get; set; }
        public string? TraceId { get; set; }

    }

    // Các ngoại lệ tùy chỉnh
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) { }
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }
}
