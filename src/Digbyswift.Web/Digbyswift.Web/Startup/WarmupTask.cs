#if NETSTANDARD2_1
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Digbyswift.Web.Startup
{
    public interface IStartupTask
    {
        Task ExecuteAsync(CancellationToken cancellationToken = default);
    }

    public class WarmupServicesStartupTask : IStartupTask
    {
        private readonly IServiceCollection _services;
        private readonly IServiceProvider _provider;
        public WarmupServicesStartupTask(IServiceCollection services, IServiceProvider provider)
        {
            _services = services;
            _provider = provider;
        }

        public Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            using (var scope = _provider.CreateScope())
            {
                foreach (var singleton in GetServices(_services))
                {
                    scope.ServiceProvider.GetServices(singleton);
                }
            }

            return Task.CompletedTask;
        }

        static IEnumerable<Type> GetServices(IServiceCollection services)
        {
            var set = new HashSet<Type>();
            
            foreach (var descriptor in services)
            {
                if (descriptor.ImplementationType == typeof(WarmupServicesStartupTask))
                    continue;

                if (descriptor.ServiceType.ContainsGenericParameters)
                    continue;
                
                var service = descriptor.ServiceType;
                if (set.Add(service)) yield return service;
            }
        }
    }

}
#endif