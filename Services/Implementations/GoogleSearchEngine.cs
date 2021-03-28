using HtmlAgilityPack;
using InfoTrackSearchEngineApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace InfoTrackSearchEngineApp.Services.Implementations
{
    public class GoogleSearchEngine : ISearchEngine
    {
        private readonly string _baseUrl = "https://infotrack-tests.infotrack.com.au/Google";
        private static readonly HttpClient client = new HttpClient();
        private readonly SearchConfigSettings _searchConfigSettings;

        public GoogleSearchEngine(SearchConfigSettings searchConfigSettings)
        {
            _searchConfigSettings = searchConfigSettings;
        }

        public async virtual Task<IEnumerable<int>> Search(string keyword, string url)
        {
            var response = new List<int>();
            var totalSearchResultNodesCounted = 0;

            for (var i = 1; i <= 10; ++i)
            {
                if (totalSearchResultNodesCounted >= _searchConfigSettings.MaxSearchResultsChecking)
                    break;

                var pageId = ((i < 10) ? "0" : "") + i;

                var fullRequestUrl = GetBaseUrl() + "/Page" + pageId + ".html";

                var responseMessage = await client.GetAsync(fullRequestUrl);
                var responseContent = await responseMessage.Content.ReadAsByteArrayAsync();
                var stringResponseContent = Encoding.GetEncoding("utf-8").GetString(responseContent, 0, responseContent.Length - 1);
                string htmlContent = WebUtility.HtmlDecode(stringResponseContent);

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(htmlContent);

                #region Check Google Ad results
                var searchEngineAdResultNodes = doc.DocumentNode.SelectNodes("//li[@class = 'ads-fr']");

                var resultIndex = totalSearchResultNodesCounted;

                if (searchEngineAdResultNodes != null)
                {
                    totalSearchResultNodesCounted += searchEngineAdResultNodes.Count;

                    foreach (var resultNode in searchEngineAdResultNodes)
                    {
                        var resultNodeHref = resultNode.SelectNodes("div/div/div/div/a").First().Attributes["href"];

                        if (resultNodeHref != null) 
                            if (resultNodeHref.Value.StartsWith(url, StringComparison.OrdinalIgnoreCase)
                                && (resultIndex < _searchConfigSettings.MaxSearchResultsChecking))
                                response.Add(resultIndex);
                        ++resultIndex;
                    }
                }

                #endregion

                #region Normal search results
                var searchEngineResultNodes = doc.DocumentNode.SelectNodes("//div[@class = 'g']");

                resultIndex = totalSearchResultNodesCounted;

                if (searchEngineResultNodes != null) { 
                    totalSearchResultNodesCounted += searchEngineResultNodes.Count;

                    foreach (var resultNode in searchEngineResultNodes)
                    {
                        var resultNodeHref = resultNode.SelectNodes("div/div/a").First().Attributes["href"];

                        if (resultNodeHref != null)
                            if (resultNodeHref.Value.StartsWith(url, StringComparison.OrdinalIgnoreCase)
                                && (resultIndex < _searchConfigSettings.MaxSearchResultsChecking))
                                response.Add(resultIndex);
                        ++resultIndex;
                    }
                }
                #endregion

            }

            return response;
        }

        public virtual string GetBaseUrl()
        {
            return _baseUrl;
        }
    }
}
