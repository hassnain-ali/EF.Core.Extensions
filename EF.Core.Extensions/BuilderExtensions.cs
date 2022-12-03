using Microsoft.Extensions.DependencyInjection;
using System;

namespace EF.Core.Extensions;

#if NET6_0_OR_GREATER
public static class BuilderExtensions
{
    public static IServiceCollection AddIf<Tservice, Timplimentation>(this IServiceCollection services,
        bool condition,
        ServiceLifetime? lifetime = ServiceLifetime.Singleton)
        where Tservice : class
        where Timplimentation : class, Tservice => condition
            ? lifetime switch
            {
                ServiceLifetime.Singleton => services.AddSingleton<Tservice, Timplimentation>(),
                ServiceLifetime.Scoped => services.AddScoped<Tservice, Timplimentation>(),
                ServiceLifetime.Transient => services.AddTransient<Tservice, Timplimentation>(),
                _ => services.AddSingleton<Tservice, Timplimentation>()
            }
            : services;
    public static IServiceCollection AddIf<Tservice>(this IServiceCollection services,
        bool condition,
        ServiceLifetime? lifetime = ServiceLifetime.Singleton)
        where Tservice : class => condition
            ? lifetime switch
            {
                ServiceLifetime.Singleton => services.AddSingleton<Tservice>(),
                ServiceLifetime.Scoped => services.AddScoped<Tservice>(),
                ServiceLifetime.Transient => services.AddTransient<Tservice>(),
                _ => services.AddSingleton<Tservice>()
            }
            : services;
    public static IServiceCollection AddIf<TService>(this IServiceCollection services,
        bool condition,
        Func<IServiceProvider, TService> implementationFactory,
        ServiceLifetime? lifetime = ServiceLifetime.Singleton)
        where TService : class => condition
            ? lifetime switch
            {
                ServiceLifetime.Singleton => services.AddSingleton(implementationFactory),
                ServiceLifetime.Scoped => services.AddScoped(implementationFactory),
                ServiceLifetime.Transient => services.AddTransient(implementationFactory),
                _ => services.AddSingleton(implementationFactory)
            }
            : services;
    public static IServiceCollection AddIf<TService>(this IServiceCollection services,
        bool condition,
        Type type,
        ServiceLifetime? lifetime = ServiceLifetime.Singleton)
        where TService : class => condition
            ? lifetime switch
            {
                ServiceLifetime.Singleton => services.AddSingleton(type),
                ServiceLifetime.Scoped => services.AddScoped(type),
                ServiceLifetime.Transient => services.AddTransient(type),
                _ => services.AddSingleton(type)
            }
            : services;
    public static IServiceCollection AddIf<TService>(this IServiceCollection services,
    bool condition,
    Type serviceType,
    Type implementationType,
    ServiceLifetime? lifetime = ServiceLifetime.Singleton)
    where TService : class => condition
            ? lifetime switch
            {
                ServiceLifetime.Singleton => services.AddSingleton(serviceType, implementationType),
                ServiceLifetime.Scoped => services.AddScoped(serviceType, implementationType),
                ServiceLifetime.Transient => services.AddTransient(serviceType, implementationType),
                _ => services.AddSingleton(serviceType, implementationType)
            }
            : services;

}

#endif