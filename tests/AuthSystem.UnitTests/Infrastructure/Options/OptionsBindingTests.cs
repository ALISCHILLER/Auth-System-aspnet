using System;
using System.IO;
using AuthSystem.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AuthSystem.UnitTests.Infrastructure.Options;

public class OptionsBindingTests
{
    [Fact]
    public void EmailOptions_Binds_From_AppSettings()
    {
        var serviceProvider = BuildServiceProvider();

        var options = serviceProvider.GetRequiredService<IOptionsMonitor<EmailOptions>>().CurrentValue;

        Assert.Equal("noop", options.Provider);
        Assert.Equal("no-reply@example.com", options.From);
        Assert.Equal("Auth System", options.DisplayName);
        Assert.Equal("localhost", options.Host);
        Assert.Equal(25, options.Port);
        Assert.False(options.EnableSsl);
    }

    [Fact]
    public void SmsOptions_Binds_From_AppSettings()
    {
        var serviceProvider = BuildServiceProvider();

        var options = serviceProvider.GetRequiredService<IOptionsMonitor<SmsOptions>>().CurrentValue;

        Assert.Equal("noop", options.Provider);
        Assert.Equal("https://example.com/api/sms", options.Endpoint);
        Assert.Equal("+10000000000", options.FromNumber);
        Assert.Collection(options.Headers,
            header =>
            {
                Assert.Equal("X-Tenant", header.Key);
                Assert.Equal("dev", header.Value);
            });
        Assert.Equal(30, options.TimeoutSeconds);
        Assert.Equal(3, options.RetryCount);
    }

    private static ServiceProvider BuildServiceProvider()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(GetApiProjectDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var services = new ServiceCollection();
        services.Configure<EmailOptions>(configuration.GetSection(EmailOptions.SectionName));
        services.Configure<SmsOptions>(configuration.GetSection(SmsOptions.SectionName));

        return services.BuildServiceProvider();
    }

    private static string GetApiProjectDirectory()
    {
        var assemblyPath = AppContext.BaseDirectory;
        return Path.GetFullPath(Path.Combine(assemblyPath, "..", "..", "..", "..", "src", "AuthSystem.Api"));
    }
}