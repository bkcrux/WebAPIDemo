using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Dynamic;

namespace ExpenseTracker.API.Helpers
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string sort)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (sort == null)
            {
                return source;
            }

            //split sort string
            var lstSort = sort.Split(',');

            //run through the sorting options and create a sort expression string from them
            string completeSortExpression = "";
            foreach (var s in lstSort)
            {
                //if sort option starts with a minus -, then sort desc, otherwise asc
                if (s.StartsWith("-"))
                {
                    completeSortExpression = completeSortExpression + s.Remove(0, 1) + " descending,";
                }
                else
                {
                    completeSortExpression = completeSortExpression + s + ",";
                }
            }

            if (!string.IsNullOrWhiteSpace(completeSortExpression))
            {
                source = source.OrderBy(completeSortExpression
                    .Remove(completeSortExpression.Count() - 1));

            }

            return source;
        }
    }
}