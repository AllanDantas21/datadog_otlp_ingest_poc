using Microsoft.Extensions.Logging;
using System;

public class App
{
    private readonly ILogger<App> _logger;

    public App(ILogger<App> logger)
    {
        _logger = logger;
    }

    public void Run()
    {
        Console.WriteLine("\n--- Escrevendo logs com _logger (Ambos Providers) ---");
        _logger.LogInformation("Log informativo enviado para ambos os providers!");
        _logger.LogWarning("Isso é um aviso simulado enviado para ambos os providers.");
        _logger.LogError(new Exception("Erro de teste"), "Falha enviada para ambos os providers.");
    }
}
