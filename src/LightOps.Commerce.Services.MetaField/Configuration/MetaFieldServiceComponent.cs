﻿using System.Collections.Generic;
using System.Linq;
using LightOps.Commerce.Services.MetaField.Api.Models;
using LightOps.Commerce.Services.MetaField.Api.Queries;
using LightOps.Commerce.Services.MetaField.Api.QueryHandlers;
using LightOps.Commerce.Services.MetaField.Api.Services;
using LightOps.Commerce.Services.MetaField.Domain.Mappers.V1;
using LightOps.Commerce.Services.MetaField.Domain.Services;
using LightOps.CQRS.Api.Queries;
using LightOps.DependencyInjection.Api.Configuration;
using LightOps.DependencyInjection.Domain.Configuration;
using LightOps.Mapping.Api.Mappers;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LightOps.Commerce.Services.MetaField.Configuration
{
    public class MetaFieldServiceComponent : IDependencyInjectionComponent, IMetaFieldServiceComponent
    {
        public string Name => "lightops.commerce.services.meta-field";

        public IReadOnlyList<ServiceRegistration> GetServiceRegistrations()
        {
            return new List<ServiceRegistration>()
                .Union(_services.Values)
                .Union(_mappers.Values)
                .Union(_queryHandlers.Values)
                .ToList();
        }

        #region Services
        internal enum Services
        {
            HealthService,
            MetaFieldService,
        }

        private readonly Dictionary<Services, ServiceRegistration> _services = new Dictionary<Services, ServiceRegistration>
        {
            [Services.HealthService] = ServiceRegistration.Transient<IHealthService, HealthService>(),
            [Services.MetaFieldService] = ServiceRegistration.Transient<IMetaFieldService, MetaFieldService>(),
        };

        public IMetaFieldServiceComponent OverrideHealthService<T>()
            where T : IHealthService
        {
            _services[Services.HealthService].ImplementationType = typeof(T);
            return this;
        }

        public IMetaFieldServiceComponent OverrideMetaFieldService<T>()
            where T : IMetaFieldService
        {
            _services[Services.MetaFieldService].ImplementationType = typeof(T);
            return this;
        }
        #endregion Services

        #region Mappers
        internal enum Mappers
        {
            ProtoMetaFieldMapperV1,
        }

        private readonly Dictionary<Mappers, ServiceRegistration> _mappers = new Dictionary<Mappers, ServiceRegistration>
        {
            [Mappers.ProtoMetaFieldMapperV1] = ServiceRegistration
                .Transient<IMapper<IMetaField, Proto.Services.MetaField.V1.ProtoMetaField>, ProtoMetaFieldMapper>(),
        };

        public IMetaFieldServiceComponent OverrideProtoMetaFieldMapperV1<T>() where T : IMapper<IMetaField, Proto.Services.MetaField.V1.ProtoMetaField>
        {
            _mappers[Mappers.ProtoMetaFieldMapperV1].ImplementationType = typeof(T);
            return this;
        }
        #endregion Mappers

        #region Query Handlers
        internal enum QueryHandlers
        {
            CheckMetaFieldHealthQueryHandler,
            FetchMetaFieldByParentQueryHandler,
            FetchMetaFieldsByParentQueryHandler,
        }

        private readonly Dictionary<QueryHandlers, ServiceRegistration> _queryHandlers = new Dictionary<QueryHandlers, ServiceRegistration>
        {
            [QueryHandlers.CheckMetaFieldHealthQueryHandler] = ServiceRegistration.Transient<IQueryHandler<CheckMetaFieldHealthQuery, HealthStatus>>(),
            [QueryHandlers.FetchMetaFieldByParentQueryHandler] = ServiceRegistration.Transient<IQueryHandler<FetchMetaFieldByParentQuery, IMetaField>>(),
            [QueryHandlers.FetchMetaFieldsByParentQueryHandler] = ServiceRegistration.Transient<IQueryHandler<FetchMetaFieldsByParentQuery, IList<IMetaField>>>(),
        };

        public IMetaFieldServiceComponent OverrideCheckMetaFieldHealthQueryHandler<T>() where T : ICheckMetaFieldHealthQueryHandler
        {
            _queryHandlers[QueryHandlers.CheckMetaFieldHealthQueryHandler].ImplementationType = typeof(T);
            return this;
        }

        public IMetaFieldServiceComponent OverrideIFetchMetaFieldByParentQueryHandler<T>() where T : IFetchMetaFieldByParentQueryHandler
        {
            _queryHandlers[QueryHandlers.FetchMetaFieldByParentQueryHandler].ImplementationType = typeof(T);
            return this;
        }

        public IMetaFieldServiceComponent OverrideIFetchMetaFieldsByParentQueryHandler<T>() where T : IFetchMetaFieldsByParentQueryHandler
        {
            _queryHandlers[QueryHandlers.FetchMetaFieldsByParentQueryHandler].ImplementationType = typeof(T);
            return this;
        }
        #endregion Query Handlers
    }
}