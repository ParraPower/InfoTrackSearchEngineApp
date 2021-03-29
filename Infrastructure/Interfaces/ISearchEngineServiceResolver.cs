using InfoTrackSearchEngineApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoTrackSearchEngineApp.Infrastructure.Interfaces
{
    public interface ISearchEngineServiceResolver
    {
        public ISearchEngine Resolve(string name);

        public IEnumerable<ISearchEngine> GetServices();
    }
}
