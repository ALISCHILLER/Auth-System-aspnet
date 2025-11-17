using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace AuthSystem.Api.Extensions;

internal static class ServiceCollectionDecoratorExtensions
{
    public static IServiceCollection Decorate<TService, TDecorator>(this IServiceCollection services)
        where TService : class
        where TDecorator : class, TService
    {
        ArgumentNullException.ThrowIfNull(services);

        var descriptor = services.LastOrDefault(service => service.ServiceType == typeof(TService))
            ?? throw new InvalidOperationException($"Service type {typeof(TService)} is not registered and cannot be decorated.");

        ServiceDescriptor decoratedDescriptor = descriptor switch
        {
            { ImplementationFactory: not null } => ServiceDescriptor.Describe(
                typeof(TService),
                provider => CreateDecorator(provider, descriptor.ImplementationFactory!(provider)),
                descriptor.Lifetime),
            { ImplementationInstance: not null } => ServiceDescriptor.Describe(
                typeof(TService),
                provider => CreateDecorator(provider, descriptor.ImplementationInstance!),
                descriptor.Lifetime),
            _ when descriptor.ImplementationType is not null => ServiceDescriptor.Describe(
                typeof(TService),
                provider =>
                {
                    var original = ActivatorUtilities.CreateInstance(provider, descriptor.ImplementationType!);
                    return CreateDecorator(provider, original);
                },
                descriptor.Lifetime),
            _ => throw new InvalidOperationException("Unsupported service descriptor")
        };

        services.Remove(descriptor);
        services.Add(decoratedDescriptor);

        return services;

        TService CreateDecorator(IServiceProvider provider, object inner)
            => ActivatorUtilities.CreateInstance<TDecorator>(provider, inner);
    }
}