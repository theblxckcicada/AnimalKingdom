using AnimalKingdom.API.Handlers;
using AnimalKingdom.API.Models;
// using AnimalKingdom.Shared.Models;
using MediatR;

namespace AnimalKingdom.API.DependencyInjection;

internal static class IServiceCollectionExtensions
{
    public static IServiceCollection AddEntityHandlers(
        this IServiceCollection services,
        bool replaceExistingImplementations = false
    )
    {
        // TODO: Map all generic entity handlers here
        return services.AddEntityHandlers<Animal, Guid>(replaceExistingImplementations);
    }

    public static IServiceCollection AddEntityHandlers<TModel, TKey>(
        this IServiceCollection services,
        bool replaceExistingImplementations = false
    )
        where TModel : class, IEntityBase<TKey>
    {
        var modelType = typeof(TModel);
        var keyType = typeof(TKey);

        foreach (var definition in EntityHandlerDefinitions)
        {
            var serviceType = definition.BuildServiceType(modelType, keyType);

            var duplicate = services.FirstOrDefault(d => d.ServiceType == serviceType);
            if (duplicate != null && !replaceExistingImplementations)
            {
                continue;
            }

            if (duplicate != null)
            {
                services.Remove(duplicate);
            }

            var implementationType = definition.BuildImplementationType(modelType, keyType);
            var descriptor = new ServiceDescriptor(
                serviceType,
                implementationType,
                ServiceLifetime.Scoped
            );
            services.Add(descriptor);
        }

        return services;
    }

    private class EntityHandlerDefinition(
        Type handlerType,
        Type requestType,
        Type? responseType = null
    )
    {
        private static readonly Type GenericHandlerType = typeof(IRequestHandler<,>);
        private readonly Type handlerType = handlerType;
        private readonly Type requestType = requestType;
        private readonly Type? responseType = responseType;

        public Type BuildServiceType(Type modelType, Type keyType)
        {
            // Use the model type as the response type if not provided
            if (this.responseType == null)
            {
                return GenericHandlerType.MakeGenericType(
                    this.requestType.MakeGenericType(modelType, keyType),
                    modelType
                );
            }

            if (!this.responseType.IsGenericTypeDefinition)
            {
                // Use the actual response type if it is not a generic type definition
                return GenericHandlerType.MakeGenericType(
                    this.requestType.MakeGenericType(modelType, keyType),
                    this.responseType
                );
            }

            // Otherwise assume a single generic argument
            return GenericHandlerType.MakeGenericType(
                this.requestType.MakeGenericType(modelType, keyType),
                this.responseType.MakeGenericType(modelType)
            );
        }

        public Type BuildImplementationType(Type modelType, Type keyType)
        {
            return this.handlerType.MakeGenericType(modelType, keyType);
        }
    }

    private static readonly EntityHandlerDefinition[] EntityHandlerDefinitions =
        new EntityHandlerDefinition[]
        {
            new EntityHandlerDefinition(
                typeof(AddEntityHandler<,>),
                typeof(AddEntityRequest<,>),
                typeof(CommandResponse<>)
            ),
            new EntityHandlerDefinition(
                typeof(GetEntitiesHandler<,>),
                typeof(GetEntitiesRequest<,>),
                typeof(IList<>)
            ),
            new EntityHandlerDefinition(typeof(GetEntityHandler<,>), typeof(GetEntityRequest<,>)),
            new EntityHandlerDefinition(
                typeof(QueryEntitiesHandler<,>),
                typeof(QueryEntitiesRequest<,>),
                typeof(QueryEntitiesResponse<>)
            ),
            new EntityHandlerDefinition(
                typeof(RemoveEntityHandler<,>),
                typeof(RemoveEntityRequest<,>),
                typeof(CommandResponse<>)
            ),
            new EntityHandlerDefinition(
                typeof(UpdateEntityHandler<,>),
                typeof(UpdateEntityRequest<,>),
                typeof(CommandResponse<>)
            ),
        };
}
