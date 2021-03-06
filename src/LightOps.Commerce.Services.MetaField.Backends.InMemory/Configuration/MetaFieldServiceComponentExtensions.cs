﻿using System;
using LightOps.Commerce.Services.MetaField.Backends.InMemory.Domain.CommandHandlers;
using LightOps.Commerce.Services.MetaField.Backends.InMemory.Domain.QueryHandlers;
using LightOps.Commerce.Services.MetaField.Configuration;
using LightOps.DependencyInjection.Configuration;

namespace LightOps.Commerce.Services.MetaField.Backends.InMemory.Configuration
{
    public static class MetaFieldServiceComponentExtensions
    {
        public static IMetaFieldServiceComponent UseInMemoryBackend(
            this IMetaFieldServiceComponent serviceComponent,
            IDependencyInjectionRootComponent rootComponent,
            Action<IInMemoryMetaFieldServiceBackendComponent> componentConfig = null)
        {
            var component = new InMemoryMetaFieldServiceBackendComponent();

            // Invoke config delegate
            componentConfig?.Invoke(component);

            // Attach to root component
            rootComponent.AttachComponent(component);

            // Override command handlers
            serviceComponent
                .OverridePersistMetaFieldCommandHandler<PersistMetaFieldCommandHandler>()
                .OverrideDeleteMetaFieldCommandHandler<DeleteMetaFieldCommandHandler>();

            // Override query handlers
            serviceComponent
                .OverrideCheckMetaFieldServiceHealthQueryHandler<CheckMetaFieldServiceHealthQueryHandler>()
                .OverrideFetchMetaFieldsByIdsQueryHandler<FetchMetaFieldsByIdsQueryHandler>()
                .OverrideFetchMetaFieldsByParentIdsQueryHandler<FetchMetaFieldsByParentIdsQueryHandler>()
                .OverrideFetchMetaFieldsBySearchQueryHandler<FetchMetaFieldsBySearchQueryHandler>();

            return serviceComponent;
        }
    }
}
