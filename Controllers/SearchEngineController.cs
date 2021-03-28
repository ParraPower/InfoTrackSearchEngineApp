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
    [Route("[controller]")]
    public class SearchEngineController : ControllerBase
    {
        private readonly ILogger<SearchEngineController> _logger;

        private readonly ISearchEngineServiceResolver _searchEngineServiceResolver;

        public SearchEngineController(ISearchEngineServiceResolver searchEngineServiceResolver, ILogger<SearchEngineController> logger)
        {
            _logger = logger;
            _searchEngineServiceResolver = searchEngineServiceResolver;
        }
        
        [HttpGet]
        public Task<IEnumerable<int>> Get(string searchEngineName, string keyword, string url)
        {
            var searchEngineService = _searchEngineServiceResolver.Resolve(searchEngineName);

            return searchEngineService.Search(keyword, url);
        }
    }
}
