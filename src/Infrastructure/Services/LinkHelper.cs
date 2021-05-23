using Core.Models;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class LinkHelper
    {
        private readonly string _hostname;
        public LinkHelper(string hostname)
        {
            _hostname = hostname;
        }

        public PagedResponse<T> BuildPaginationResponse<T>(IEnumerable<T> data, int totalCount, int page, int limit, string path)
        {
            int firstPageIdx = 1;
            int lastPageIdx = (int)Math.Ceiling((double)totalCount / (double)limit);
            int prevPageIdx = (page > 1) ? page - 1 : firstPageIdx;
            int nextPageIdx = (page < lastPageIdx) ? page + 1 : lastPageIdx;

            Dictionary<string, string> queryParams = new Dictionary<string, string>
            {
                ["limit"] = limit.ToString()
            };

            Uri baseUri = new Uri(_hostname);
            queryParams["page"] = firstPageIdx.ToString();
            string firstPageUrl = new Uri(baseUri, QueryHelpers.AddQueryString(path, queryParams)).AbsoluteUri;

            queryParams["page"] = lastPageIdx.ToString();
            string lastPageUrl = new Uri(baseUri, QueryHelpers.AddQueryString(path, queryParams)).AbsoluteUri;

            queryParams["page"] = prevPageIdx.ToString();
            string prevPageUrl = new Uri(baseUri, QueryHelpers.AddQueryString(path, queryParams)).AbsoluteUri;

            queryParams["page"] = nextPageIdx.ToString();
            string nextPageUrl = new Uri(baseUri, QueryHelpers.AddQueryString(path, queryParams)).AbsoluteUri;

            return new PagedResponse<T>
            {
                FirstPage = firstPageUrl,
                LastPage = lastPageUrl,
                PrevPage = prevPageUrl,
                NextPage = nextPageUrl,
                Pages = lastPageIdx,
                TotalCount = totalCount,
                Data = data
            };
        }
    }
}
