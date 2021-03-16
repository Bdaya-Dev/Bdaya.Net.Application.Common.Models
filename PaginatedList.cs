using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Bdaya.Net.Application.Common.Models
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }


        [JsonConstructor]
        public PaginatedList()
        {
                
        }
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
            => (PageIndex, TotalPages, TotalCount, Items, HasPreviousPage, HasNextPage) =
            (pageIndex, (int)Math.Ceiling(count / (double)pageSize), count, items, PageIndex > 1, PageIndex < TotalPages);


        public bool HasPreviousPage { get; set; }

        public bool HasNextPage { get; set; }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
