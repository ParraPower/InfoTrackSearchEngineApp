using InfoTrackSearchEngineApp.Services.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace InfoTrackSearchEngineApp.Infrastructure
{
    public class ConfigureServicesHelper 
    {
        public void AddTransientServices(IServiceCollection services)
        {
            services.AddTransient(typeof(GoogleSearchEngine));
        }

        public void AddSingletonConfigs(IConfiguration configuration, IServiceCollection services)
        {
            SearchConfigSettings searchConfigSettings = new SearchConfigSettings();
            configuration.GetSection("SearchConfigSettings").Bind(searchConfigSettings);

            //Create singleton from instance
            services.AddSingleton<SearchConfigSettings>(searchConfigSettings);
        }
    }
}
