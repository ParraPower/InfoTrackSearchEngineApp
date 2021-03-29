using HtmlAgilityPack;
using InfoTrackSearchEngineApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InfoTrackSearchEngineApp.Services.Implementations
{
    public class BingSearchEngine : ISearchEngine
    {
        private readonly string _baseUrl = "https://infotrack-tests.infotrack.com.au/Bing";
        private static readonly HttpClient client = new HttpClient();
        private readonly SearchConfigSettings _searchConfigSettings;

        public BingSearchEngine(SearchConfigSettings searchConfigSettings)
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

                #region Check Bing Ad results
                var searchEngineAdResultNodes = doc.DocumentNode.SelectNodes("//li[@class = 'b_ad']");

                var resultIndex = totalSearchResultNodesCounted;

                if (searchEngineAdResultNodes != null)
                {
                    totalSearchResultNodesCounted += searchEngineAdResultNodes.Count;

                    foreach (var resultNode in searchEngineAdResultNodes)
                    {
                        var resultNodeHyperLink = resultNode.SelectNodes("//cite").FirstOrDefault();

                        if (resultNodeHyperLink != null)
                            if (resultNodeHyperLink.InnerText.StartsWith(url, StringComparison.OrdinalIgnoreCase)
                                && (resultIndex < _searchConfigSettings.MaxSearchResultsChecking))
                                response.Add(resultIndex);

                        ++resultIndex;
                    }
                }
                #endregion

                #region Normal search results
                var searchEngineResultNodes = doc.DocumentNode.SelectNodes("//li[@class = 'b_algo']");

                resultIndex = totalSearchResultNodesCounted;

                if (searchEngineResultNodes != null) { 
                    totalSearchResultNodesCounted += searchEngineResultNodes.Count;

                    foreach (var resultNode in searchEngineResultNodes)
                    {
                        var resultNodeAnchor = resultNode.SelectNodes("h2/a");

                        if (resultNodeAnchor == null)
                            resultNodeAnchor = resultNode.SelectNodes("//a");

                        if (resultNodeAnchor != null)
                            if (resultNodeAnchor.First().Attributes["href"].Value.StartsWith(url, StringComparison.OrdinalIgnoreCase)
                                && (resultIndex < _searchConfigSettings.MaxSearchResultsChecking))
                                response.Add(resultIndex);
                        ++resultIndex;
                    }
                }
                #endregion

                #region Ad bottom search results
                var searchEngineAdBottomResultNodes = doc.DocumentNode.SelectNodes("//li[@class = 'b_ad b_adBottom']/ul/li");

                resultIndex = totalSearchResultNodesCounted;

                if (searchEngineAdBottomResultNodes != null)
                {
                    totalSearchResultNodesCounted += searchEngineAdBottomResultNodes.Count;

                    foreach (var resultNode in searchEngineAdBottomResultNodes)
                    {
                        var resultNodeHyperLink = resultNode.SelectNodes("//cite").FirstOrDefault();

                        if (resultNodeHyperLink != null)
                            if (resultNodeHyperLink.InnerText.StartsWith(url, StringComparison.OrdinalIgnoreCase)
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetFriendlyName()
        {
            return "Bing";
        }
    }
}
