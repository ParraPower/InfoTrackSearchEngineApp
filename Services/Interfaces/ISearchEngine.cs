using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoTrackSearchEngineApp.Services.Interfaces
{
    public interface ISearchEngine
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public Task<IEnumerable<int>> Search(string keyword, string url);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetBaseUrl();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetFriendlyName();
    }
}
