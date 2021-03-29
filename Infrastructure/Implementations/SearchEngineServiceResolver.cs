using InfoTrackSearchEngineApp.Infrastructure.Interfaces;
using InfoTrackSearchEngineApp.Services.Implementations;
using InfoTrackSearchEngineApp.Services.Interfaces;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace InfoTrackSearchEngineApp.Infrastructure.Implementations
{
    public class SearchEngineServiceResolver : ISearchEngineServiceResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public SearchEngineServiceResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ISearchEngine Resolve(string name)
        {
            return GetServices().FirstOrDefault(searchEngine => searchEngine.GetFriendlyName() == name);
        }

        private IEnumerable<Type> GetAllTypesThatImplementInterface()
        {
            return Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type => typeof(ISearchEngine).IsAssignableFrom(type) && !type.IsInterface);
        }

        public IEnumerable<ISearchEngine> GetServices()
        {
            var types = GetAllTypesThatImplementInterface();
            var list = new List<ISearchEngine>();

            foreach (var type in types)
                list.Add((ISearchEngine)_serviceProvider.GetService(type));

            return list.ToList();
        }
    }
}
