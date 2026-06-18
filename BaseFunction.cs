using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using Serilog;
using System;
using System.Collections.Generic;

public class BaseFunction
{
    public static ILoggerFactory CreateLoggerFactory()
    {
        var apiKey = Environment.GetEnvironmentVariable("DD_API_KEY");
        var endpoint = Environment.GetEnvironmentVariable("OTLP_ENDPOINT") ?? "https://otlp.datadoghq.com/v1/logs";

        var serilogLogger = new LoggerConfiguration()
            .WriteTo.Console(outputTemplate: "[Serilog] [{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        return LoggerFactory.Create(builder =>
        {
            builder.AddSerilog(serilogLogger);

            builder.AddOpenTelemetry(options =>
            {
                options.SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                        .AddService("meu-app-dotnet-otel")
                        .AddAttributes(new Dictionary<string, object>
                        {
                            ["env"] = "Test",
                            ["host.name"] = "Simulate_otel_logs"
                        }));

                options.AddOtlpExporter(otlpOptions =>
                {
                    otlpOptions.Endpoint = new Uri(endpoint);
                    otlpOptions.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
                    otlpOptions.Headers = $"dd-api-key={apiKey}";
                });
            });
        });
    }
}
