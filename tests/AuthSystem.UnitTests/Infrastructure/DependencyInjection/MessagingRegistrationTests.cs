using System;
using System.Collections.Generic;
using System.Net.Http;
using AuthSystem.Application.Common.Abstractions.Messaging;
using AuthSystem.Infrastructure;
using AuthSystem.Infrastructure.Messaging.Email;
using AuthSystem.Infrastructure.Messaging.Sms;
using AuthSystem.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AuthSystem.UnitTests.Infrastructure.DependencyInjection;

public class MessagingRegistrationTests
{
    [Fact]
    public void EmailSender_Resolves_Smtp_When_Configured()
    {
        var provider = BuildServiceProvider(new Dictionary<string, string?>
        {
            [$"{EmailOptions.SectionName}:Provider"] = "smtp",
            [$"{EmailOptions.SectionName}:Host"] = "localhost",
            [$"{EmailOptions.SectionName}:Port"] = "25",
            [$"{EmailOptions.SectionName}:EnableSsl"] = "false",
            [$"{SmsOptions.SectionName}:Provider"] = "noop",
            ["ConnectionStrings:Default"] = "Server=(localdb)\\\MSSQLLocalDB;Database=AuthSystem;Trusted_Connection=True;MultipleActiveResultSets=True"
        });

        var sender = provider.GetRequiredService<IEmailSender>();

        Assert.IsType<SmtpEmailSender>(sender);
    }

    [Fact]
    public void EmailSender_Falls_Back_To_NoOp()
    {
        var provider = BuildServiceProvider(new Dictionary<string, string?>
        {
            [$"{EmailOptions.SectionName}:Provider"] = "noop",
            [$"{SmsOptions.SectionName}:Provider"] = "noop",
            ["ConnectionStrings:Default"] = "Server=(localdb)\\\MSSQLLocalDB;Database=AuthSystem;Trusted_Connection=True;MultipleActiveResultSets=True"
        });

        var sender = provider.GetRequiredService<IEmailSender>();

        Assert.IsType<NoOpEmailSender>(sender);
    }

    [Fact]
    public void SmsSender_Resolves_Http_When_Configured()
    {
        var provider = BuildServiceProvider(new Dictionary<string, string?>
        {
            [$"{EmailOptions.SectionName}:Provider"] = "noop",
            [$"{SmsOptions.SectionName}:Provider"] = "http",
            [$"{SmsOptions.SectionName}:Endpoint"] = "https://example.com/api/sms",
            [$"{SmsOptions.SectionName}:RetryCount"] = "0",
            ["ConnectionStrings:Default"] = "Server=(localdb)\\\MSSQLLocalDB;Database=AuthSystem;Trusted_Connection=True;MultipleActiveResultSets=True"
        });

        var sender = provider.GetRequiredService<ISmsSender>();

        Assert.IsType<ProviderSmsSender>(sender);
    }

    [Fact]
    public void SmsHttpClient_Respects_Timeout_And_Headers()
    {
        var provider = BuildServiceProvider(new Dictionary<string, string?>
        {
            [$"{EmailOptions.SectionName}:Provider"] = "noop",
            [$"{SmsOptions.SectionName}:Provider"] = "http",
            [$"{SmsOptions.SectionName}:Endpoint"] = "https://example.com/api/sms",
            [$"{SmsOptions.SectionName}:TimeoutSeconds"] = "10",
            [$"{SmsOptions.SectionName}:Headers:Authorization"] = "Bearer token",
            ["ConnectionStrings:Default"] = "Server=(localdb)\\\MSSQLLocalDB;Database=AuthSystem;Trusted_Connection=True;MultipleActiveResultSets=True"
        });

        var factory = provider.GetRequiredService<IHttpClientFactory>();
        var client = factory.CreateClient("sms");

        Assert.Equal(TimeSpan.FromSeconds(10), client.Timeout);
        Assert.True(client.DefaultRequestHeaders.Contains("Authorization"));
    }

    private static ServiceProvider BuildServiceProvider(IDictionary<string, string?> values)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(values)
            .Build();

        var services = new ServiceCollection();
        services.AddLogging();

        var environment = new TestHostEnvironment();
        services.AddSingleton<IHostEnvironment>(environment);
        services.AddInfrastructure(configuration, environment);

        return services.BuildServiceProvider(validateScopes: true);
    }

    private sealed class TestHostEnvironment : IHostEnvironment
    {
        public string EnvironmentName { get; set; } = Environments.Development;
        public string ApplicationName { get; set; } = "AuthSystem";
        public string ContentRootPath { get; set; } = AppContext.BaseDirectory;
    }
}