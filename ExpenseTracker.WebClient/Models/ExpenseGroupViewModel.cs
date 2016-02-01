using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExpenseTracker.DTO;
using PagedList;
using ExpenseTracker.WebClient.Helpers;

namespace ExpenseTracker.WebClient.Models
{
    public class ExpenseGroupViewModel
    {
        public IPagedList<ExpenseGroup> ExpenseGroups { get; set; }
        public IEnumerable<ExpenseGroupStatus> ExpenseGroupStatuses { get; set; }
        public PaginationInfo paginationInfo { get; set; }
    }


}