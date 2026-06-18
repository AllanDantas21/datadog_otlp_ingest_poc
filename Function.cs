using Amazon.Lambda.Core;
using Microsoft.Extensions.Logging;
using System;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

public class Function
{
 
    public string FunctionHandler(string input, ILambdaContext context)
    {
        // AWS Lambda logger can be used to log to CloudWatch
        context.Logger.LogInformation($"Lambda function invoked with input: '{input}'");

        using var loggerFactory = BaseFunction.CreateLoggerFactory();
        var logger = loggerFactory.CreateLogger<App>();
        var app = new App(logger);
        app.Run();

        context.Logger.LogInformation("Lambda execution completed successfully.");
        return $"Execution successful. Input was: '{input}'";
    }
}
