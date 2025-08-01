using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SGB.Application.Wrappers
{
    public static class ApiResponseFactory
    {
        public static ApiResponse<T> Success<T>(T data, string message = "")
            => new() { IsSuccess = true, Message = message, Data = data };

        public static ApiResponse<T> Fail<T>(string message)
            => new() { IsSuccess = false, Message = message, Data = default };
    }
}