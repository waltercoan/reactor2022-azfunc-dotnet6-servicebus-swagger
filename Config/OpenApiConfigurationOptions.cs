using System;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;

namespace azfunc.Config;

public class OpenApiConfigurationOptions : DefaultOpenApiConfigurationOptions
{
    public override OpenApiInfo Info { get; set; } = new OpenApiInfo()
    {
        Version = "1.0.0",
        Title = $"API de votação em músicas - Worker Runtime: {Environment.GetEnvironmentVariable("FUNCTIONS_WORKER_RUNTIME")}",
        Description = "API de votação em músicas",
        Contact = new OpenApiContact()
        {
            Name = "Walter Coan",
            Url = new Uri("https://github.com/waltercoan"),
        },
        License = new OpenApiLicense()
        {
            Name = "MIT",
            Url = new Uri("http://opensource.org/licenses/MIT"),
        }
    };

    public override OpenApiVersionType OpenApiVersion { get; set; } = OpenApiVersionType.V3;
}