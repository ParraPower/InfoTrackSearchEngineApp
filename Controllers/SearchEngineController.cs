using InfoTrackSearchEngineApp.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoTrackSearchEngineApp.Controllers
{
    [ApiController]
    
    public class SearchEngineController : ControllerBase
    {
        private readonly ILogger<SearchEngineController> _logger;
        private readonly ISearchEngineServiceResolver _searchEngineServiceResolver;
        private readonly SearchConfigSettings _searchConfigSettings;

        public SearchEngineController(ISearchEngineServiceResolver searchEngineServiceResolver, SearchConfigSettings searchConfigSettings, ILogger<SearchEngineController> logger)
        {
            _logger = logger;
            _searchEngineServiceResolver = searchEngineServiceResolver;
            _searchConfigSettings = searchConfigSettings;
        }
        
        [Route("[controller]/list")]
        [HttpGet]
        public IEnumerable<SearchEngineViewModel> GetSearchEngines()
        {
            var searchEngineServices = _searchEngineServiceResolver.GetServices();

            return searchEngineServices.Select(services =>
            {
                return new SearchEngineViewModel()
                {
                    Name = services.GetFriendlyName(),
                    IsDefault = services.GetFriendlyName() == _searchConfigSettings.DefaultSearchEngineName
                };
            });
        }

        [Route("[controller]")]
        [HttpGet]
        public Task<IEnumerable<int>> Get(string searchEngineName, string keyword, string url)
        {
            var searchEngineService = _searchEngineServiceResolver.Resolve(searchEngineName);

            return searchEngineService.Search(keyword, url);
        }
    }
}
