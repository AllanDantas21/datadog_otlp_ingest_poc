using Microsoft.Extensions.Logging;
using System;

// 1. Configura a LoggerFactory contendo ambos os providers (Serilog e OpenTelemetry)
using var loggerFactory = BaseFunction.CreateLoggerFactory();
var logger = loggerFactory.CreateLogger<App>();

Console.WriteLine("Inicializando aplicação com o ILogger contendo ambos os providers...");

var app = new App(logger);
app.Run();

Console.WriteLine("\nPrograma finalizado. Verifique o console para os logs do Serilog e o Log Explorer no Datadog para os logs do OpenTelemetry!");