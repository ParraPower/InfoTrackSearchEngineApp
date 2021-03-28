using InfoTrackSearchEngineApp.Infrastructure.Interfaces;
using InfoTrackSearchEngineApp.Services.Implementations;
using InfoTrackSearchEngineApp.Services.Interfaces;
using System;

namespace InfoTrackSearchEngineApp.Infrastructure.Implementations
{
    public class SearchEngineServiceResolver : ISearchEngineServiceResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public SearchEngineServiceResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ISearchEngine Resolve(string name = "Google")
        {
            if (name == "Google")
                return (ISearchEngine)_serviceProvider.GetService(typeof(GoogleSearchEngine));
            //... other condition
            else
                return null;
        }
    }
}
