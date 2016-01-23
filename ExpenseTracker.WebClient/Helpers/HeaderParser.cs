using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;

namespace ExpenseTracker.WebClient.Helpers
{
    public static class HeaderParser
    {
        public static PaginationInfo FindAndParsePaginationInfo(HttpResponseHeaders responseHeaders)
        {
            //find x-pagination
            if (responseHeaders.Contains("X-Pagination"))
            {
                var xPag = responseHeaders.First(rh => rh.Key == "X-Pagination").Value;
                return JsonConvert.DeserializeObject<PaginationInfo>(xPag.First());

            }

            return null;
        }
    }
}