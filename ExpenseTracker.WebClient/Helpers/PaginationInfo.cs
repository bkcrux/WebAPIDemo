using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpenseTracker.WebClient.Helpers
{
    public class PaginationInfo
    {
        public int TotalPage { get; set; }
        public int TotalCount { get; set; }

        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public string NextPageLink { get; set; }
        public string PreviousPageLink { get; set; }


        public PaginationInfo(int totalCount, int totalPage, int currentPage, int pageSize,
             string previousPageLink, string nextPageLink)
        {
            this.TotalCount = totalCount;
            this.TotalPage = totalPage;
            this.CurrentPage = currentPage;
            this.PageSize = pageSize;
            this.NextPageLink = nextPageLink;
            this.PreviousPageLink = previousPageLink;
        }
    }
}