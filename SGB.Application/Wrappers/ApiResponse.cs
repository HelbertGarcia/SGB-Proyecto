namespace SGB.Application.Wrappers
{
    public class ApiResponse<T>
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public T Data { get; set; }
    }
}