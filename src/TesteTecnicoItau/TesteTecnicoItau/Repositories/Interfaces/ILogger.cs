namespace TesteTecnicoItau.Repositories.Interfaces
{
    public interface ILogger
    {
        void LogInfo(string message);
        void LogError(string message, Exception ex);
        void LogWarning(string messsage);
    }
}