using TesteTecnicoItau.Repositories.Interfaces;

namespace TesteTecnicoItau.Logging
{
    public class ConsoleLogger : ILogger
    {
        public void LogInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"INFO: {message}");
            Console.ResetColor();
        }

        public void LogError(string message, Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"ERROR: {message}\nException: {ex.Message}");
            Console.ResetColor();
        }

        public void LogWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"ALERTA: {message}");
            Console.ResetColor();
        }
    }
}