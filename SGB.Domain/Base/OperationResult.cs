namespace SGB.Domain.Base
{
    public class OperationResult<T>
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; } = string.Empty;
        public T? Data { get; private set; }

        private OperationResult(bool isSuccess, string message, T? data = default)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }
        public static OperationResult<T> Success(T data, string message = "")
        {
            return new OperationResult<T>(true, message, data);
        }
        public static OperationResult<T> Failure(string message)
        {
            return new OperationResult<T>(false, message);
        }
    }
}