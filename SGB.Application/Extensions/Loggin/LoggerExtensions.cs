namespace SGB.Api.Extensions.Loggin
{
    public static class LoggerExtensions
    {
        public interface IAppLogger<T>
        {
            void Info(string message, params object[] args);
            void Error(string message, params object[] args);
            void Error(Exception exception, string message, params object[] args);
        }
    }
}