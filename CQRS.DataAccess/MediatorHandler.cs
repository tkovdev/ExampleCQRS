using System.Reflection;
using CQRS.DataAccess.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CQRS.DataAccess;

public static class MediatorHandler
{
    public static void RegisterHandlers(IServiceCollection services)
    {
        var commandAssembly = Assembly.Load("CQRS.Commands"); // Or specify your handler assembly

        // Register command handlers
        var commandHandlerTypes = commandAssembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)))
            .ToList();

        foreach (var handlerType in commandHandlerTypes)
        {
            var interfaceType = handlerType.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>));
            services.AddScoped(interfaceType, handlerType);
        }

        var queryAssembly = Assembly.Load("CQRS.Queries"); // Or specify your handler assembly
        // Register query handlers
        var queryHandlerTypes = queryAssembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)))
            .ToList();

        foreach (var queryHandlerType in queryHandlerTypes)
        {
            var interfaceType = queryHandlerType.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>));
            services.AddScoped(interfaceType, queryHandlerType);
        }
    }
}