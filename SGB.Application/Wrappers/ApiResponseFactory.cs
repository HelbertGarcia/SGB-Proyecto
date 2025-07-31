using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SGB.Application.Wrappers
{
    public static class ApiResponseFactory
    {
        public static ApiResponse<T> Success<T>(T data, string message = "")
            => new() { IsSuccess = true, Message = message, Data = data };

        public static ApiResponse<T> Fail<T>(string message)
            => new() { IsSuccess = false, Message = message, Data = default };

        public static ApiResponse<T> Fail<T>(string message, ModelStateDictionary modelState)
        {
            var errors = modelState
                .Where(ms => ms.Value.Errors.Any())
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = message,
                Errors = errors
            };
        }
    }
}